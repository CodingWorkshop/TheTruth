using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TruthAPI.ViewModels;
using Utility;

namespace TruthAPI.Hubs
{
    public class ManagementHub : Hub<IManagementHub>
    {
        private IHttpContextAccessor _accessor;
        public static event EventHandler NotifyEvent;
        public ManagementHub(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;
        }

        /// <summary>
        /// 管端來取學生機器清單
        /// </summary>
        /// <returns></returns>
        [HubMethodName("getonlineusers")]
        public Task GetOnlineUsers()
        {
            return Clients.All.GetOnlineUsers( Utility.VideoUtility.GetClientInfo().Select(r=> new ClientIdentityViewModel{
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
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} 管端 Login");
            return base.OnConnectedAsync();

        }

        /// <summary>
        /// 離線
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} 管端 Log Out");
            return base.OnDisconnectedAsync(exception);
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
        Task GetOnlineUsers(IEnumerable<ClientIdentityViewModel> clientIdentities);
    }

}