using Microsoft.Extensions.DependencyInjection;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Stores.InMemory.Messages
{
    public static class SMTPImpostorInMemoryMessagesStoreConfiguration
    {
        public static IServiceCollection AddSMTPImpostorInMemoryMessagesStore(
            this IServiceCollection services)
        {
            services.AddTransient<ISMTPImpostorMessagesStoreProvider, SMTPImpostorInMemoryMessagesStoreProvider>();

            return services;
        }
    }
}
