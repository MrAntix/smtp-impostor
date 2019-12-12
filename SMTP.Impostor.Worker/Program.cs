using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;

namespace SMTP.Impostor.Worker
{
    public class Program
    {
        const string MUTEX_NAME = "SMTP.Impostor.Worker";

        public static void Main(string[] args)
        {
            if (!Mutex.TryOpenExisting(MUTEX_NAME, out _))
            {
                var mutex = new Mutex(false, MUTEX_NAME);
                CreateHostBuilder(args).Build().Run();
                mutex.Close();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var path = AppDomain.CurrentDomain.BaseDirectory;
                    config.SetBasePath(path);                                   // loads the appsettings
                    hostingContext.HostingEnvironment.ContentRootPath = path;   // for webapp
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<SMTPImpostorWorkerService>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
