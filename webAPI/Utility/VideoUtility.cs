using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;

namespace Utility
{
    public static class VideoUtility
    {
        private static ConcurrentDictionary<string, string> ConnetionIdDic = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, IEnumerable<Video>> VideoDic = new ConcurrentDictionary<string, IEnumerable<Video>>();
        private static ConcurrentDictionary<string, ClientIdentity> ClientIdentityDic = new ConcurrentDictionary<string, ClientIdentity>();
        public static event EventHandler NotifyEvent;

        public static IEnumerable<ClientIdentity> GetClientConnetionIdDic()
        {
            return ClientIdentityDic.Values;
        }

        public static IEnumerable<ClientIdentity> GetClientInfo()
        {
            throw new NotImplementedException();
        }

        public static string GetConnectionIdByIp(string ip)
        {
            return ConnetionIdDic.TryGetValue(ip, out var connectionId) ? connectionId : null;
        }
     

        public static void AddConnetionId(string ip, string connectionId)
        {
            ConnetionIdDic.TryAdd(ip, connectionId);
        }
        public static void AddVideo(string ip, IEnumerable<Video> videos)
        {
            VideoDic.TryAdd(ip, videos);
        }

        public static void AddClientIdentity(string ip, ClientIdentity clientIdentity)
        {
            ClientIdentityDic.TryAdd(ip, clientIdentity);
        }

        public static void RemoveVideo(string ip)
        {
            VideoDic.TryRemove(ip, out var newdic);
        }

         public static void RemoveClientIdentity(string ip)
        {
            ClientIdentityDic.TryRemove(ip, out var newdic);
        }

         public static void RemoveConnetionId(string ip)
        {
            ConnetionIdDic.TryRemove(ip, out var newdic);
        }

        public static void SetNotifyEvent(EventHandler e)
        {
            if (NotifyEvent == null)
            {
                NotifyEvent = e;
            }
        }

        public static void DoNotifyEvent()
        {
            NotifyEvent?.Invoke(null, EventArgs.Empty);
        }
    }
}