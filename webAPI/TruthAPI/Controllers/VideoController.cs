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
        protected virtual string CallerIp => Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

        private readonly IVideoService _videoService;
        private readonly IHubContext<VideoHub, IVideoHub> _videoHub;
        private readonly IHubContext<ManagementHub, IManagementHub> _managementHub;
        
        public VideoController(
            IVideoService videoService,
            IHubContext<VideoHub, IVideoHub> videoHub,
            IHubContext<ManagementHub, IManagementHub> managementHub)
        {
            _videoHub = videoHub;
            _managementHub = managementHub;
            _videoService = videoService;

            ManagementHub.AddNotifyEvent((sender, args) =>
            {
                OnlineUserChange();
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
            var videos = _videoService
                .SearchVideos(categoryIds, startDate, endDate)
                .Select(VideoToViewModel)
                .ToList();

            return videos.Any() ?
                Ok(NewCollectionViewModel<VideoViewModel>().SetContent(videos)) :
                NotFound(NewCollectionViewModel<VideoViewModel>().SetMessage("No videos.")) as IActionResult;
        }

        [HttpPost("SetVideo")]
        public async Task<IActionResult> SetVideo([FromBody] SetVideoViewModel inputParams)
        {
            var id = inputParams.Id;

            if(GetIpInfo(id) == null)
                return NotFound(NewViewModel<VideoViewModel>()
                    .SetMessage($"Id {id} is not found."));
            
            var connectionId = _videoService.SetVideos(inputParams.Codes, id, inputParams.StartTime, inputParams.EndTime);

            if(inputParams.StartTime > DateTime.Now){
                 return Ok(NewViewModel<VideoViewModel>()
                    .SetMessage($"{id} Reservation success."));
            }

            OnlineUserChange();

            if(string.IsNullOrWhiteSpace(connectionId))
                return Ok(NewViewModel<VideoViewModel>()
                    .SetMessage($"Set videos to id {id} success. client is offline."));

            await SetClientVideo(connectionId, id);

            return Ok(NewViewModel<VideoViewModel>()
                .SetMessage($"Set videos to id {id} success. client is online."));
        }

        [HttpPost("CleanVideo")]
        public async Task<IActionResult> CleanVideo([FromBody] CleanVideoViewModel inputParams)
        {
            var id = inputParams.Id;

            if(GetIpInfo(id) == null)
                return NotFound(NewViewModel<VideoViewModel>()
                    .SetMessage($"Id {id} is not found."));
            
            await SetClientVideo(_videoService.CleanVideo(id), id);

            OnlineUserChange();

            return Ok(NewViewModel<VideoViewModel>()
                .SetMessage($"Id {id} video is clean."));
        }

        [HttpGet("GetVideoList")]
        public IActionResult GetVideoList()
        {
            var videos = _videoService
                .GetVideoListById(CallerIp)
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
            var path = _videoService.GetVideoByCode(code, CallerIp);

            return !string.IsNullOrWhiteSpace(path) ?
                Redirect(path) :
                StatusCode((int) HttpStatusCode.NotModified,
                        NewViewModel<string>().SetMessage($"code {code} not set video.")) as IActionResult;
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
                NotFound(NewCollectionViewModel<VideoViewModel>().SetMessage("Not set categories.")) as IActionResult;
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
                NotFound(NewCollectionViewModel<VideoViewModel>().SetMessage("Not set clientIdentities.")) as IActionResult;
        }

        private async Task SetClientVideo(string connectionId, string id)
        {
            await _videoHub.Clients.Client(connectionId)
                .PlayVideo(_videoService.GetVideoListById(id)
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

        private void OnlineUserChange()
        {
            _managementHub.Clients.All.GetOnlineUser(_videoService.GetClientIdentities().Select(r =>
                new ClientIdentityViewModel
                {
                    Id = r.Id,
                        IsActive = r.IsActive,
                        IsOnline = r.IsOnline,
                }));
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