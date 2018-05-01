using DataAccess;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Utility
{
    public static class VideoUtility
    {
        private static ConcurrentDictionary<string, string> _connetionIdDic = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, IEnumerable<Video>> _videoDic = new ConcurrentDictionary<string, IEnumerable<Video>>();
        private static ConcurrentDictionary<string, ClientIdentity> _clientIdentityDic = new ConcurrentDictionary<string, ClientIdentity>();

        public static IEnumerable<Video> GetClientVideo(string id)
        {
            return _videoDic.TryGetValue(id, out var videos) ? videos : new List<Video>();
        }

        public static IEnumerable<ClientIdentity> GetAllClientInfo()
        {
            return _clientIdentityDic.Values;
        }

        public static string GetConnectionIdByIp(string id)
        {
            return _connetionIdDic.TryGetValue(id, out var connectionId) ? connectionId : string.Empty;
        }

        public static void AddConnetionId(string id, string connectionId)
        {
            _connetionIdDic.TryAdd(id, connectionId);
        }

        public static void AddVideo(string id, IEnumerable<Video> videos)
        {
            _videoDic.TryAdd(id, videos);
        }

        public static void AddClientIdentity(ClientIdentity clientIdentity)
        {
            _clientIdentityDic.TryAdd(clientIdentity.Ip, clientIdentity);
        }

        public static void RemoveVideo(string id)
        {
            _videoDic.TryRemove(id, out var newdic);
        }

        public static void RemoveClientIdentity(string id)
        {
            _clientIdentityDic.TryRemove(id, out var newdic);
        }

        public static void RemoveConnetionId(string id)
        {
            _connetionIdDic.TryRemove(id, out var newdic);
        }

        public static void UpdateVideo(string id, IEnumerable<Video> videos)
        {
            _videoDic.AddOrUpdate(id, videos, (s, enumerable) => videos);
        }

        public static void UpdateOnlineStatus(string id, bool isOnline)
        {
            var identity = GetClientIdentityById(id);

            if (identity == null)
                return;

            Console.WriteLine($"UpdateOnlineStatus : {id}, {isOnline}");
            identity.IsOnline = isOnline;
        }

        public static void UpdateActiveStatus(string id, bool isActive)
        {
            var identity = GetClientIdentityById(id);

            if (identity == null)
                return;

            identity.IsActive = isActive;
        }

        private static ClientIdentity GetClientIdentityById(string id)
        {
            _clientIdentityDic.TryGetValue(id, out var identity);
            return identity;
        }
    }
}