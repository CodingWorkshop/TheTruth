using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TheTruth.ViewModels;

namespace TheTruth.Hubs {

    public class VideoHub : Hub {

        private NLog.Logger _logger;
        public VideoHub (NLog.Logger logger) {
            _logger = logger;
        }
        /// <summary>
        /// Client 端 來取Video
        /// </summary>
        /// <returns></returns>
        [HubMethodName("requestVideo")]
        public Task RequestVideo () {
            string ip = GetRemoteIpAddress ();
            //Console.WriteLine(ip);
            var videos = Utility.GetIpVideoDic().GetValueOrDefault(ip)?
            .Select (r => new VideoViewModel {
                Category = r.Category,
                    Name = r.Name,
                    Code = r.Code,
                    Date = r.Date,
            }).ToList ();
            return Clients.Caller.SendAsync ("PlayVideo", videos);
        }
        /// <summary>
        /// 連線進來
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync () {
            Utility.GetIpConnetionIdDic ().TryAdd (GetRemoteIpAddress (), Context.ConnectionId);
            return Clients.Caller.SendAsync ("PointMe", "Ok");
        }

        /// <summary>
        /// 離線
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync (Exception exception) {
            Utility.GetIpConnetionIdDic ().Remove (GetRemoteIpAddress ());
            return Clients.Caller.SendAsync ("PointMe", "Bye");
        }

        /// <summary>
        /// Get IP
        /// </summary>
        /// <returns></returns>
        private string GetRemoteIpAddress () {
            return Context?.Connection.RemoteIpAddress.MapToIPv4 ().ToString () ?? "127.0.0.1";
        }
    }
}