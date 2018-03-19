using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Security.Cryptography;
using Repository.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheTruth.Controllers
{
    [Route("api/[controller]")]
    public class VideoController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _videoPath;

        public VideoController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _videoPath = $"{_hostingEnvironment.WebRootPath}\\VideoRootPath";
        }

        // GET: api/video
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/video/demo
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "medias", $"{name}.mp4");
            var memoryStream = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "video/mp4");
        }

        // GET api/video/r/demo
        [HttpGet("r/{name}")]
        public IActionResult GetRedirect(string name)
        {
            return Redirect($"~/medias/{name}.mp4");
        }

        [HttpGet("GetVideoList")]
        public IActionResult GetVideoList()
        {
            var result = new GenericFileRepository(_videoPath)
                .GetAll().Select(s => new { s.Category, s.Date, s.Code });

            return new JsonResult(result);
        }

        [HttpGet("PlayVideo")]
        public IActionResult PlayVideo(string code)
        {
            var param = code.Split("_").Select(s => s).ToList();

            var result = new GenericFileRepository(_videoPath)
                .GetAll()
                .Where(w => w.Category == param[0] && w.Date == param[1])
                .Select(s => s.Url)
                .First()
                .Replace(_videoPath, "~/VideoRootPath");

            return Redirect(result);
        }
    }
}