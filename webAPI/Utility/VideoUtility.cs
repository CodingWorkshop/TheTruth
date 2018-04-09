using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;

namespace Utility
{
    public static class VideoUtility
    {
        private static ConcurrentDictionary<string, string> IpConnetionIdDic = new ConcurrentDictionary<string, string>();
        private static Dictionary<string, IEnumerable<Video>> IpVideoDic = new Dictionary<string, IEnumerable<Video>>();
        public static event EventHandler OnlineUserEvent;
        public static void Notify()
        {
            OnlineUserEvent(null, EventArgs.Empty);
        }
        public static ConcurrentDictionary<string, string> GetClientConnetionIdDic()
        {
            return IpConnetionIdDic;
        }

        public static void SetIpConnetionIdDic(ConcurrentDictionary<string, string> ipConnetionIdDic)
        {
            IpConnetionIdDic = ipConnetionIdDic;
        }

        public static Dictionary<string, IEnumerable<Video>> GetAllVideoDic()
        {
            return IpVideoDic;
        }
        public static IEnumerable<Video> GetIpVideo(string ip)
        {
            IpVideoDic.TryGetValue(ip, out var videos);
            return videos;
        }

        public static void SetAllVideoDic(Dictionary<string, IEnumerable<Video>> ipVideoDic)
        {
            IpVideoDic = ipVideoDic;
        }
        public static void SetIpVideo(string ip, IEnumerable<Video> videos)
        {
            IpVideoDic.TryGetValue(ip, out var thisVideos);
            if(thisVideos == null)
                IpVideoDic.Add(ip, videos);
            else
                IpVideoDic["ip"] = videos;

        }
    }
}