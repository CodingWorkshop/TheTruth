using System.Collections.Generic;
using DataAccess;
using TheTruth.ViewModels;

namespace TheTruth {
    public static class Utility {
        private static Dictionary<string, string> IpConnetionIdDic = new Dictionary<string, string>();
        private static Dictionary<string, List<Video>> IpVideoDic = new Dictionary<string, List<Video>>();

        public static Dictionary<string, string> GetIpConnetionIdDic() {
            return IpConnetionIdDic;
        }
        public static void SetIpConnetionIdDic(Dictionary<string, string> ipConnetionIdDic) {
            IpConnetionIdDic = ipConnetionIdDic;
        }

        public static Dictionary<string, List<Video>> GetIpVideoDic() {
            return IpVideoDic;
        }
        public static void SetIpVideoDic(Dictionary<string, List<Video>> ipVideoDic) {
            IpVideoDic = ipVideoDic;
        }
    }
}