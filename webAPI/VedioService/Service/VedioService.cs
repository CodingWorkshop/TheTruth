﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using Utility;
using VideoService.Interface;

namespace VideoService.Service
{
    public class VideoService : IVideoService
    {
        private IEnumerable<Video> _allVideos;

        private readonly Dictionary<string, IEnumerable<Video>> _userVideos =
            new Dictionary<string, IEnumerable<Video>>();

        private IEnumerable<CategoryInfo> _categories;

        private IEnumerable<ClientIdentity> _clientIdentities;

        private string _rootPath;

        public void Init(string rootPath,
            IEnumerable<CategoryInfo> categories,
            IEnumerable<ClientIdentity> clientIdentities)
        {
            _rootPath = rootPath;
            _categories = categories;
            _clientIdentities = clientIdentities;
            InitDirectories();
            InitVideos();
        }

        public void AddClientIdentity(int id, string ip, bool isActive)
        {
            if (_clientIdentities.All(i => i.Id != id))
            {
                _clientIdentities = _clientIdentities
                    .Concat(new[]
                    {
                        new ClientIdentity
                        {
                            Id = id,
                            Ip = ip,
                            IsActive = isActive
                        }
                    });
            }
            else
            {
                var client = _clientIdentities.First(i => i.Id == id);
                client.Ip = ip;
                client.IsActive = true;
            }
        }

        public IEnumerable<Video> SearchVideos(
            IEnumerable<int> categoryIds, DateTime? beginTime,
            DateTime? endTime, string rootPath)
        {
            InitVideos();

            var query = _allVideos;

            if(categoryIds.Any())
                query = query.Where(w => categoryIds.Any(where => where == w.CategoryId));

            if(beginTime != null && beginTime != DateTime.MinValue)
                query = query.Where(w => w.DateTime >= beginTime);
            else
                query = query.Where(w => w.DateTime >= DateTime.Today.AddMonths(-1));

            if(endTime != null && endTime != DateTime.MinValue)
                query = query.Where(w => w.DateTime <= endTime);
            else
                query = query.Where(w => w.DateTime <= DateTime.Now);

            return query.ToList();
        }

        public void SetVideos(IEnumerable<string> codes,
            string ip, string rootPath)
        {
            var videos = new List<Video>();

            foreach(var code in codes)
            {
                var video = _allVideos
                    .Where(w => w.Code == code);

                if(video.Any())
                    videos.Add(video.First());
            }
            if(_userVideos.ContainsKey(ip))
                _userVideos[ip] = videos;
            else
                _userVideos.Add(ip, videos);

            VideoUtility.SetAllVideoDic(_userVideos);
        }

        public IEnumerable<Video> GetVideoListByIp(string ip)
        {
            return _userVideos.ContainsKey(ip) ?
                _userVideos[ip] :
                new List<Video>();
        }

        public string GetVideoByCode(string code, string rootPath, string ip)
        {
            if(_userVideos[ip].Any(v => v.Code == code))
                return _userVideos[ip]
                    .First(v => v.Code == code)
                    .Url
                    .Replace(rootPath, "~/Videos");

            return string.Empty;
        }

        public IEnumerable<CategoryInfo> GetCategories()
        {
            return _categories;
        }

        public IEnumerable<ClientIdentity> GetClientIdentities()
        {
            return _clientIdentities;
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
                    DisplayName = cate?.DisplayName,
                    Category = category,
                    Date = date,
                    Url = pathInfo.FullName,
                    Name = pathInfo.Name
            };
        }

        private void InitDirectories()
        {
            foreach(var category in _categories)
                _rootPath.InitDirectory(category.Name)
                .InitDirectory(DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}