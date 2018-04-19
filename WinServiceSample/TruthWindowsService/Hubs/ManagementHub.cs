using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Truth.ViewModels;

namespace TruthWindowsService.Hubs
{
    public class ManagementHub : Hub<IManagementHub>
    {
        private IHttpContextAccessor _accessor;

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
            Utility.VideoUtility.GetClientInfo().Select(r=> new ClientIdentityViewModel{
                Id = r.Id,
                IsActive = r.IsActive,
                IsOnline = r.IsOnline,
            });
            return Clients.All.GetOnlineUsers();
        }

        /// <summary>
        /// 連線進來
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} 管端 Login");
            GetOnlineUsers();
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
            Utility.VideoUtility.SetNotifyEvent(e);
        }

        /// <summary>
        /// Get IP
        /// </summary>
        /// <returns></returns>
        private string GetRemoteIpAddress()
        {
            return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        //public void DoNotify(object o, EventArgs e)
        //{
        //    Clients.Caller.getOnlineUsers(Utility.VideoUtility.GetClientConnetionIdDic().Count);
        //}
    }

    public interface IManagementHub
    {
        Task GetOnlineUsers(IEnumerable<ClientIdentityViewModel> clientIdentities);
    }
}