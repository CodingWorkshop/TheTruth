using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TheTruth.ViewModels;

namespace TheTruth.Hubs {

    public class VideoHub : Hub {

        public VideoHub () {
        }
        /// <summary>
        /// Client 端 來取Video
        /// </summary>
        /// <returns></returns>
        [HubMethodName("requestVideo")]
        public Task RequestVideo () {
             Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} come to get Videos");
            string ip = GetRemoteIpAddress ();
            //Console.WriteLine(ip);
            var videos = Utility.GetIpVideoDic().GetValueOrDefault(ip)?
            .Select (r => new VideoViewModel {
                Category = r.Category,
                    Name = r.Name,
                    Code = r.Code,
                    Date = r.Date,
            }).ToList ();
            return Clients.Client(Context.ConnectionId).SendAsync ("playVideo", $" give videos to {Context.ConnectionId}");
        }
        /// <summary>
        /// 連線進來
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync () {
              Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} Login");
            Utility.GetIpConnetionIdDic ().TryAdd (GetRemoteIpAddress (), Context.ConnectionId);
            return Clients.Caller.SendAsync ("playVideo", "Login Ok");
        }

        /// <summary>
        /// 離線
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync (Exception exception) {
            Console.WriteLine($"{GetRemoteIpAddress()} {Context.ConnectionId} Log Out");
            Utility.GetIpConnetionIdDic ().Remove (GetRemoteIpAddress ());
            return Clients.All.SendAsync ("playVideo", "Bye");
        }

        /// <summary>
        /// Get IP
        /// </summary>
        /// <returns></returns>
        private string GetRemoteIpAddress () {
            return Context?.Connection.RemoteIpAddress.ToString() ?? "127.0.0.1";
        }
    }
}