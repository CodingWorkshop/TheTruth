using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace TruthAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var logger = NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            //logger.Debug("init main");
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://:::5000", "http://localhost:5000")
            .UseStartup<Startup>()
            .UseNLog()
            .Build();
    }
}