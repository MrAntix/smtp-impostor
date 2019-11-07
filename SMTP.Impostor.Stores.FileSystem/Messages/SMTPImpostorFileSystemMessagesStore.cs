using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SMTP.Impostor.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SMTP.Impostor.Stores.FileSystem.Messages
{

    public class SMTPImpostorFileSystemMessagesStore : ISMTPImpostorMessagesStore
    {
        public const string MESSAGE_EXTN = ".eml";

        readonly ILogger<ISMTPImpostorMessagesStore> _logger;
        readonly IList<SMTPImpostorMessageInfo> _messageIndex;

        public SMTPImpostorFileSystemMessagesStore(
            ILogger<ISMTPImpostorMessagesStore> logger,
            ISMTPImpostorSettings settings,
            Guid hostId)
        {
            _logger = logger ?? NullLogger<SMTPImpostorFileSystemMessagesStore>.Instance;
            if (settings is null)
                throw new ArgumentNullException(nameof(settings));

            StorePath = Path.Combine(settings.FileStoreRoot, hostId.ToString());
            Directory.CreateDirectory(StorePath);

            _logger.LogInformation($"Impostor file store \"{StorePath}\"");

            //TryOpenStorePath();
            _messageIndex = new List<SMTPImpostorMessageInfo>();
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
            .GetAsync(string messageId)
        {
            if (string.IsNullOrWhiteSpace(messageId))
                throw new ArgumentNullException(nameof(messageId));

            if (CheckMessagePath(out var path, false))
            {
                var messagePath = GetMessageFilePath(path, messageId);
                if (File.Exists(messagePath))
                {
                    var content = await File.ReadAllTextAsync(messagePath);
                    return SMTPImpostorMessage.FromContent(content);
                }
            }

            return null;
        }

        async Task ISMTPImpostorMessagesStore
            .PutAsync(SMTPImpostorMessage message)
        {
            CheckMessagePath(out var path, true);

            if (message is null)
                throw new ArgumentNullException(nameof(message));

            var messagePath = GetMessageFilePath(path, message.Id);
            await File.WriteAllTextAsync(messagePath, message.Content);
        }

        async Task<SMTPImpostorMessageStoreSearchResult> ISMTPImpostorMessagesStore
            .SearchAsync(SMTPImpostorMessageStoreSearchCriteria criteria)
        {
            if (criteria is null)
                throw new ArgumentNullException(nameof(criteria));

            if (CheckMessagePath(out var path, false))
            {
                var directory = new DirectoryInfo(path);
                var query = directory.EnumerateFiles($"*{MESSAGE_EXTN}")
                    .OrderByDescending(fi => fi.CreationTimeUtc)
                    .AsEnumerable();

                var totalCount = query.Count();

                if (criteria.Ids?.Any() ?? false)
                {
                    query = query.Where(info =>
                        criteria.Ids.Any(id => info.FullName == GetMessageFilePath(path, id))
                        );
                }

                var messages = await Task.WhenAll(
                    query
                        .Skip(criteria.Index).Take(criteria.Count)
                        .Select(async fi =>
                        {
                            var content = await File.ReadAllTextAsync(fi.FullName);
                            return SMTPImpostorMessage.FromContent(content);
                        })
                    );

                return new SMTPImpostorMessageStoreSearchResult(
                    criteria.Index, totalCount,
                    messages);
            }

            return SMTPImpostorMessageStoreSearchResult.Empty;
        }

        string GetMessageFilePath(string path, string messageId)
        {
            return Path.Combine(path, $"{messageId}{MESSAGE_EXTN}");
        }

        bool CheckMessagePath(out string path, bool create)
        {
            path = StorePath;
            if (Directory.Exists(path)) return true;

            if (create)
            {
                Directory.CreateDirectory(path);
                return true;
            }

            return false;
        }
    }
}
