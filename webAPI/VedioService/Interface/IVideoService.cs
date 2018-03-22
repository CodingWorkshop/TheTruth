using System;
using System.Collections.Generic;
using DataAccess;

namespace BLL.VideoService.Interface
{
    public interface IVideoService
    {
        List<DataAccess.Video> GetVideoListByIp(string ip);

        List<DataAccess.Video> GetVideoList(string rootPath);

        string GetVideo(string code, string rootPath, string ip);

        List<DataAccess.Video> GetAllVideo(string category, DateTime? beginTime,
            DateTime? endTime, string rootPath);

        void SetVideos(List<string> codes, string ip, string rootPath);
    }
}