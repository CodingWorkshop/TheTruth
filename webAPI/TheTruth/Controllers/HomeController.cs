using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Sockets;
using Newtonsoft.Json;
using TheTruth.Hubs;
using TheTruth.Models;

namespace TheTruth.Controllers {
    public class HomeController : Controller {
        private static HubConnectionBuilder _hub;
        public HomeController () {
            _hub = new HubConnectionBuilder ();
        }
        public async Task<IActionResult> Index () {

            var connection = _hub
                .WithUrl ("http://localhost:5000/hubs")
                .Build ();

            Console.WriteLine ("Starting connection.");
            await connection.StartAsync ();
            Console.WriteLine ("Starting Done.");
              connection.On<string> ("playVideo", data => {
                Console.WriteLine (data);
            });
            await connection.InvokeAsync ("requestVideo");

            //Console.WriteLine ("Dispose connection.");
            //await connection.DisposeAsync();
            //Console.WriteLine ("Dispose done.");
          
            return View ();
        }

        public IActionResult About () {
            ViewData["Message"] = $"Your application description page.";
            return View ();
        }

        public IActionResult Contact () {
            ViewData["Message"] = "Your contact page.";
            return View ();
        }

        public IActionResult Error () {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}