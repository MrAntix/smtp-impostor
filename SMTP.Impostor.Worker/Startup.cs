using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMTP.Impostor.Store.File;
using SMTP.Impostor.Worker.Hubs;
using SMTP.Impostor.Worker.Properties;

namespace SMTP.Impostor.Worker
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Settings = configuration.Get<SMTPImpostorServerSettings>();
        }

        public IConfiguration Configuration { get; }
        public SMTPImpostorServerSettings Settings { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "www";
            });

            services
                .AddSMTPImpostor(Settings.Impostor)
                .AddSMTPImpostorFileStore(Settings.ImpostorFileStore)
                .AddSMTPImpostorHub()
                .AddHostedService<SMTPImpostorService>();
        }

        public void Configure(
            IApplicationBuilder app, IWebHostEnvironment env,
            ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSMTPImpostorHub()
                .UseSpa(spa =>
                {
                    spa.Options.SourcePath = "src";
                });
        }
    }
}
