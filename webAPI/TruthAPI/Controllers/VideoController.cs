using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TruthAPI.Hubs;
using TruthAPI.ViewModels;
using Utility;
using VideoService.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TruthAPI.Controllers
{
    [Route("api/[controller]")]
    public class VideoController : Controller
    {
        private readonly string _videoPath;
        private IVideoService _videoService;
        private readonly IHubContext<VideoHub, IVideoHub> _videoHub;
        private readonly IHubContext<ManagementHub, IManagementHub> _managementHub;
        private IRepository<Category> _categoryRepository;
        private IRepository<ClientIdentity> _clientIdentityRepository;

        private string _rootPath;

        public VideoController(
            IHostingEnvironment hostingEnvironment,
            IVideoService videoService,
            IRepository<Category> categoryRepository,
            IRepository<ClientIdentity> clientIdentityRepository,
            IHubContext<VideoHub, IVideoHub> videoHub,
            IHubContext<ManagementHub, IManagementHub> managementHub)
        {
            _rootPath = hostingEnvironment.WebRootPath ?? hostingEnvironment.ContentRootPath;
            _videoPath = $"{_rootPath}\\Videos";

            _videoHub = videoHub;
            _managementHub = managementHub;

            var categories = InitCategoryRepository(categoryRepository);

            var clientIdentities = InitClientIdentities(clientIdentityRepository);

            InitVideoService(videoService, categories, clientIdentities);
        }

        private IEnumerable<Category> InitCategoryRepository(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryRepository.Init(_rootPath);
            var categories = _categoryRepository.GetAll();
            return categories;
        }

        private IEnumerable<ClientIdentity> InitClientIdentities(IRepository<ClientIdentity> clientIdentityRepository)
        {
            _clientIdentityRepository = clientIdentityRepository;
            _clientIdentityRepository.Init(_rootPath);
            var identities = _clientIdentityRepository.GetAll();
            return identities;
        }

        private void InitVideoService(IVideoService videoService, IEnumerable<Category> categories, IEnumerable<ClientIdentity> clientIdentities)
        {
            _videoService = videoService;
            _videoService.Init(_videoPath, categories, clientIdentities);
            ManagementHub.AddNotifyEvent((sender, args) =>
            {
                _managementHub.Clients.All.GetOnlineUsers(Utility.VideoUtility.GetAllClientInfo().Select(r => new ClientIdentityViewModel
                {
                    Id = r.Id,
                    IsActive = r.IsActive,
                    IsOnline = r.IsOnline,
                }));
            });
            VideoHub.AddConnectedEvent((senger, args) =>
            {
                ManagementHub.DoNotifyEvent();
            });

            VideoHub.AddDisconnectedEvent((senger, args) =>
            {
                ManagementHub.DoNotifyEvent();
            });
        }

        [HttpGet("SearchVideos")]
        public IActionResult SearchVideos(
            List<int> categoryIds, DateTime? startDate, DateTime? endDate)
        {
            return new JsonResult(_videoService
                .SearchVideos(categoryIds, startDate, endDate, _videoPath)
                .Select(VideoToViewModel));
        }

        [HttpPost("SetVideo")]
        public async Task<IActionResult> SetVideo([FromBody] SetVideoViewModel setVideoParams)
        {
            var ipInfo = GetIpInfo(setVideoParams.Id);

            if (ipInfo == null)
                return new JsonResult("No client.");

            var ip = ipInfo.Ip;

            _videoService.SetVideos(setVideoParams.Codes, ip, _videoPath);

            var connectionId = VideoUtility.GetConnectionIdByIp(ip);
            VideoUtility.UpdateActiveStatus(ip, true);

            if (string.IsNullOrWhiteSpace(connectionId))
                return new JsonResult("No online client.");

            await SetClientVideo(connectionId, ip);

            return new JsonResult("Ok");
        }

        [HttpPost("CleanVideo")]
        public async Task<IActionResult> CleanVideo([FromBody] CleanVideoViewModel cleanVideoParams)
        {
            var ipInfo = GetIpInfo(cleanVideoParams.Id);

            if (ipInfo == null)
                return new JsonResult("No client.");

            var ip = ipInfo.Ip;

            _videoService.CleanVideo(ip);

            var connectionId = VideoUtility.GetConnectionIdByIp(ip);
            VideoUtility.UpdateActiveStatus(ip, false);

            await SetClientVideo(connectionId, ip);

            return new JsonResult("Ok");
        }

        [HttpGet("GetVideoList")]
        public IActionResult GetVideoList()
        {
            return new JsonResult(_videoService
                .GetVideoListByIp(GetCallerIp())
                .Select(VideoToViewModel));
        }

        [HttpGet("PlayVideo")]
        public IActionResult PlayVideo(string code)
        {
            return Redirect(_videoService.GetVideoByCode(code, _videoPath, GetCallerIp()));
        }

        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            return new JsonResult(_videoService.GetCategories()
                .Select(s => new CategoryViewModel
                {
                    Id = s.Id,
                    DisplayName = s.DisplayName
                }));
        }

        [HttpGet("GetClientIdentities")]
        public IActionResult GetClientIdentities()
        {
            return new JsonResult(_videoService
                .GetClientIdentities()
                .Select(s => new ClientIdentityViewModel
                {
                    Id = s.Id,
                    IsActive = s.IsActive,
                    IsOnline = s.IsOnline,
                }));
        }

        protected virtual string GetCallerIp()
        {
            return Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private async Task SetClientVideo(string connectionId, string ip)
        {
            await _videoHub.Clients.Client(connectionId)
                .PlayVideo(_videoService.GetVideoListByIp(ip)
                    .Select(VideoToViewModel));
        }

        private ClientIdentity GetIpInfo(string id)
        {
            return _videoService.GetClientIdentities()
                .FirstOrDefault(i => i.Id.ToString() == id);
        }

        private VideoViewModel VideoToViewModel(Video video)
        {
            return new VideoViewModel
            {
                Id = video.CategoryId,
                Name = video.Name,
                Date = video.Date,
                CategoryName = video.CategoryName,
                Code = video.Code
            };
        }
    }
}