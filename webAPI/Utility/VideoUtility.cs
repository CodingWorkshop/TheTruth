using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;

namespace Utility
{
    public static class VideoUtility
    {
        private static ConcurrentDictionary<string, string> _connetionIdDic = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, IEnumerable<Video>> _videoDic = new ConcurrentDictionary<string, IEnumerable<Video>>();
        private static ConcurrentDictionary<string, ClientIdentity> _clientIdentityDic = new ConcurrentDictionary<string, ClientIdentity>();
        private static event EventHandler NotifyEvent;

        public static IEnumerable<Video> GetClientVideo(string ip)
        {
            return _videoDic.TryGetValue(ip, out var videos) ? videos : new List<Video>();
        }

        public static IEnumerable<ClientIdentity> GetAllClientInfo()
        {
            return _clientIdentityDic.Values;
        }

        public static string GetConnectionIdByIp(string ip)
        {
            return _connetionIdDic.TryGetValue(ip, out var connectionId) ? connectionId : string.Empty;
        }
     

        public static void AddConnetionId(string ip, string connectionId)
        {
            _connetionIdDic.TryAdd(ip, connectionId);
        }
        public static void AddVideo(string ip, IEnumerable<Video> videos)
        {
            _videoDic.TryAdd(ip, videos);
        }

        public static void AddClientIdentity(ClientIdentity clientIdentity)
        {
            _clientIdentityDic.TryAdd(clientIdentity.Ip, clientIdentity);
        }

        public static void RemoveVideo(string ip)
        {
            _videoDic.TryRemove(ip, out var newdic);
        }

         public static void RemoveClientIdentity(string ip)
        {
            _clientIdentityDic.TryRemove(ip, out var newdic);
        }

         public static void RemoveConnetionId(string ip)
        {
            _connetionIdDic.TryRemove(ip, out var newdic);
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

        public static void UpdateOnlineStatus(string ip, bool isOnline)
        {
            var identity = GetClientIdentityByIp(ip);

            if (identity == null)
                return;

            identity.IsOnline = isOnline;
        }

        public static void UpdateActiveStatus(string ip, bool isActive)
        {
            var identity = GetClientIdentityByIp(ip);

            if (identity == null)
                return;

            identity.IsActive = isActive;
        }

        private static ClientIdentity GetClientIdentityByIp(string ip)
        {
            _clientIdentityDic.TryGetValue(ip, out var identity);
            return identity;
        }
    }
}