using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using TruthAPI.Hubs;
using TruthAPI.ViewModels;
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
        private IRepository<Reservation> _reservationRepository;

        private string _rootPath;

        public VideoController(
            IHostingEnvironment hostingEnvironment,
            IVideoService videoService,
            IRepository<Category> categoryRepository,
            IRepository<ClientIdentity> clientIdentityRepository,
            IRepository<Reservation> reservationRepository,
            IHubContext<VideoHub, IVideoHub> videoHub,
            IHubContext<ManagementHub, IManagementHub> managementHub)
        {
            _rootPath = hostingEnvironment.WebRootPath ?? hostingEnvironment.ContentRootPath;
            _videoPath = $"{_rootPath}\\Videos";

            _videoHub = videoHub;
            _managementHub = managementHub;

            var categories = InitCategoryRepository(categoryRepository);

            var clientIdentities = InitClientIdentities(clientIdentityRepository);
            var reservation = InitReservationRepository(reservationRepository);
            InitVideoService(videoService, categories, clientIdentities, reservation);
        }

        [HttpGet("SearchVideos")]
        public IActionResult SearchVideos(
            List<int> categoryIds, DateTime? startDate, DateTime? endDate)
        {
            var videos = _videoService
                .SearchVideos(categoryIds, startDate, endDate, _videoPath)
                .Select(VideoToViewModel)
                .ToList();

            return videos.Any() ?
                Ok(NewCollectionViewModel<VideoViewModel>().SetContent(videos)) :
                NotFound(NewCollectionViewModel<VideoViewModel>().SetMessage("No videos.")) as IActionResult;
        }

        [HttpPost("SetVideo")]
        public async Task<IActionResult> SetVideo([FromBody] SetVideoViewModel setVideoParams)
        {

            var ipInfo = GetIpInfo(setVideoParams.Id);

            if(ipInfo == null)
                return NotFound(NewViewModel<VideoViewModel>()
                    .SetMessage($"Id {setVideoParams.Id} is not found."));

            // TODO 預備將所有傳入IP改成Id
            //var ip = ipInfo.Ip;
            var ip = ipInfo.Id;

            var connectionId = _videoService.SetVideos(setVideoParams.Codes, ip, _videoPath, setVideoParams.StartTime, setVideoParams.EndTime);

            if(setVideoParams.StartTime > DateTime.Now){
                 return Ok(NewViewModel<VideoViewModel>()
                    .SetMessage($"{setVideoParams.Id} Reservation success."));
            }
            
            GetOnlineUser();

            if(string.IsNullOrWhiteSpace(connectionId))
                return Ok(NewViewModel<VideoViewModel>()
                    .SetMessage($"Set videos to id {setVideoParams.Id} success. client is offline."));

            await SetClientVideo(connectionId, ip);

            return Ok(NewViewModel<VideoViewModel>()
                .SetMessage($"Set videos to id {setVideoParams.Id} success. client is online."));
        }

        [HttpPost("CleanVideo")]
        public async Task<IActionResult> CleanVideo([FromBody] CleanVideoViewModel cleanVideoParams)
        {
            var ipInfo = GetIpInfo(cleanVideoParams.Id);

            if(ipInfo == null)
                return NotFound(NewViewModel<VideoViewModel>()
                    .SetMessage($"Id {cleanVideoParams.Id} is not found."));

            var ip = ipInfo.Ip;

            await SetClientVideo(_videoService.CleanVideo(ip), ip);

            GetOnlineUser();

            return Ok(NewViewModel<VideoViewModel>()
                .SetMessage($"Id {cleanVideoParams.Id} video is clean."));
        }

        [HttpGet("GetVideoList")]
        public IActionResult GetVideoList()
        {
            var videos = _videoService
                .GetVideoListByIp(GetCallerIp())
                .Select(VideoToViewModel);

            return videos.Any() ?
                Ok(NewCollectionViewModel<VideoViewModel>().SetContent(videos)) :
                NotFound(NewCollectionViewModel<VideoViewModel>()
                    .SetMessage("Not set videos on this client."))
            as IActionResult;
        }

        [HttpGet("PlayVideo")]
        public IActionResult PlayVideo(string code)
        {
            var path = _videoService.GetVideoByCode(code, _videoPath, GetCallerIp());

            return !string.IsNullOrWhiteSpace(path) ?
                Redirect(path) :
                StatusCode((int) HttpStatusCode.NotModified, NewViewModel<string>()
                    .SetMessage($"code {code} not set video."))
            as IActionResult;
        }

        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            var categories = _videoService
                .GetCategories()
                .Select(s => new CategoryViewModel
                {
                    Id = s.Id,
                        DisplayName = s.DisplayName
                });

            return categories.Any() ?
                Ok(NewCollectionViewModel<CategoryViewModel>().SetContent(categories)) :
                NotFound(NewCollectionViewModel<VideoViewModel>().SetMessage("Not set categories."))
            as IActionResult;
        }

        [HttpGet("GetClientIdentities")]
        public IActionResult GetClientIdentities()
        {
            var identities = _videoService
                .GetClientIdentities()
                .Select(s => new ClientIdentityViewModel
                {
                    Id = s.Id,
                        IsActive = s.IsActive,
                        IsOnline = s.IsOnline,
                });

            return identities.Any() ?
                Ok(NewCollectionViewModel<ClientIdentityViewModel>().SetContent(identities)) :
                NotFound(NewCollectionViewModel<VideoViewModel>().SetMessage("Not set clientIdentities."))
            as IActionResult;
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

        private void GetOnlineUser()
        {
            _managementHub.Clients.All.GetOnlineUsers(_videoService.GetClientIdentities().Select(r =>
                new ClientIdentityViewModel
                {
                    Id = r.Id,
                        IsActive = r.IsActive,
                        IsOnline = r.IsOnline,
                }));
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
        private IEnumerable<Reservation> InitReservationRepository(IRepository<Reservation> reservationRepository)
        {
            _reservationRepository = reservationRepository;
            _reservationRepository.Init(_rootPath);
            var reservation = _reservationRepository.GetAll();
            return reservation;
        }

        private void InitVideoService(IVideoService videoService, IEnumerable<Category> categories,
            IEnumerable<ClientIdentity> clientIdentities, IEnumerable<Reservation> reservation)
        {
            _videoService = videoService;
            _videoService.Init(_videoPath, categories, clientIdentities, reservation);
            ManagementHub.AddNotifyEvent((sender, args) =>
            {
                GetOnlineUser();
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

        private GenericViewModel<T> NewViewModel<T>()
        {
            return GenericViewModel<T>.New();
        }

        private GenericViewModel<IEnumerable<T>> NewCollectionViewModel<T>()
        {
            return GenericViewModel<IEnumerable<T>>.New();
        }
    }
}