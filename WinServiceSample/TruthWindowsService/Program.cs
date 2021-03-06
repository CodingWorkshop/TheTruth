﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TruthWindowsService.CustomHost;

namespace TruthWindowsService
{
    public class Program
    {
        private static string pathToContentRoot;

        public static void Main(string[] args)
        {
            bool isService = true;
            if (Debugger.IsAttached || args.Contains("--console"))
            {
                isService = false;
            }

            pathToContentRoot = Directory.GetCurrentDirectory();
            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                pathToContentRoot = Path.GetDirectoryName(pathToExe);
            }

            if (isService)
            {
                BuildWebHost(args).RunAsCustomService();
            }
            else
            {
                BuildWebHost(args).Run();
            }

            //var logger = NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            //logger.Debug("init main");
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseHttpSys(options =>
                {
                    options.Authentication.AllowAnonymous = true;
                    options.Authentication.Schemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.NTLM;
                    options.MaxConnections = 100;
                    options.MaxRequestBodySize = 30000000;
                    options.UrlPrefixes.Add("http://localhost:5000");
                })
                .UseStartup<Startup>()
                .UseContentRoot(pathToContentRoot)
                .UseNLog()
                .Build();
    }
}