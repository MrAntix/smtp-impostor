using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SMTP.Impostor.Worker.Properties;
using Microsoft.AspNetCore.Builder;
using SMTP.Impostor.Store.File;

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
                .AddHostedService<SMTPImpostorService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseSpaStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "src";
            });
        }
    }
}
