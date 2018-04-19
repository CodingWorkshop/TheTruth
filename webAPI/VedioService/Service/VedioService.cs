using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;
using VideoService.Interface;

namespace VideoService.Service
{
    public class VideoService : IVideoService
    {
        private IEnumerable<Video> _allVideos;
        
        private IEnumerable<Category> _categories;

        private string _rootPath;

        public void Init(string rootPath,
            IEnumerable<Category> categories,
            IEnumerable<ClientIdentity> clientIdentities)
        {
            _rootPath = rootPath;
            _categories = categories;

            foreach (var identity in clientIdentities)
            {
                VideoUtility.AddClientIdentity(identity);
            }

            InitDirectories();
            InitVideos();
        }

        public IEnumerable<Video> SearchVideos(
            IEnumerable<int> categoryIds, DateTime? beginTime,
            DateTime? endTime, string rootPath)
        {
            InitVideos();

            var query = _allVideos;

            if (categoryIds.Any())
                query = query.Where(w => categoryIds.Any(where => where == w.CategoryId));

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

        public string SetVideos(IEnumerable<string> codes,
            string ip, string rootPath)
        {
            var videos = new List<Video>();

            foreach (var code in codes)
            {
                var video = _allVideos
                    .Where(w => w.Code == code);

                if (video.Any())
                    videos.Add(video.First());
            }

            VideoUtility.AddVideo(ip, videos);
            VideoUtility.UpdateActiveStatus(ip, true);
            return VideoUtility.GetConnectionIdByIp(ip);
        }

        public string CleanVideo(string ip)
        {
            VideoUtility.AddVideo(ip, new List<Video>());
            VideoUtility.UpdateActiveStatus(ip, false);
            return VideoUtility.GetConnectionIdByIp(ip);
        }

        public void AddConnetionId(string ip, string connectionId)
        {
            VideoUtility.AddConnetionId(ip, connectionId);
        }

        public void UpdateOnlineStatus(string ip, bool isOnline)
        {
            VideoUtility.UpdateOnlineStatus(ip, true);
        }

        public void RemoveConnetionId(string ip)
        {
            VideoUtility.RemoveConnetionId(ip);
        }

        public IEnumerable<Video> GetVideoListByIp(string ip)
        {
            return VideoUtility.GetClientVideo(ip);
        }

        public string GetVideoByCode(string code, string rootPath, string ip)
        {
            return VideoUtility.GetClientVideo(ip)
                .FirstOrDefault(v => v.Code == code)?
                .Url
                .Replace(rootPath, "~/Videos");
        }

        public IEnumerable<Category> GetCategories()
        {
            return _categories;
        }

        public IEnumerable<ClientIdentity> GetClientIdentities()
        {
            return VideoUtility.GetAllClientInfo();
        }

        private void InitVideos()
        {
            _allVideos = _rootPath
                .GetChildPaths()
                .Select(PathInfoToVideoInfo);
        }

        private Video PathInfoToVideoInfo(PathInfo pathInfo)
        {
            var category = pathInfo.Hierarchy[0];
            var date = pathInfo.Hierarchy[1];

            var cate = _categories
                .FirstOrDefault(c => c.Name == category);

            return new Video
            {
                CategoryId = cate?.Id ?? 0,
                CategoryName = cate?.DisplayName,
                Category = category,
                Date = date,
                Url = pathInfo.FullName,
                Name = pathInfo.Name
            };
        }

        private void InitDirectories()
        {
            foreach (var category in _categories)
                _rootPath.InitDirectory(category.Name)
                .InitDirectory(DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}