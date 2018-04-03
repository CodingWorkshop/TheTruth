using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using Microsoft.AspNetCore.SignalR;
using TheTruth.Hubs;
using TheTruth.ViewModels;
using Utility;
using VideoService.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheTruth.Controllers
{
    [Route("api/[controller]")]
    public class VideoController : Controller
    {
        private readonly string _videoPath;
        private readonly IVideoService _service;
        private readonly IHubContext<VideoHub> _videoHub;

        public VideoController(IHostingEnvironment hostingEnvironment,
            IVideoService service, IHubContext<VideoHub> videoHub)
        {
            _videoPath = $"{hostingEnvironment.WebRootPath}\\Videos";
            _service = service;
            _videoHub = videoHub;

            var categories = new List<CategoryInfo>
            {
                new CategoryInfo {Id = 1, DisplayName = "國文", Name = "Chinese"},
                new CategoryInfo {Id = 2, DisplayName = "英文", Name = "English"},
                new CategoryInfo {Id = 3, DisplayName = "數學", Name = "Math"},
                new CategoryInfo {Id = 4, DisplayName = "物理", Name = "Physical"},
                new CategoryInfo {Id = 5, DisplayName = "化學", Name = "Chemical"},
                new CategoryInfo {Id = 6, DisplayName = "社會", Name = "Social"},
                new CategoryInfo {Id = 7, DisplayName = "歷史", Name = "History"},
                new CategoryInfo {Id = 8, DisplayName = "地理", Name = "Geography"},
            };
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

            service.Init(_videoPath, categories, clientIdentities);
        }

        [HttpGet("SearchVideos")]
        public IActionResult SearchVideos(
            List<int> categoryIds, DateTime? startDate, DateTime? endDate)
        {
            return new JsonResult(_service.SearchVideos(
                    categoryIds, startDate, endDate, _videoPath)
                .Select(VideoToViewModel));
        }

        [HttpGet("SetVideo")]
        public IActionResult SetVideo(string ip, List<string> codes)
        {
            _service.SetVideos(codes, ip, _videoPath);

            var ips = VideoUtility.GetClientConnetionIdDic();
            if (!ips.ContainsKey(ip))
                return new JsonResult("No client.");

            var connectionId = ips[ip];
            var client = _videoHub.Clients.Client(connectionId);
            client.SendAsync("playVideo", _service.GetVideoListByIp(ip));

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
                DisplayName = video.DisplayName,
                Code = video.Code
            };
        }
    }
}