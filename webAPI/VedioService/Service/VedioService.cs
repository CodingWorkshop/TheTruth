using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using Repository.Repository;
using Utility;
using VideoService.Interface;

namespace VideoService.Service
{
    public class VideoService : IVideoService
    {
        private readonly Dictionary<string, List<Video>> _videos =
            new Dictionary<string, List<Video>>();

        public List<Video> GetVideoListByIp(string ip)
        {
            return _videos[ip];
        }

        public string GetVideo(string code, string rootPath, string ip)
        {
            if (_videos[ip].Any(v => v.Code == code))
                return _videos[ip]
                    .First(v => v.Code == code)
                    .Url
                    .Replace(rootPath, "~/VideoRootPath");

            return string.Empty;
        }

        public List<Video> GetAllVideo(string category, DateTime? beginTime,
            DateTime? endTime, string rootPath)
        {
            var query = new GenericFileRepository(rootPath).GetAll();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(w => category.Contains(w.Category));

            if (beginTime != null && beginTime != DateTime.MinValue)
                query = query.Where(w => w.DateTime >= beginTime);
            else
                query = query.Where(w => w.DateTime >= DateTime.Today.AddMonths(-1));

            if (endTime != null && endTime != DateTime.MinValue)
                query = query.Where(w => w.DateTime <= endTime);
            else
                query = query.Where(w => w.DateTime <= DateTime.Now);

            return query.ToList();
        }

        public void SetVideos(List<string> codes, string ip, string rootPath)
        {
            var allVideo = new GenericFileRepository(rootPath).GetAll();

            var videos = new List<Video>();

            foreach (var code in codes)
            {
                var param = code.Split('_').Select(s => s).ToList();
                var video = allVideo.Where(w => w.Category == param[0] && w.Date == param[1]);
                if (video.Any())
                    videos.Add(video.First());
            }
            if (_videos.ContainsKey(ip))
                _videos[ip] = videos;
            else
                _videos.Add(ip, videos);

            VideoUtility.SetIpVideoDic(_videos);
        }

        public List<string> GetCategories(string rootPath)
        {
            var query = new GenericFileRepository(rootPath).GetAll();

            return query
                .Select(s => s.Category)
                .Distinct()
                .ToList();
        }

        public List<ClientIdentity> GetClientIdentities()
        {
            var clients = new List<ClientIdentity>();

            for (var i = 1; i <= 30; i++)
            {
                clients.Add(new ClientIdentity
                {
                    Id = i,
                    Ip = $"192.168.0.{i}",
                    IsActive = false,
                });
            }

            return clients
                .Where(w => _videos.ContainsKey(w.Ip))
                .ToList();
        }
    }
}