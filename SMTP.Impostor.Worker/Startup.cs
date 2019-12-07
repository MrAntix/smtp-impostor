using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SMTP.Impostor.Stores.FileSystem.HostSettings;
using SMTP.Impostor.Stores.FileSystem.Messages;
using SMTP.Impostor.Worker.Hubs;
using SMTP.Impostor.Worker.Properties;

namespace SMTP.Impostor.Worker
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Settings = configuration.Get<Settings>();
        }

        public IConfiguration Configuration { get; }
        public Settings Settings { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "www";
            });

            services
                .AddSMTPImpostor(Settings.Impostor)
                .AddSMTPImpostorFileSystemMessagesStore()
                .AddSMTPImpostorFileSystemHostSettingsStore()
                .AddSMTPImpostorHub()
                .AddSMTPImpostorWorker(Settings.Impostor);
        }

        public void Configure(
            IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSMTPImpostorHub()
                .UseSpa(spa =>
                {
                    spa.Options.SourcePath = "src";
                });
        }
    }
}
