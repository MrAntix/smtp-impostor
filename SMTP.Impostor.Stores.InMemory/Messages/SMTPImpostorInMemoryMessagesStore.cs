using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SMTP.Impostor.Messages;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Subjects;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SMTP.Impostor.Stores.InMemory.Messages
{
    public class SMTPImpostorInMemoryMessagesStore :
        ISMTPImpostorMessagesStore
    {
        static object Lock = new object();
        ILogger<ISMTPImpostorMessagesStore> _logger;
        readonly SMTPImpostorMessagesStoreSettings _settings;

        readonly Subject<ISMTPImpostorMessageEvent> _events;
        readonly IList<SMTPImpostorMessage> _messages;

        public SMTPImpostorInMemoryMessagesStore(
            ILogger<ISMTPImpostorMessagesStore> logger,
            SMTPImpostorMessagesStoreSettings settings)
        {
            _logger = logger ?? NullLogger<ISMTPImpostorMessagesStore>.Instance;
            _settings = settings;
            _logger.LogInformation($"Impostor in memory store");
            _events = new Subject<ISMTPImpostorMessageEvent>();
            _messages = new List<SMTPImpostorMessage>();
        }

        public IImmutableList<SMTPImpostorMessage> Messages => _messages.ToImmutableList();
        public IObservable<ISMTPImpostorMessageEvent> Events => _events;

        Task<SMTPImpostorMessageStoreSearchResult> SearchAsync(
            SMTPImpostorMessageStoreSearchCriteria criteria)
        {
            var query = _messages.ToArray().AsEnumerable();
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
                    criteria.Index, totalCount,
                    query)
                );
        }

        Task<SMTPImpostorMessage> GetMessageAsync(string messageId)
        {
            return Task.FromResult(
                _messages.FirstOrDefault(m => m.Id == messageId)
                );
        }

        Task LaunchMessageAsync(string messageId)
        {
            throw new NotImplementedException();
        }

        Task PutAsync(SMTPImpostorMessage message)
        {
            _messages.Add(message);
            if (_settings.MaxMessages != 0)
                while (_messages.Count > _settings.MaxMessages)
                {
                    _messages.RemoveAt(0);
                }

            return Task.CompletedTask;
        }

        Task DeleteAsync(string messageId)
        {
            lock (Lock)
            {
                var message = GetMessageAsync(messageId)
                .GetAwaiter().GetResult();

                if (message != null)
                    _messages.Remove(message);

            }

            return Task.CompletedTask;
        }

        int ISMTPImpostorMessagesStore.Count => _messages.Count;
        async Task<SMTPImpostorMessageStoreSearchResult> ISMTPImpostorMessagesStore
            .SearchAsync(SMTPImpostorMessageStoreSearchCriteria criteria) => await SearchAsync(criteria);

        async Task ISMTPImpostorMessagesStore
            .LaunchAsync(string messageId) => await LaunchMessageAsync(messageId);

        async Task<SMTPImpostorMessage> ISMTPImpostorMessagesStore
            .GetAsync(string messageId) => await GetMessageAsync(messageId);

        async Task ISMTPImpostorMessagesStore
            .PutAsync(SMTPImpostorMessage message) => await PutAsync(message);

        async Task ISMTPImpostorMessagesStore
            .DeleteAsync(string messageId) => await DeleteAsync(messageId);

        void IDisposable.Dispose() { }
    }
}
