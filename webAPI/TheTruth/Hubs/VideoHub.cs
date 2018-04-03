using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TheTruth.ViewModels;

namespace TheTruth.Hubs
{
    public class VideoHub : Hub
    {
        private IHttpContextAccessor _accessor;

        public VideoHub(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;
        }

        /// <summary>
        /// Client 端 來取Video
        /// </summary>
        /// <returns></returns>
        [HubMethodName("requestVideo")]
        public Task RequestVideo()
        {
            var ip = GetRemoteIpAddress();
            Console.WriteLine($"{ip} {Context.ConnectionId} come to get Videos");
            List<VideoViewModel> videos = GetClientVideos(ip);
            return Clients.Client(Context.ConnectionId).SendAsync("playVideo", videos);
        }

        /// <summary>
        /// 連線進來
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var ip = GetRemoteIpAddress();
            Console.WriteLine($"{ip} {Context.ConnectionId} Login");
            Utility.VideoUtility.GetClientConnetionIdDic().AddOrUpdate(ip, Context.ConnectionId,(key, oldValue) => Context.ConnectionId);
            //Utility.VideoUtility.Notify();
            var videos = GetClientVideos(ip);
            Clients.Caller.SendAsync("loginCenter", "Login Ok");
            return Clients.Caller.SendAsync("playVideo", videos);
        }

        /// <summary>
        /// 離線
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var ip = GetRemoteIpAddress();
            Utility.VideoUtility.GetClientConnetionIdDic().TryRemove(ip, out var newDic);
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Get IP
        /// </summary>
        /// <returns></returns>
        private string GetRemoteIpAddress()
        {
            return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        private static List<VideoViewModel> GetClientVideos(string ip)
        {
            return Utility.VideoUtility.GetIpVideo(ip) ?
                .Select(r => new VideoViewModel
                {
                    Id = r.CategoryId,
                        DisplayName = r.DisplayName,
                        Name = r.Name,
                        Code = r.Code,
                        Date = r.Date,
                }).ToList();
        }

    }
}