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

        string SetVideos(IEnumerable<string> codes, string ip, string rootPath,
         DateTime startTime, DateTime endTime);

        IEnumerable<Category> GetCategories();

        IEnumerable<ClientIdentity> GetClientIdentities();

        void Init(string rootPath, IEnumerable<Category> categories, IEnumerable<ClientIdentity> clientIdentities,IEnumerable<Reservation> reservation);

        string CleanVideo(string ip);

        void AddConnetionId(string ip, string connectionId);

        void UpdateOnlineStatus(string ip, bool isOnline);

        void RemoveConnetionId(string ip);
    }
}