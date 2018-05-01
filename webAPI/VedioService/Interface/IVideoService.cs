using System;
using System.Collections.Generic;
using DataAccess;

namespace VideoService.Interface
{
    public interface IVideoService
    {
        IEnumerable<Video> GetVideoListById(string ip);

        string GetVideoByCode(string code, string ip);

        IEnumerable<Video> SearchVideos(IEnumerable<int> categoryIds, DateTime? beginTime, DateTime? endTime);

        string SetVideos(IEnumerable<string> codes, string id, DateTime startTime, DateTime endTime);

        IEnumerable<Category> GetCategories();

        IEnumerable<ClientIdentity> GetClientIdentities();

        string CleanVideo(string ip);

        void AddConnetionId(string ip, string connectionId);

        void UpdateOnlineStatus(string ip, bool isOnline);

        void RemoveConnetionId(string ip);
    }
}