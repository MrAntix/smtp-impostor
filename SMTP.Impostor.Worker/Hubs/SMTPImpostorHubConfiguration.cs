using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SMTP.Impostor.Worker.Hubs.Actions;

namespace SMTP.Impostor.Worker.Hubs
{
    public static class SMTPImpostorHubConfiguration
    {
        const string HUB_PATH = "/hub";

        public static IServiceCollection AddSMTPImpostorHub(
            this IServiceCollection services)
        {
            services.AddSingleton<SMTPImpostorHubService>();
            services.AddSingleton<IHubActionExecutor, HubActionExecutor>();
            foreach (var actionType in typeof(HubActionExecutor)
                .Assembly.GetTypes()
                .Where(t =>
                    !t.IsAbstract
                    && typeof(IHubAction).IsAssignableFrom(t)))
            {
                services.AddSingleton(actionType);
                services.AddSingleton(sp => sp.GetRequiredService(actionType) as IHubAction);
            }

            return services;
        }

        public static IApplicationBuilder UseSMTPImpostorHub(
            this IApplicationBuilder app)
        {
            var hub = app.ApplicationServices.GetRequiredService<SMTPImpostorHubService>();

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
                });

            return app;
        }
    }
}
