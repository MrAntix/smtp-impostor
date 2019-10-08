using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SMTP.Impostor.Worker.Hubs
{
    public static class SMTPImpostorHubConfiguration
    {
        const string HUB_PATH = "/hub";

        public static IServiceCollection AddSMTPImpostorHub(
            this IServiceCollection services)
        {
            services.AddSingleton<SMTPImpostorHubService>();

            return services;
        }

        public static IApplicationBuilder UseSMTPImpostorHub(
            this IApplicationBuilder app)
        {
            var service = app.ApplicationServices
                .GetRequiredService<SMTPImpostorHubService>();

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
                        await service.ConnectAsync(SMTPImpostorHubClient.Wrap(webSocket));
                    });
                });

            return app;
        }
    }
}
