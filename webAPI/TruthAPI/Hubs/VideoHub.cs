using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using TruthAPI.ViewModels;
using VideoService.Interface;

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
        private IVideoService _videoService;

        private static event EventHandler<SignalRConnectionEventArgs>
        ConnectedEvent;

        private static event EventHandler<SignalRConnectionEventArgs>
        DisconnectedEvent;

        public VideoHub(IHttpContextAccessor accessor, IVideoService videoService)
        {
            _accessor = accessor;
            _videoService = videoService;
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
            var ip = GetRemoteIpAddress();

            _videoService.AddConnetionId(ip, Context.ConnectionId);
            _videoService.UpdateOnlineStatus(ip, true);

            OnConnectionChanged(ConnectedEvent, ip);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var ip = GetRemoteIpAddress();

            Console.WriteLine($"ip : {ip}");
            _videoService.RemoveConnetionId(ip);
            _videoService.UpdateOnlineStatus(ip, false);

            OnConnectionChanged(DisconnectedEvent, ip);

            return base.OnDisconnectedAsync(exception);
        }

        private void OnConnectionChanged(
            EventHandler<SignalRConnectionEventArgs> connectionEvent,
            string ip)
        {
            connectionEvent?.Invoke(
                this,
                new SignalRConnectionEventArgs
                {
                    Id = int.Parse(ip.Split('.').Last()),
                    Ip = ip,
                    
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
    }
}