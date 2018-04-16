using System;
using System.Collections.Generic;
using DataAccess;

namespace VideoService.Interface
{
    public interface IVideoService
    {
        IEnumerable<Video> GetVideoListByIp(string ip);

        string GetVideoByCode(string code, string rootPath, string ip);

        IEnumerable<Video> SearchVideos(IEnumerable<int> categoryIds, DateTime? beginTime,
            DateTime? endTime, string rootPath);

        void SetVideos(IEnumerable<string> codes, string ip, string rootPath);

        IEnumerable<Category> GetCategories();

        IEnumerable<ClientIdentity> GetClientIdentities();

        void Init(string rootPath, IEnumerable<Category> categories, IEnumerable<ClientIdentity> clientIdentities);

        void AddClientIdentity(int id, string ip, bool isActive);
    }
}