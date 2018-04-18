using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Repository.Interface;
using TruthAPI.Hubs;
using TruthAPI.ViewModels;
using Utility;
using VideoService.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TruthAPI.Controllers
{
    public class SetParams
    {
        public string Id { get; set; }
        public List<string> Codes { get; set; }
    }

    [Route("api/[controller]")]
    public class VideoController : Controller
    {
        private readonly string _videoPath;
        private IVideoService _videoService;
        private readonly IHubContext<VideoHub, IVideoHub> _videoHub;
        private readonly IHubContext<ManagementHub, IManagementHub> _managementHub;
        private IRepository<Category> _categoryRepository;

        private string _rootPath;

        public VideoController(
            IHostingEnvironment hostingEnvironment,
            IVideoService videoService,
            IRepository<Category> categoryRepository,
            IHubContext<VideoHub, IVideoHub> videoHub,
            IHubContext<ManagementHub, IManagementHub> managementHub)
        {
            _rootPath = hostingEnvironment.WebRootPath ?? hostingEnvironment.ContentRootPath;
            _videoPath = $"{_rootPath}\\Videos";
            
            _videoHub = videoHub;
            _managementHub = managementHub;

            var categories = InitCategoryRepository(hostingEnvironment, categoryRepository);

            var clientIdentities = GetClientIdentities1();

            InitVideoService(videoService, categories, clientIdentities);
        }

        private IEnumerable<Category> InitCategoryRepository(IHostingEnvironment hostingEnvironment, IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryRepository.Init(_rootPath);
            var categories = _categoryRepository.GetAll();
            return categories;
        }

        private void InitVideoService(IVideoService videoService, IEnumerable<Category> categories, IEnumerable<ClientIdentity> clientIdentities)
        {
            _videoService = videoService;
            _videoService.Init(_videoPath, categories, clientIdentities);
            ManagementHub.AddNotifyEvent(
                (sender, args) => { _managementHub.Clients.All.getOnlineUsers(Utility.VideoUtility.GetClientCount()); }
            );
            VideoHub.AddConnectedEvent(
                (senger, args) =>
                {
                    _videoService.AddClientIdentity(
                        args.Id, args.Ip, true);
                    _managementHub.Clients.All.getOnlineUsers(Utility.VideoUtility.GetClientCount());
                });

            VideoHub.AddDisconnectedEvent((senger, args) =>
            {
                _videoService.AddClientIdentity(
                    args.Id, args.Ip, false);
                _managementHub.Clients.All.getOnlineUsers(Utility.VideoUtility.GetClientCount());
            });
        }

        private IEnumerable<ClientIdentity> GetClientIdentities1()
        {
            var clientIdentities = new List<ClientIdentity>();
            for (var i = 1; i <= 30; i++)
            {
                clientIdentities.Add(new ClientIdentity
                {
                    Id = i,
                    Ip = $"192.168.0.{i}",
                    IsActive = false
                });
            }

            return clientIdentities;
        }

        [HttpGet("SearchVideos")]
        public IActionResult SearchVideos(
            List<int> categoryIds, DateTime? startDate, DateTime? endDate)
        {
            return new JsonResult(_videoService.SearchVideos(
                    categoryIds, startDate, endDate, _videoPath)
                .Select(VideoToViewModel));
        }

        [HttpPost("SetVideo")]
        public async Task<IActionResult> SetVideo([FromBody] SetParams setParams)
        {
            var ipInfo = _videoService.GetClientIdentities()
                .FirstOrDefault(i => i.Id.ToString() == setParams.Id);

            if (ipInfo == null)
                return new JsonResult("No client.");

            var ip = ipInfo.Ip;

            _videoService.SetVideos(setParams.Codes, ip, _videoPath);

            var ips = VideoUtility.GetClientConnetionIdDic();
            if (!ips.ContainsKey(ip))
                return new JsonResult("No online client.");

            await _videoHub.Clients.Client(ips[ip])
                .PlayVideo(_videoService.GetVideoListByIp(ip)
                    .Select(VideoToViewModel));

            return new JsonResult("Ok");
        }

        [HttpGet("CleanVideo")]
        public async Task<IActionResult> CleanVideo(string id)
        {
            var ipInfo = _videoService.GetClientIdentities()
                .FirstOrDefault(i => i.Id.ToString() == id);

            if (ipInfo == null)
                return new JsonResult("No client.");

            var ip = ipInfo.Ip;

            _videoService.CleanVideo(ip);

            var ips = VideoUtility.GetClientConnetionIdDic();
            if (!ips.ContainsKey(ip))
                return new JsonResult("No online client.");

            await _videoHub.Clients.Client(ips[ip])
                .PlayVideo(_videoService.GetVideoListByIp(ip)
                    .Select(VideoToViewModel));

            return new JsonResult("Ok");
        }

        [HttpGet("GetVideoList")]
        public IActionResult GetVideoList()
        {
            var videos = _videoService.GetVideoListByIp(GetCallerIp());
            var result = videos
                .Select(VideoToViewModel);

            return new JsonResult(result);
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
                }));
        }

        protected virtual string GetCallerIp()
        {
            return Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
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