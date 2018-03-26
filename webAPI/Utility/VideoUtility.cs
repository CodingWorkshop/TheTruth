using DataAccess;
using System.Collections.Generic;

namespace Utility
{
    public static class VideoUtility
    {
        private static Dictionary<string, string> IpConnetionIdDic = new Dictionary<string, string>();
        private static Dictionary<string, List<Video>> IpVideoDic = new Dictionary<string, List<Video>>();

        public static Dictionary<string, string> GetIpConnetionIdDic()
        {
            return IpConnetionIdDic;
        }

        public static void SetIpConnetionIdDic(Dictionary<string, string> ipConnetionIdDic)
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