using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SMTP.Impostor.Web.Properties;

namespace SMTP.Impostor.Web
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
            services
                .AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "wwwroot";
                });
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

            app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "src";
                });
        }
    }
}
