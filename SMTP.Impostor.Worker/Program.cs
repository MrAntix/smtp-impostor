using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime.InteropServices;
using System.Text;

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

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder packageFullName);
        const long ERROR_NO_PACKAGE = 15700L;

        public static bool IsRunningAsUwp()
        {
            if (IsWindows7OrLower()) return false;

            var length = 0;
            var result = GetCurrentPackageFullName(ref length, new StringBuilder(1024));

            return result != ERROR_NO_PACKAGE;
        }

        public static bool IsWindows7OrLower()
        {
            int versionMajor = Environment.OSVersion.Version.Major;
            int versionMinor = Environment.OSVersion.Version.Minor;
            var version = versionMajor + (double)versionMinor / 10;
            return version <= 6.1;
        }
    }
}
