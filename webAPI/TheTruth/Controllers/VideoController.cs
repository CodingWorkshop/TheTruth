using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Repository.Repository;
using TheTruth.ViewModels;
using VedioService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheTruth.Controllers
{
    [Route("api/[controller]")]
    public class VideoController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _videoPath;
        private readonly VedioService.VedioService _service;

        public VideoController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _videoPath = $"{_hostingEnvironment.WebRootPath}\\VideoRootPath";
            _service = new VedioService.VedioService();
        }

        [HttpGet("GetVideoList")]
        public IActionResult GetVideoList()
        {
            return new JsonResult(_service.GetVideoList(_videoPath)
                .Select(s => new VideoViewModel
                {
                    Name = s.Name,
                    Date = s.Date,
                    Category = s.Category,
                    Code = s.Code
                }));
        }

        [HttpGet("GetVideo")]
        public IActionResult PlayVideo(string code)
        {
            return Redirect(_service.GetVideo(code, _videoPath));
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
    }
}