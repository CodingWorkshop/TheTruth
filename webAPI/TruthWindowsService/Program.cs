using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace TruthWindowsService
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
            .UseUrls("http://localhost:5000")
            .UseStartup<Startup>()
            .UseNLog()
            .Build();
    }
}