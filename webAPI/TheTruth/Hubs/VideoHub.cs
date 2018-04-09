using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using DataAccess;

namespace TheTruth.Hubs
{
    public class SignalRConnectionEventArgs : EventArgs
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public bool IsActive { get; set; }
    }

    public class VideoHub : Hub<IVideoHub>
    {
        private IHttpContextAccessor _accessor;

        private static event EventHandler<SignalRConnectionEventArgs>
            ConnectedEvent;

        private static event EventHandler<SignalRConnectionEventArgs>
            DisconnectedEvent;

        public VideoHub(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public static void AddConnectedEvent(
            EventHandler<SignalRConnectionEventArgs> e)
        {
            if (ConnectedEvent == null)
                ConnectedEvent = e;
        }

        public static void AddDisconnectedEvent(
            EventHandler<SignalRConnectionEventArgs> e)
        {
            if (DisconnectedEvent == null)
                DisconnectedEvent = e;
        }

        ///// <summary>
        ///// Client 端 來取Video
        ///// </summary>
        ///// <returns></returns>
        //[HubMethodName("requestVideo")]
        //public Task RequestVideo()
        //{
        //    Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} come to get Videos");
        //    var ip = GetRemoteIpAddress();
        //    Console.WriteLine(ip);
        //    var videos = Utility.VideoUtility.GetIpVideoDic()
        //        .GetValueOrDefault(ip)
        //        .Select(r => new VideoViewModel
        //        {
        //            Id = r.CategoryId,
        //            DisplayName = r.DisplayName,
        //            Name = r.Name,
        //            Code = r.Code,
        //            Date = r.Date,
        //        }).ToList();
        //
        //    return Clients.Client(Context.ConnectionId).PlayVideo(videos);
        //}

        /// <summary>
        /// 連線進來
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} Login");

            var ip = GetRemoteIpAddress();

            Utility.VideoUtility
                .GetClientConnetionIdDic()
                .TryAdd(ip, Context.ConnectionId);

            OnConnectionChanged(ConnectedEvent, ip);

            return base.OnConnectedAsync();
            //return Clients.Caller.Connected("Login Ok");
        }

        /// <summary>
        /// 離線
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} Log Out");

            var ip = GetRemoteIpAddress();

            Utility.VideoUtility
                .GetClientConnetionIdDic()
                .TryRemove(ip, out var newDic);

            OnConnectionChanged(DisconnectedEvent, ip);

            return base.OnDisconnectedAsync(exception);
            //return Clients.All.Disconnected("Bye");
        }

        private void OnConnectionChanged(
            EventHandler<SignalRConnectionEventArgs> connectionEvent,
            string ip)
        {
            Console.WriteLine();
            connectionEvent?.Invoke(
                this,
                new SignalRConnectionEventArgs
                {
                    Id = int.Parse(ip.Split('.').Last()),
                    Ip = ip,
                    IsActive = true
                });
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

    public interface IVideoHub
    {
        Task PlayVideo(IEnumerable<VideoViewModel> videos);

        //Task Connected(string msg);
        //
        //Task Disconnected(string msg);
    }
}