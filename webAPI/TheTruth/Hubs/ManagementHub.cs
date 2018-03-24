using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TheTruth.ViewModels;

namespace TheTruth.Hubs {

    public class ManagementHub : Hub {
        private IHttpContextAccessor _accessor;
        public ManagementHub(IHttpContextAccessor accessor) {
            this._accessor = accessor;
        }
        /// <summary>
        /// 管端來取學生機器清單
        /// </summary>
        /// <returns></returns>
        [HubMethodName("requestVideo")]
        public Task RequestVideo() {

            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} come to get Videos");
            string ip = GetRemoteIpAddress();
            //Console.WriteLine(ip);
            var videos = Utility.GetIpVideoDic().GetValueOrDefault(ip) ?
                .Select(r => new VideoViewModel {
                    Category = r.Category,
                        Name = r.Name,
                        Code = r.Code,
                        Date = r.Date,
                }).ToList();
            return Clients.Client(Context.ConnectionId).SendAsync("playVideo", $" give videos to {Context.ConnectionId}");
        }
        /// <summary>
        /// 連線進來
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync() {
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} 管端 Login");
            return Clients.Caller.SendAsync("playVideo", "Login Ok");
        }
        /// <summary>
        /// 管端來取學生機器清單
        /// </summary>
        /// <returns></returns>
        [HubMethodName("getonlineusers")]
        public Task GetOnlineUser() {
            var userList = Utility.GetClientConnetionIdDic();
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} 管端");
            return Clients.All.SendAsync("getonlineusers", JsonConvert.SerializeObject(userList));
        }


        /// <summary>
        /// 離線
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception) {
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} 管端 Log Out");
            return Clients.All.SendAsync("playVideo", "Bye");
        }

        /// <summary>
        /// Get IP
        /// </summary>
        /// <returns></returns>
        private string GetRemoteIpAddress() {
            return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}