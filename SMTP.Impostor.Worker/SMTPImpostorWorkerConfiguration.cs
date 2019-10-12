using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SMTP.Impostor.Worker
{
    public static class SMTPImpostorWorkerConfiguration
    {
        public static IServiceCollection AddSMTPImpostorWorker(
            this IServiceCollection services)
        {
            services.TryAddSingleton<SMTPImpostorSerialization>();
            services.AddHostedService<SMTPImpostorWorkerService>();

            return services;
        }
    }
}
