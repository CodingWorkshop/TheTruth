using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheTruth.Controllers;
using Xunit;
using NSubstitute;
using VideoService.Interface;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Common;
using TheTruth.ViewModels;

namespace BLL.VideoService.Test
{
    public class VideoControllerTests
    {
        [Fact]
        public void GetVideoList_GetVideos()
        {
            var ip = "localhost";

            var hostEnvironment = Substitute.For<IHostingEnvironment>();
            hostEnvironment.WebRootPath
                .Returns(ip);

            var videos = new List<Video>
            {
                new Video
                {
                    Category = "Ch",
                    Date = "20180301",
                    Url = "",
                    Name = "Ch_20180301_1.mp4"
                },
                new Video
                {
                    Category = "Ch",
                    Date = "20180301",
                    Url = "",
                    Name = "Ch_20180301_2.mp4"
                }
            };

            var videoService = Substitute.For<IVideoService>();
            videoService.GetVideoListByIp(ip)
                .Returns(videos);

            var expected = videos.Select(s => new VideoViewModel
            {
                Name = s.Name,
                DisplayName = s.Category,
                Date = s.Date,
                Code = s.Code
            });

            var videoController = new MockVideoController(hostEnvironment, videoService, ip);
            var actual = ((JsonResult)videoController.GetVideoList()).Value;

            actual.Should().IsSameOrEqualTo(expected);
        }

        public class MockVideoController : VideoController
        {
            private readonly string _ip;

            public MockVideoController(IHostingEnvironment hostingEnvironment, IVideoService service, string ip)
                : base(hostingEnvironment, service, null, null)
            {
                _ip = ip;
            }

            protected override string GetCallerIp()
            {
                return _ip;
            }
        }
    }
}