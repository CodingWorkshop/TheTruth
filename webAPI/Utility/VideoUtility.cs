using System.Collections.Concurrent;
using System.Collections.Generic;
using DataAccess;

namespace Utility
{
    public static class VideoUtility
    {
        private static ConcurrentDictionary<string, string> IpConnetionIdDic = new ConcurrentDictionary<string, string>();
        private static Dictionary<string, List<Video>> IpVideoDic = new Dictionary<string, List<Video>>();

        public static ConcurrentDictionary<string, string> GetClientConnetionIdDic()
        {
            return IpConnetionIdDic;
        }

        public static void SetIpConnetionIdDic(ConcurrentDictionary<string, string> ipConnetionIdDic)
        {
            IpConnetionIdDic = ipConnetionIdDic;
        }

        public static Dictionary<string, List<Video>> GetIpVideoDic()
        {
            return IpVideoDic;
        }

        public static void SetIpVideoDic(Dictionary<string, List<Video>> ipVideoDic)
        {
            IpVideoDic = ipVideoDic;
        }
    }
}