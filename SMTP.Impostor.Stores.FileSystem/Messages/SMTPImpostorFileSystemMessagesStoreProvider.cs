using System;
using Microsoft.Extensions.Logging;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Stores.FileSystem.Messages
{
    public class SMTPImpostorFileSystemMessagesStoreProvider :
        ISMTPImpostorMessagesStoreProvider
    {
        readonly ILogger<ISMTPImpostorMessagesStore> _logger;
        readonly ISMTPImpostorSettings _settings;

        public SMTPImpostorFileSystemMessagesStoreProvider(
            ILogger<ISMTPImpostorMessagesStore> logger,
            ISMTPImpostorSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        string ISMTPImpostorMessagesStoreProvider.Type => "FileSystem";

        ISMTPImpostorMessagesStore ISMTPImpostorMessagesStoreProvider
            .Create(
                Guid hostId,
                SMTPImpostorMessagesStoreSettings settings)
        {
            return new SMTPImpostorFileSystemMessagesStore(
                _logger,
                hostId,
                new SMTPImpostorFileSystemMessagesStoreSettings(settings, _settings.FileStoreRoot));
        }
    }
}
