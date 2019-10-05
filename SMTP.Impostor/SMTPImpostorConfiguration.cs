using System;
using Microsoft.Extensions.DependencyInjection;
using SMTP.Impostor.Hosts;
using SMTP.Impostor.Sockets;

namespace SMTP.Impostor
{
    public static class SMTPImpostorConfiguration
    {
        public static IServiceCollection AddSMTPImpostor(
            this IServiceCollection services,
            ISMTPImpostorSettings settings = null)
        {
            services.AddSingleton(settings ?? SMTPImpostorSettings.Default);

            services.AddTransient<ISMTPImpostorHost, SMTPImpostorHost>();
            services.AddSingleton<Func<ISMTPImpostorHost>>(
                p => () => p.GetRequiredService<ISMTPImpostorHost>()
                );

            services.AddSingleton<ISMTPImpostorSocketHandlerProvider, SocketHandlerProvider>();
            services.AddTransient<SMTPImpostor>();

            return services;
        }
    }
}
