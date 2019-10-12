using Microsoft.Extensions.DependencyInjection;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Stores.FileSystem.Messages
{
    public static class SMTPImpostorFileSystemMessagesStoreConfiguration
    {
        public static IServiceCollection AddSMTPImpostorFileSystemMessagesStore(
            this IServiceCollection services)
        {
            services.AddTransient<ISMTPImpostorMessagesStore, SMTPImpostorFileSystemMessagesStore>();

            return services;
        }
    }
}
