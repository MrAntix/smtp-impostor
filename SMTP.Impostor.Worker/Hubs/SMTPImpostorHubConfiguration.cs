using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SMTP.Impostor.Worker.Actions;
using System.Linq;

namespace SMTP.Impostor.Worker.Hubs
{
    public static class SMTPImpostorHubConfiguration
    {
        const string HUB_PATH = "/hub";

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
            var logger = app.ApplicationServices.GetRequiredService<ILogger<SMTPImpostor>>();

            app.UseWebSockets()
                .Map(HUB_PATH, hubApp =>
                {
                    hubApp.Run(async (context) =>
                    {
                        if (!context.WebSockets.IsWebSocketRequest)
                        {
                            context.Response.StatusCode = 400;
                            return;
                        }

                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await hub.ConnectAsync(SMTPImpostorHubClient.Wrap(webSocket));

                    });

                    logger.LogWarning("SMTP Impostor Worker is Started {Hub}", hubApp);
                });

            return app;
        }
    }
}
