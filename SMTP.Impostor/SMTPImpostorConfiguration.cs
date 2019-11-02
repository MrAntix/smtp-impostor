using Microsoft.Extensions.DependencyInjection;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor
{
    public static class SMTPImpostorConfiguration
    {
        public static IServiceCollection AddSMTPImpostor(
            this IServiceCollection services,
            ISMTPImpostorSettings settings = null)
        {
            services.AddSingleton(settings ?? SMTPImpostorSettings.Default);

            services.AddSingleton<SMTPImpostor>();
            services.AddSingleton<ISMTPImpostorHostProvider>(sp => sp.GetRequiredService<SMTPImpostor>());

            return services;
        }
    }
}
