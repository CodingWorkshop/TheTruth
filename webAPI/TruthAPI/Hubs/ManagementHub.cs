using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TruthAPI.ViewModels;
using VideoService.Interface;

namespace TruthAPI.Hubs
{
    public class ManagementHub : Hub<IManagementHub>
    {
        private IHttpContextAccessor _accessor;
        private IVideoService _videoService;
        public static event EventHandler NotifyEvent;

        public ManagementHub(IHttpContextAccessor accessor, IVideoService videoService)
        {
            _accessor = accessor;
            _videoService = videoService;
        }

        /// <summary>
        /// 管端來取學生機器清單
        /// </summary>
        /// <returns></returns>
        [HubMethodName("getonlineusers")]
        public Task GetOnlineUsers()
        {
            return Clients.All.GetOnlineUser(
                _videoService
                    .GetClientIdentities()
                    .Select(r=> new ClientIdentityViewModel
                    {
                        Id = r.Id,
                        IsActive = r.IsActive,
                        IsOnline = r.IsOnline,
                    }));
        }

        /// <summary>
        /// 連線進來
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            DoNotifyEvent();
            return base.OnConnectedAsync();
        }

        public static void AddNotifyEvent(
            EventHandler e)
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

        /// <summary>
        /// Get IP
        /// </summary>
        /// <returns></returns>
        private string GetRemoteIpAddress()
        {
            return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }

    public interface IManagementHub
    {
        Task GetOnlineUser(IEnumerable<ClientIdentityViewModel> clientIdentities);
    }

}