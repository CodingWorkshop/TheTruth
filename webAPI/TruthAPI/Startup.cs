using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TruthAPI.Hubs;

namespace TruthAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSignalR(options =>
            {
                // Faster pings for testing
                options.KeepAliveInterval = TimeSpan.FromSeconds(5);
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            services.AddMvc();
            services.AddOptions();
            services.AddSingleton<VideoService.Interface.IVideoService, VideoService.Service.VideoService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async(context, next) =>
            {
                await next();

                if(context.Response.StatusCode == 404 &&
                    !System.IO.Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api"))
                {
                    context.Request.Path = "/index.html";
                    context.Response.StatusCode = 200;
                    await next();
                }
            });
            app.UseCors("CorsPolicy");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<VideoHub>("/videohub");
                routes.MapHub<ManagementHub>("/managementhub");
            });
            app.UseMvc();
        }
    }
}