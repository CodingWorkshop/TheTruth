using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TheTruth.ViewModels;
using Utility;

namespace TheTruth.Hubs
{
    public class ManagementHub : Hub
    {
        private IHttpContextAccessor _accessor;

        public ManagementHub(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;
            Utility.VideoUtility.OnlineUserEvent += new EventHandler(this.DoNotify);
        }

        /// <summary>
        /// 管端來取學生機器清單
        /// </summary>
        /// <returns></returns>
        [HubMethodName("getonlineusers")]
        public Task GetOnlineUsers()
        {
            return Clients.Caller.SendAsync("getonlineusers", Utility.VideoUtility.GetClientConnetionIdDic().Count);
        }

        /// <summary>
        /// 連線進來
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} 管端 Login");
            return Clients.Caller.SendAsync("management", "Login Ok");
        }

        /// <summary>
        /// 離線
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} 管端 Log Out");
            return Clients.All.SendAsync("management", "Bye");
        }

        /// <summary>
        /// Get IP
        /// </summary>
        /// <returns></returns>
        private string GetRemoteIpAddress()
        {
            return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        private void DoNotify(object o, EventArgs e)
        {
            Clients.Caller.SendAsync("getonlineusers", Utility.VideoUtility.GetClientConnetionIdDic().Count);
        }
    }
}