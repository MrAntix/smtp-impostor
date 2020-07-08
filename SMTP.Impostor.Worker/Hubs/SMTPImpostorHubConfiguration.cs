using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SMTP.Impostor.Worker.Actions;

namespace SMTP.Impostor.Worker.Hubs
{
    public static class SMTPImpostorHubConfiguration
    {
        const string HUB_PATH = "/hub";
        const string DOWNLOAD_PATH = "/download";

        public static IServiceCollection AddSMTPImpostorHub(
            this IServiceCollection services)
        {
            services.AddSingleton<SMTPImpostorHubService>();
            services.AddSingleton<IActionExecutor, ActionExecutor>();
            services.TryAddSingleton<SMTPImpostorSerialization>();
            foreach (var actionType in typeof(ActionExecutor)
                .Assembly.GetTypes()
                .Where(t =>
                    !t.IsAbstract
                    && typeof(IAction).IsAssignableFrom(t)))
            {
                services.AddSingleton(actionType);
                services.AddSingleton(sp => sp.GetRequiredService(actionType) as IAction);
            }

            return services;
        }

        public static IApplicationBuilder UseSMTPImpostorHub(
            this IApplicationBuilder app)
        {
            var hub = app.ApplicationServices.GetRequiredService<SMTPImpostorHubService>();
            var impostor = app.ApplicationServices.GetRequiredService<SMTPImpostor>();

            app.UseWebSockets()
                .Map(HUB_PATH, hubApp =>
                {
                    hubApp.Use(async (context, next) =>
                    {
                        if (!context.WebSockets.IsWebSocketRequest)
                        {
                            context.Response.StatusCode = 400;
                            return;
                        }

                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await hub.ConnectAsync(SMTPImpostorHubClient.Wrap(webSocket));
                    });
                })
                .Map(DOWNLOAD_PATH, downloadApp =>
                {
                    downloadApp.Use(async (context, next) =>
                    {
                        var host = impostor.Hosts.Values.First();
                        var message = await host.Messages.GetAsync("2945dd3c-873b-4900-8ebd-b9360d2760c6");

                        context.Response.ContentType = "application/eml";
                        context.Response.Headers.Add("Content-Disposition", "inline; filename=\"message.txt\"");
                        await context.Response.Body.WriteAsync(
                            Encoding.UTF8.GetBytes(message.Content)
                            );
                    });
                });

            return app;
        }
    }
}
