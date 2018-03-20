﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using TheTruth.Hubs;
using TheTruth.Models;

namespace TheTruth.Controllers {
    public class HomeController : Controller {
        public async Task<IActionResult> Index () {

            // TODO TempCode
            // var connection = new HubConnectionBuilder ()
            //     .WithUrl ("http://localhost:5000/hubs")
            //     .Build ();
            // await connection.StartAsync ();
            // Console.WriteLine ($"Starting connection. Press Ctrl-C to close.");
            // await connection.InvokeAsync ("requestVideo");

            return View ();
        }

        public IActionResult About () {
            ViewData["Message"] = "Your application description page.";

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