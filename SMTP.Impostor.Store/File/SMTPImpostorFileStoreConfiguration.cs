using Microsoft.Extensions.DependencyInjection;

namespace SMTP.Impostor.Store.File
{
    public static class SMTPImpostorFileStoreConfiguration
    {
        public static IServiceCollection AddSMTPImpostorFileStore(
            this IServiceCollection services,
            SMTPImpostorFileStoreSettings settings = null)
        {
            services.AddSingleton(settings ?? SMTPImpostorFileStoreSettings.Default);

            services.AddTransient<ISMTPImpostorStore, SMTPImpostorFileStore>();

            return services;
        }
    }
}
