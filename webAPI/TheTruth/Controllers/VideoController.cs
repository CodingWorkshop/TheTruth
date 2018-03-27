using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public VideoController(IHostingEnvironment hostingEnvironment, IVideoService service)
        {
            _hostingEnvironment = hostingEnvironment;
            _videoPath = $"{_hostingEnvironment.WebRootPath}\\VideoRootPath";
            _service = service;
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

        [HttpGet("GetAllVideo")]
        public IActionResult GetAllVideo(
            string category, DateTime? beginTime, DateTime? endTime)
        {
            return new JsonResult(_service.GetAllVideo(
                category, beginTime, endTime, _videoPath)
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
            return new JsonResult(_service.GetCategories(_videoPath));
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