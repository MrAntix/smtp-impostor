using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Stores.FileSystem.Messages
{

    public class SMTPImpostorFileSystemMessagesStore : ISMTPImpostorMessagesStore
    {
        readonly ILogger<SMTPImpostorFileSystemMessagesStore> _logger;

        public SMTPImpostorFileSystemMessagesStore(
            ILogger<SMTPImpostorFileSystemMessagesStore> logger,
            ISMTPImpostorSettings settings)
        {
            _logger = logger ?? NullLogger<SMTPImpostorFileSystemMessagesStore>.Instance;
            if (settings is null)
                throw new ArgumentNullException(nameof(settings));

            StorePath = settings.FileStoreRoot;
            Directory.CreateDirectory(StorePath);

            _logger.LogInformation($"Impostor file store \"{StorePath}\"");

            //TryOpenStorePath();
        }

        public readonly string StorePath;

        public bool TryOpenStorePath()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = StorePath,
                    UseShellExecute = true,
                    Verb = "open"
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Could not launch store path {StorePath}");
            }

            return false;
        }

        async Task<SMTPImpostorMessage> ISMTPImpostorMessagesStore
            .GetAsync(Guid hostId, string messageId)
        {
            if (Guid.Empty == hostId)
                throw new ArgumentException("message", nameof(hostId));
            if (string.IsNullOrWhiteSpace(messageId))
                throw new ArgumentNullException(nameof(messageId));

            var messagePath = GetMessageFilePath(hostId, messageId);
            if (File.Exists(messagePath))
            {
                var content = await File.ReadAllTextAsync(messagePath);
                return SMTPImpostorMessage.FromContent(content);
            }

            return null;
        }

        async Task ISMTPImpostorMessagesStore
            .PutAsync(Guid hostId, SMTPImpostorMessage message)
        {
            if (Guid.Empty == hostId)
                throw new ArgumentException("message", nameof(hostId));
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            var messagePath = GetMessageFilePath(hostId, message.Id);
            await File.WriteAllTextAsync(messagePath, message.Content);
        }

        async Task<IImmutableList<SMTPImpostorMessage>> ISMTPImpostorMessagesStore
            .SearchAsync(SMTPImpostorMessageStoreSearchCriteria criteria)
        {
            if (criteria is null)
                throw new ArgumentNullException(nameof(criteria));

            return new SMTPImpostorMessage[] { }.ToImmutableList();
        }

        string GetEnsureMessagePath(Guid hostId)
        {
            var path = Path.Combine(StorePath, hostId.ToString());
            Directory.CreateDirectory(path);

            return path;
        }

        string GetMessageFilePath(Guid hostId, string messageId)
        {
            return Path.Combine(GetEnsureMessagePath(hostId), $"{messageId}.eml");
        }
    }
}
