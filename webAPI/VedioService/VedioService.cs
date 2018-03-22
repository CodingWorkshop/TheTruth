using DataAccess;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VedioService
{
    public class VedioService
    {
        private static Dictionary<string, List<Video>> _videos = new Dictionary<string, List<Video>>();

        public List<Video> GetVideoListByIp(string ip)
        {
            return _videos[ip];
        }

        public List<Video> GetVideoList(string rootPath)
        {
            return new GenericFileRepository(rootPath)
                .GetAll().ToList();
        }

        public string GetVideo(string code, string rootPath, string ip)
        {
            if (_videos[ip].Any(v => v.Code == code))
                return _videos[ip]
                    .First(v => v.Code == code)
                    .Url
                    .Replace(rootPath, "~/VideoRootPath");

            return string.Empty;

            //var param = code.Split('_').Select(s => s).ToList();
            //return new GenericFileRepository(rootPath)
            //    .GetAll()
            //    .Where(w => w.Category == param[0] && w.Date == param[1])
            //    .Select(s => s.Url)
            //    .First()
            //    .Replace(rootPath, "~/VideoRootPath");
        }

        public List<Video> GetAllVideo(string category, DateTime? beginTime,
            DateTime? endTime, string rootPath)
        {
            var query = new GenericFileRepository(rootPath)
                .GetAll();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(w => category.Contains(w.Category));

            if (beginTime != null && beginTime != DateTime.MinValue)
                query = query.Where(w => w.DateTime >= beginTime);
            else
                query = query.Where(w => w.DateTime >= DateTime.Today.AddMonths(-1));

            Console.WriteLine(endTime);
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
        }
    }
}