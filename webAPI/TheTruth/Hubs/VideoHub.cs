using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TheTruth.Hubs {

    public class VideoHub : Hub {
        public Task SendVideo (string user, string message) {
            string timestamp = DateTime.Now.ToShortTimeString ();
            return Clients.All.SendAsync ("ReceiveMessage", timestamp, user, message);
        }
    }
}