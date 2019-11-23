using Microsoft.Extensions.Logging;
using SMTP.Impostor.Messages;
using System;

namespace SMTP.Impostor.Stores.InMemory.Messages
{
    public class SMTPImpostorInMemoryMessagesStoreProvider :
       ISMTPImpostorMessagesStoreProvider
    {
        readonly ILogger<ISMTPImpostorMessagesStore> _logger;

        public SMTPImpostorInMemoryMessagesStoreProvider(
            ILogger<ISMTPImpostorMessagesStore> logger)
        {
            _logger = logger;
        }

        string ISMTPImpostorMessagesStoreProvider.Type => "FileSystem";

        ISMTPImpostorMessagesStore ISMTPImpostorMessagesStoreProvider
            .Create(
                Guid hostId,
                SMTPImpostorMessagesStoreSettings settings)
        {
            return new SMTPImpostorInMemoryMessagesStore(
                _logger,
                settings);
        }
    }
}
