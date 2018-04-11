using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using TruthAPI.ViewModels;

namespace TruthAPI.Hubs
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
            if(ConnectedEvent == null)
                ConnectedEvent = e;
        }

        public static void AddDisconnectedEvent(
            EventHandler<SignalRConnectionEventArgs> e)
        {
            if(DisconnectedEvent == null)
                DisconnectedEvent = e;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} Login");

            var ip = GetRemoteIpAddress();

            Utility.VideoUtility
                .GetClientConnetionIdDic()
                .TryAdd(ip, Context.ConnectionId);
            OnConnectionChanged(ConnectedEvent, ip);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} Log Out");

            var ip = GetRemoteIpAddress();

            Utility.VideoUtility
                .GetClientConnetionIdDic()
                .TryRemove(ip, out var newDic);

            OnConnectionChanged(DisconnectedEvent, ip);
            return base.OnDisconnectedAsync(exception);
        }

        private void OnConnectionChanged(
            EventHandler<SignalRConnectionEventArgs> connectionEvent,
            string ip)
        {
            Utility.VideoUtility.DoNotifyEvent();
            connectionEvent?.Invoke(
                this,
                new SignalRConnectionEventArgs
                {
                    Id = int.Parse(ip.Split('.').Last()),
                        Ip = ip,
                        IsActive = true
                });
        }

        private string GetRemoteIpAddress()
        {
            return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
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