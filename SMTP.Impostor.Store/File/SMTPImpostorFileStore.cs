using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Store.File
{

    public class SMTPImpostorFileStore : ISMTPImpostorStore
    {
        readonly string _path;

        public SMTPImpostorFileStore(
            SMTPImpostorFileStoreSettings settings)
        {
            if (settings is null)
                throw new ArgumentNullException(nameof(settings));

            _path = Path.Combine(settings.Path ?? Path.GetTempPath(), "Impostor");
            Directory.CreateDirectory(_path);

            //Process.Start(new ProcessStartInfo
            //{
            //    FileName = _path,
            //    UseShellExecute = true,
            //    Verb = "open"
            //});
            Console.WriteLine($"Impostor file store \"{_path}\"");
        }

        async Task<SMTPImpostorMessage> ISMTPImpostorStore.GetAsync(
            string host,
            string messageId)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException("message", nameof(host));
            if (string.IsNullOrWhiteSpace(messageId))
                throw new ArgumentNullException(nameof(messageId));

            var messagePath = GetMessageFilePath(host, messageId);
            if (System.IO.File.Exists(messagePath))
            {
                var content = await System.IO.File.ReadAllTextAsync(messagePath);
                return SMTPImpostorMessage.FromContent(content);
            }

            return null;
        }

        async Task ISMTPImpostorStore.PutAsync(string host, SMTPImpostorMessage message)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException("message", nameof(host));
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            var messagePath = GetMessageFilePath(host, message.Id);
            await System.IO.File.WriteAllTextAsync(messagePath, message.Content);
        }

        Task ISMTPImpostorStore.SearchAsync(SMTPImpostorStoreSearchCriteria criteria)
        {
            if (criteria is null)
                throw new ArgumentNullException(nameof(criteria));

            throw new NotImplementedException();
        }

        string GetEnsureMessagePath(string host)
        {
            var path = Path.Combine(_path, host.Replace(":", "_"));
            Directory.CreateDirectory(path);

            return path;
        }

        string GetMessageFilePath(string host, string messageId)
        {
            return Path.Combine(GetEnsureMessagePath(host), $"{messageId}.eml");
        }
    }
}
