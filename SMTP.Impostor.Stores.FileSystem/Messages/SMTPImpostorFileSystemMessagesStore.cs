using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SMTP.Impostor.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace SMTP.Impostor.Stores.FileSystem.Messages
{

    public class SMTPImpostorFileSystemMessagesStore : ISMTPImpostorMessagesStore
    {
        public const string MESSAGE_EXTN = ".eml";
        public readonly string MESSAGE_FILTER = $"*{MESSAGE_EXTN}";

        readonly ILogger<ISMTPImpostorMessagesStore> _logger;
        readonly Subject<ISMTPImpostorMessageEvent> _events;
        readonly IList<SMTPImpostorMessageInfo> _messageIndex;
        readonly FileSystemWatcher _watcher;

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
            _events = new Subject<ISMTPImpostorMessageEvent>();

            //TryOpenStorePath();
            _messageIndex = new List<SMTPImpostorMessageInfo>();
            _watcher = new FileSystemWatcher(StorePath, MESSAGE_FILTER);
            _watcher.Created += (sender, e) =>
            {
                var messageId = Path.GetFileNameWithoutExtension(e.FullPath);

                _events.OnNext(new SMTPImpostorMessageAddedEvent(hostId, messageId));
            };
            _watcher.Deleted += async (sender, e) =>
            {
                var messageId = Path.GetFileNameWithoutExtension(e.FullPath);

                _events.OnNext(new SMTPImpostorMessageRemovedEvent(hostId, messageId));
            };
            _watcher.EnableRaisingEvents = true;
        }

        public readonly string StorePath;
        public IObservable<ISMTPImpostorMessageEvent> Events => _events;

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
            .GetAsync(string messageId) => await GetAsync(messageId);

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
                            var messageId = Path.GetFileNameWithoutExtension(fi.FullName);

                            return SMTPImpostorMessage.FromContent(content, messageId);
                        })
                    );

                return new SMTPImpostorMessageStoreSearchResult(
                    criteria.Index, totalCount,
                    messages);
            }

            return SMTPImpostorMessageStoreSearchResult.Empty;
        }

        Task ISMTPImpostorMessagesStore.DeleteAsync(string messageId)
        {
            if (string.IsNullOrWhiteSpace(messageId))
                throw new ArgumentNullException(nameof(messageId));

            if (CheckMessagePath(out var path, false))
            {
                var messagePath = GetMessageFilePath(path, messageId);
                if (File.Exists(messagePath))
                {
                    File.Delete(messagePath);
                }
            }

            return Task.CompletedTask;
        }

        async Task<SMTPImpostorMessage> GetAsync(string messageId)
        {
            if (string.IsNullOrWhiteSpace(messageId))
                throw new ArgumentNullException(nameof(messageId));

            if (CheckMessagePath(out var path, false))
            {
                var messagePath = GetMessageFilePath(path, messageId);
                if (File.Exists(messagePath))
                {
                    var content = await File.ReadAllTextAsync(messagePath);
                    return SMTPImpostorMessage.FromContent(content, messageId);
                }
            }

            return null;
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

        bool _disposed = false;

        void IDisposable.Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _events.OnCompleted();
                _watcher.Dispose();
            }
        }
    }
}
