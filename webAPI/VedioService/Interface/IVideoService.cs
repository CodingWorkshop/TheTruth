using System;
using System.Collections.Generic;
using DataAccess;

namespace VideoService.Interface
{
    public interface IVideoService
    {
        List<Video> GetVideoListByIp(string ip);

        string GetVideo(string code, string rootPath, string ip);

        List<Video> GetAllVideo(string category, DateTime? beginTime,
            DateTime? endTime, string rootPath);

        void SetVideos(List<string> codes, string ip, string rootPath);

        List<string> GetCategories(string rootPath);

        List<ClientIdentity> GetClientIdentities();
    }
}