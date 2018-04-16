using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
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
        private readonly IVideoService _service;
        private readonly IHubContext<VideoHub, IVideoHub> _videoHub;
        private readonly IHubContext<ManagementHub, IManagementHub> _managementHub;

        public VideoController(
            IHostingEnvironment hostingEnvironment,
            IVideoService service,
            IHubContext<VideoHub, IVideoHub> videoHub,
            IHubContext<ManagementHub, IManagementHub> managementHub)
        {
            _videoPath = $"{hostingEnvironment.WebRootPath}\\Videos";
            _service = service;
            _videoHub = videoHub;
            _managementHub = managementHub;
            var categories = CategoryInfos();
            var clientIdentities = GetClientIdentities1();

            _service.Init(_videoPath, categories, clientIdentities);
            ManagementHub.AddNotifyEvent(
               (sender, args) =>
                {
                    _managementHub.Clients.All.getOnlineUsers(Utility.VideoUtility.GetClientCount());
                }
            );
            VideoHub.AddConnectedEvent(
               (senger, args) =>
                {
                    _service.AddClientIdentity(
                        args.Id, args.Ip, true);
                    _managementHub.Clients.All.getOnlineUsers(Utility.VideoUtility.GetClientCount());
                });

            VideoHub.AddDisconnectedEvent((senger, args) =>
            {
                _service.AddClientIdentity(
                    args.Id, args.Ip, false);
                _managementHub.Clients.All.getOnlineUsers(Utility.VideoUtility.GetClientCount());
            });
        }

        private IEnumerable<ClientIdentity> GetClientIdentities1()
        {
            var clientIdentities = new List<ClientIdentity>();
            for(var i = 1; i <= 30; i++)
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

        private IEnumerable<CategoryInfo> CategoryInfos()
        {
            var categories = new List<CategoryInfo>
            {
                new CategoryInfo { Id = 1, DisplayName = "國文", Name = "Chinese" },
                new CategoryInfo { Id = 2, DisplayName = "英文", Name = "English" },
                new CategoryInfo { Id = 3, DisplayName = "數學", Name = "Math" },
                new CategoryInfo { Id = 4, DisplayName = "物理", Name = "Physical" },
                new CategoryInfo { Id = 5, DisplayName = "化學", Name = "Chemical" },
                new CategoryInfo { Id = 6, DisplayName = "社會", Name = "Social" },
                new CategoryInfo { Id = 7, DisplayName = "歷史", Name = "History" },
                new CategoryInfo { Id = 8, DisplayName = "地理", Name = "Geography" },
            };
            return categories;
        }

        [HttpGet("SearchVideos")]
        public IActionResult SearchVideos(
            List<int> categoryIds, DateTime? startDate, DateTime? endDate)
        {
            return new JsonResult(_service.SearchVideos(
                    categoryIds, startDate, endDate, _videoPath)
                .Select(VideoToViewModel));
        }

        [HttpPost("SetVideo")]
        public async Task<IActionResult> SetVideo([FromBody] SetParams setParams)
        {
            var ipInfo = _service.GetClientIdentities()
                .FirstOrDefault(i => i.Id.ToString() == setParams.Id);

            if(ipInfo == null)
                return new JsonResult("No client.");

            var ip = ipInfo.Ip;

            _service.SetVideos(setParams.Codes, ip, _videoPath);

            var ips = VideoUtility.GetClientConnetionIdDic();
            if(!ips.ContainsKey(ip))
                return new JsonResult("No online client.");

            await _videoHub.Clients.Client(ips[ip])
                .PlayVideo(_service.GetVideoListByIp(ip)
                    .Select(VideoToViewModel));

            return new JsonResult("Ok");
        }

        [HttpGet("GetVideoList")]
        public IActionResult GetVideoList()
        {
            var videos = _service.GetVideoListByIp(GetCallerIp());
            var result = videos
                .Select(VideoToViewModel);

            return new JsonResult(result);
        }

        [HttpGet("PlayVideo")]
        public IActionResult PlayVideo(string code)
        {
            return Redirect(_service.GetVideoByCode(code, _videoPath, GetCallerIp()));
        }

        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            return new JsonResult(_service.GetCategories()
                .Select(s => new CategoryViewModel
                {
                    Id = s.Id,
                        DisplayName = s.DisplayName
                }));
        }

        [HttpGet("GetClientIdentities")]
        public IActionResult GetClientIdentities()
        {
            return new JsonResult(_service
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