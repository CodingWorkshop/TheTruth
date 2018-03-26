using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Sockets;
using Newtonsoft.Json;
using TheTruth.Hubs;
using TheTruth.Models;

namespace TheTruth.Controllers
{
    public class HomeController : Controller
    {
        private static HubConnectionBuilder _hub;
        private IHubContext<ManagementHub> _hubcontext;

        public HomeController(IHubContext<ManagementHub> hubContext)
        {
            _hub = new HubConnectionBuilder();
            _hubcontext = hubContext;
        }

        private static int count;

        public async Task<IActionResult> Count()
        {
            await _hubcontext.Clients.All.SendAsync("getonlineusers", Utility.VideoUtility.GetClientConnetionIdDic().Count);
            return Json("");
        }

        public IActionResult Index()
        {
            //Console.WriteLine ("Dispose connection.");
            //await connection.DisposeAsync();
            //Console.WriteLine ("Dispose done.");

            return View();
        }

        public IActionResult About()
        {
            //  var connection = _hub
            //     .WithUrl("http://localhost:5000/videohub")
            //     .Build();

            // // Console.WriteLine("Starting connection.");
            //  await connection.StartAsync();
            // // Console.WriteLine("Starting Done.");
            // // connection.On<string>("playVideo", data => {
            // //     Console.WriteLine(data);
            // // });

            //  await connection.InvokeAsync("getonlineusers",Request.HttpContext.Connection.RemoteIpAddress.ToString());
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}