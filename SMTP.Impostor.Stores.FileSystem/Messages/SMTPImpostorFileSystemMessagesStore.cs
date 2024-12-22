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
        readonly object _indexLock = new();
        KeyedTaskQueue _tasks = new();

        FileSystemWatcher _watcher;

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
            _index = [];

            LoadMessageIndex(hostId);
        }

        private string CreateStorePath(
            string fileStoreRoot, Guid hostId)
        {
            var path = Path.Combine(
                string.IsNullOrWhiteSpace(fileStoreRoot)
                    ? SMTPImpostorSettings.Default.FileStoreRoot
                    : fileStoreRoot,
                hostId.ToString());

            var directoryInfo = Directory.CreateDirectory(path);

            //try
            //{
            //    var directorySecurity = directoryInfo.GetAccessControl();

            //    // Add the desired permissions
            //    directorySecurity.AddAccessRule(new FileSystemAccessRule(
            //        new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
            //        FileSystemRights.FullControl,
            //        InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
            //        PropagationFlags.None,
            //        AccessControlType.Allow));

            //    // Apply the permissions
            //    directoryInfo.SetAccessControl(directorySecurity);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "cannot set permission to the director {Path}", path);
            //}

            return path;
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

        async Task SaveFileAsync(SMTPImpostorMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);

            CheckMessagePath(out var path, true);
            var file = GetMessageFilePath(path, message.Id);

            await File.WriteAllTextAsync(file, message.Content);
        }

        async Task<SMTPImpostorMessage> TryLoadFileAsync(
            string messageId
            )
        {
            ArgumentNullException.ThrowIfNull(messageId);

            CheckMessagePath(out var path, false);
            var file = GetMessageFilePath(path, messageId);

            if (File.Exists(file))
            {
                var content = await File.ReadAllTextAsync(file);

                return SMTPImpostorMessage
                    .Parse(content, messageId);
            }

            return null;
        }

        Task DeleteFileAsync(
            string messageId
            )
        {
            ArgumentNullException.ThrowIfNull(messageId);

            CheckMessagePath(out var path, false);
            var file = GetMessageFilePath(path, messageId);

            if (File.Exists(file))
                File.Delete(file);

            return Task.CompletedTask;
        }

        Task LaunchAsync(
            string messageId
            )
        {
            ArgumentNullException.ThrowIfNull(messageId);

            CheckMessagePath(out var path, false);
            var file = GetMessageFilePath(path, messageId);
            if (!File.Exists(file))
                throw new FileNotFoundException(file);

            Process.Start(new ProcessStartInfo
            {
                FileName = file,
                UseShellExecute = true
            });

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

        async void LoadMessageIndex(Guid hostId)
        {
            if (CheckMessagePath(out var path, false))
            {
                _watcher?.Dispose();
                _watcher = new FileSystemWatcher(StorePath, MESSAGE_FILTER);

                var directory = new DirectoryInfo(path);
                var query = directory.EnumerateFiles($"*{MESSAGE_EXTN}")
                    .OrderByDescending(fi => fi.CreationTimeUtc)
                    .AsEnumerable();

                await Task.WhenAll(query
                    .Select(async (fi, i) =>
                    {
                        var messageId = Path.GetFileNameWithoutExtension(fi.FullName);

                        try
                        {
                            if (i >= _settings.General.MaxMessages)
                            {
                                File.Delete(fi.FullName);
                                return;
                            }

                            var content = await File.ReadAllTextAsync(fi.FullName);

                            var message = SMTPImpostorMessage.TryParseInfo(content, messageId);
                            lock (_indexLock) _index.Add(message);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Could not load message id {messageId}", messageId);
                        }
                    })
                );

                _watcher.Created += (sender, e) =>
                {
                    var messageId = Path.GetFileNameWithoutExtension(e.FullPath);
                    _tasks[messageId].Enqueue(async () =>
                    {
                        var message = await TryLoadFileAsync(messageId);
                        if (message != null)
                        {
                            _index.Insert(0, message);
                            if (_index.Count > _settings.General.MaxMessages)
                            {
                                var last = _index.Last();
                                await DeleteFileAsync(last.Id);

                                _index.Remove(last);
                            }
                        }

                        _events.OnNext(new SMTPImpostorMessageAddedEvent(hostId, messageId));
                    });
                };
                _watcher.Deleted += (sender, e) =>
                {
                    var messageId = Path.GetFileNameWithoutExtension(e.FullPath);
                    _tasks[messageId].Enqueue(() =>
                    {
                        var message = _index.FirstOrDefault(m => m.Id == messageId);
                        if (message is not null) _index.Remove(message);

                        _events.OnNext(new SMTPImpostorMessageRemovedEvent(hostId, messageId));
                    });
                };
                _watcher.EnableRaisingEvents = true;

            }
        }

        int ISMTPImpostorMessagesStore.Count => _index.Count;
        Task<SMTPImpostorMessageStoreSearchResult> ISMTPImpostorMessagesStore
            .SearchAsync(SMTPImpostorMessageStoreSearchCriteria criteria) =>
            _tasks["SEARCH"].EnqueueAsync(() => SearchAsync(criteria));

        Task<SMTPImpostorMessage> ISMTPImpostorMessagesStore
            .GetAsync(string messageId) =>
            _tasks[messageId].EnqueueAsync(() => TryLoadFileAsync(messageId));

        Task ISMTPImpostorMessagesStore
            .LaunchAsync(string messageId) =>
            _tasks[messageId].EnqueueAsync(() => LaunchAsync(messageId));

        Task ISMTPImpostorMessagesStore
            .PutAsync(SMTPImpostorMessage message) =>
            _tasks[message.Id].EnqueueAsync(() => SaveFileAsync(message));

        Task ISMTPImpostorMessagesStore
           .DeleteAsync(string messageId) =>
           _tasks[messageId].EnqueueAsync(() => DeleteFileAsync(messageId));

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
