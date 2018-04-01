using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using TheTruth.ViewModels;
using VideoService.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheTruth.Controllers
{
    [Route("api/[controller]")]
    public class VideoController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _videoPath;
        private readonly IVideoService _service;
        private readonly List<CategoryInfo> _categories;

        public VideoController(IHostingEnvironment hostingEnvironment, IVideoService service)
        {
            _hostingEnvironment = hostingEnvironment;
            _videoPath = $"{_hostingEnvironment.WebRootPath}\\Videos";
            _service = service;
            _categories = new List<CategoryInfo>
            {
                new CategoryInfo {Id = 1, DisplayName = "國文", Folder = "Chinese"},
                new CategoryInfo {Id = 2, DisplayName = "英文", Folder = "English"},
                new CategoryInfo {Id = 3, DisplayName = "數學", Folder = "Math"},
                new CategoryInfo {Id = 4, DisplayName = "物理", Folder = "Physical"},
                new CategoryInfo {Id = 5, DisplayName = "化學", Folder = "Chemical"},
                new CategoryInfo {Id = 6, DisplayName = "社會", Folder = "Social"},
                new CategoryInfo {Id = 7, DisplayName = "歷史", Folder = "History"},
                new CategoryInfo {Id = 8, DisplayName = "地理", Folder = "Geography"},
            };

            service.InitDirectories(_videoPath, _categories);
        }

        [HttpGet("GetVideoList")]
        public IActionResult GetVideoList()
        {
            var videos = _service.GetVideoListByIp(GetCallerIp());
            var result = videos
                .Select(s => new VideoViewModel
                {
                    Name = s.Name,
                    Date = s.Date,
                    Category = s.Category,
                    Code = s.Code
                });

            return new JsonResult(result);
        }

        [HttpGet("PlayVideo")]
        public IActionResult PlayVideo(string code)
        {
            return Redirect(_service.GetVideo(code, _videoPath, GetCallerIp()));
        }

        [HttpGet("SearchVideos")]
        public IActionResult SearchVideos(
            List<string> categories, DateTime? beginTime, DateTime? endTime)
        {
            return new JsonResult(_service.SearchVideos(
                categories, beginTime, endTime, _videoPath)
                .Select(s => new VideoViewModel
                {
                    Name = s.Name,
                    Date = s.Date,
                    Category = s.Category,
                    Code = s.Code
                }));
        }

        [HttpGet("SetVideo")]
        public IActionResult SetVideo(string ip, List<string> codes)
        {
            _service.SetVideos(codes, ip, _videoPath);
            return new JsonResult("Ok");
        }

        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            return new JsonResult(_categories
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
    }
}