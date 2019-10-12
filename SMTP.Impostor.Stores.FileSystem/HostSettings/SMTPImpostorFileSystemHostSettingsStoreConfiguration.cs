using Microsoft.Extensions.DependencyInjection;

namespace SMTP.Impostor.Stores.FileSystem.HostSettings
{
    public static class SMTPImpostorFileSystemHostSettingsStoreConfiguration
    {
        public static IServiceCollection AddSMTPImpostorFileSystemHostSettingsStore(
            this IServiceCollection services)
        {
            services.AddTransient<ISMTPImpostorHostSettingsStore, SMTPImpostorFileSystemHostSettingsStore>();

            return services;
        }
    }
}
