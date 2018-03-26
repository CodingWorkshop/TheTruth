using System;
using System.Collections.Generic;
using DataAccess;

namespace VideoService.Interface
{
    public interface IVideoService
    {
        List<Video> GetVideoListByIp(string ip);

        List<Video> GetVideoList(string rootPath);

        string GetVideo(string code, string rootPath, string ip);

        List<Video> GetAllVideo(string category, DateTime? beginTime,
            DateTime? endTime, string rootPath);

        void SetVideos(List<string> codes, string ip, string rootPath);
    }
}