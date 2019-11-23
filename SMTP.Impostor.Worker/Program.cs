using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SMTP.Impostor.Worker
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                    config.SetBasePath(
                        AppDomain.CurrentDomain.BaseDirectory);
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
