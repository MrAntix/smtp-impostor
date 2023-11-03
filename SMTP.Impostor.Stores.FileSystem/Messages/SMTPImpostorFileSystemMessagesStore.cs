using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SMTP.Impostor.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SMTP.Impostor.Stores.FileSystem.Messages
{

    public class SMTPImpostorFileSystemMessagesStore : ISMTPImpostorMessagesStore
    {
        public const string MESSAGE_EXTN = ".eml";
        public readonly string MESSAGE_FILTER = $"*{MESSAGE_EXTN}";

        readonly ILogger<ISMTPImpostorMessagesStore> _logger;
        readonly SMTPImpostorFileSystemMessagesStoreSettings _settings;

        readonly Subject<ISMTPImpostorMessageEvent> _events;
        readonly List<SMTPImpostorMessageInfo> _index;
        readonly object _indexLock = new Object();
        readonly FileSystemWatcher _watcher;

        public SMTPImpostorFileSystemMessagesStore(
            ILogger<ISMTPImpostorMessagesStore> logger,
            Guid hostId,
            SMTPImpostorFileSystemMessagesStoreSettings settings
            )
        {
            _logger = logger ?? NullLogger<ISMTPImpostorMessagesStore>.Instance;
            _settings = settings;
            if (settings is null)
                throw new ArgumentNullException(nameof(settings));

            StorePath = CreateStorePath(settings.FileStoreRoot, hostId);

            _logger.LogInformation($"Impostor file store \"{StorePath}\"");
            _events = new Subject<ISMTPImpostorMessageEvent>();

            _index = new List<SMTPImpostorMessageInfo>();
            LoadMessageIndex();

            _watcher = new FileSystemWatcher(StorePath, MESSAGE_FILTER);
            _watcher.Created += async (sender, e) =>
            {
                var messageId = Path.GetFileNameWithoutExtension(e.FullPath);

                var message = await GetAsync(messageId);
                if (message != null)
                    lock (_indexLock) _index.Insert(0, message);

                _events.OnNext(new SMTPImpostorMessageAddedEvent(hostId, messageId));
                await CheckMaxMessages();
            };
            _watcher.Deleted += async (sender, e) =>
            {
                var messageId = Path.GetFileNameWithoutExtension(e.FullPath);

                lock (_indexLock)
                {
                    var message = _index.FirstOrDefault(m => m.Id == messageId);
                    if (message != null) _index.Remove(message);
                }

                _events.OnNext(new SMTPImpostorMessageRemovedEvent(hostId, messageId));
                await CheckMaxMessages();
            };
            _watcher.EnableRaisingEvents = true;
        }

        private string CreateStorePath(
            string fileStoreRoot, Guid hostId)
        {
            var path = Path.Combine(
                string.IsNullOrWhiteSpace(fileStoreRoot)
                    ? SMTPImpostorSettings.Default.FileStoreRoot
                    : fileStoreRoot,
                hostId.ToString());
            Directory.CreateDirectory(path);

            return path;
        }

        async Task CheckMaxMessages()
        {
            try
            {
                if (_settings.General.MaxMessages > 0)
                {
                    var message = default(SMTPImpostorMessageInfo);
                    lock (_indexLock)
                        message = _index.Skip(_settings.General.MaxMessages).LastOrDefault();

                    if (message != null) await DeleteAsync(message.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckMaxMessages failed");
            }
        }

        public readonly string StorePath;
        public IObservable<ISMTPImpostorMessageEvent> Events => _events;

        Task<SMTPImpostorMessageStoreSearchResult> SearchAsync(SMTPImpostorMessageStoreSearchCriteria criteria)
        {
            if (criteria is null)
                throw new ArgumentNullException(nameof(criteria));

            if (CheckMessagePath(out var path, false))
            {
                var directory = new DirectoryInfo(path);
                var query = _index.ToArray().AsEnumerable();
                var totalCount = query.Count();

                if (criteria.Ids?.Any() ?? false)
                {
                    query = query.Where(info =>
                        criteria.Ids.Any(id => info.Id == id)
                        );
                }

                if (!string.IsNullOrWhiteSpace(criteria.Text))
                {
                    var text = Regex.Escape(SMTPImpostorMessage.TRIM.Replace(criteria.Text, ""));
                    var containsText = new Regex($"^{text}|({SMTPImpostorMessage.BREAK_CHARS}){text}", RegexOptions.IgnoreCase);

                    query = query.Where(info =>
                            containsText.IsMatch(info.Subject)
                            || containsText.IsMatch(info.From.ToString())
                            );
                }

                return Task.FromResult(
                        new SMTPImpostorMessageStoreSearchResult(
                        criteria.Index, totalCount, query
                    )
                );
            }

            return Task.FromResult(
                    SMTPImpostorMessageStoreSearchResult.Empty
                );
        }

        async Task<SMTPImpostorMessage> GetAsync(
            string messageId)
        {
            if (string.IsNullOrWhiteSpace(messageId))
                throw new ArgumentNullException(nameof(messageId));

            if (CheckMessagePath(out var path, false))
            {
                var messagePath = GetMessageFilePath(path, messageId);
                var content = await Delegates.RetryAsync(
                    async () => File.Exists(messagePath)
                        ? await File.ReadAllTextAsync(messagePath)
                        : null,
                        _logger);

                return SMTPImpostorMessage
                    .Parse(content, messageId);
            }

            return null;
        }

        Task LaunchAsync(
            string messageId)
        {
            if (!CheckMessagePath(out var path, false))
                throw new FileNotFoundException(path);

            var messagePath = GetMessageFilePath(path, messageId);
            if (!File.Exists(messagePath))
                throw new FileNotFoundException(messagePath);

            Process.Start(new ProcessStartInfo
            {
                FileName = messagePath,
                UseShellExecute = true,
            });

            return Task.CompletedTask;
        }

        async Task PutAsync(SMTPImpostorMessage message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            CheckMessagePath(out var path, true);

            var messagePath = GetMessageFilePath(path, message.Id);
            await File.WriteAllTextAsync(messagePath, message.Content);
        }

        Task DeleteAsync(string messageId)
        {
            if (string.IsNullOrWhiteSpace(messageId))
                throw new ArgumentNullException(nameof(messageId));

            if (CheckMessagePath(out var path, false))
            {
                var messagePath = GetMessageFilePath(path, messageId);
                Delegates.Retry(() =>
                    {
                        if (File.Exists(messagePath))
                            File.Delete(messagePath);
                    },
                    _logger
                );
            }

            return Task.CompletedTask;
        }

        static string GetMessageFilePath(string path, string messageId)
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

        async void LoadMessageIndex()
        {
            if (CheckMessagePath(out var path, false))
            {
                var directory = new DirectoryInfo(path);
                var query = directory.EnumerateFiles($"*{MESSAGE_EXTN}")
                    .OrderByDescending(fi => fi.CreationTimeUtc)
                    .AsEnumerable();

                await Task.WhenAll(query
                    .Select(async fi =>
                    {
                        var content = await File.ReadAllTextAsync(fi.FullName);
                        var messageId = Path.GetFileNameWithoutExtension(fi.FullName);

                        try
                        {
                            var message = SMTPImpostorMessage.TryParseInfo(content, messageId);
                            lock (_indexLock) _index.Add(message);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Could not load message id {messageId}", messageId);
                        }
                    })
                );
            }
        }

        bool TryOpenStorePath()
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

        int ISMTPImpostorMessagesStore.Count => _index.Count;
        async Task<SMTPImpostorMessageStoreSearchResult> ISMTPImpostorMessagesStore
            .SearchAsync(SMTPImpostorMessageStoreSearchCriteria criteria) => await SearchAsync(criteria);

        async Task<SMTPImpostorMessage> ISMTPImpostorMessagesStore
            .GetAsync(string messageId) => await GetAsync(messageId);

        async Task ISMTPImpostorMessagesStore
            .LaunchAsync(string messageId) => await LaunchAsync(messageId);

        async Task ISMTPImpostorMessagesStore
            .PutAsync(SMTPImpostorMessage message) => await PutAsync(message);

        async Task ISMTPImpostorMessagesStore
            .DeleteAsync(string messageId) => await DeleteAsync(messageId);

        bool _disposed = false;

        void IDisposable.Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _events.OnCompleted();
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
            }
        }
    }
}
