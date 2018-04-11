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
        private int userCount = Utility.VideoUtility.GetClientConnetionIdDic().Count;
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
            return Clients.All.getOnlineUsers(userCount);
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
}
public interface IManagementHub
{
    Task getOnlineUsers(int count);
}