using System;
using System.Net.Mail;

namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessageInfo
    {
        public SMTPImpostorMessageInfo(
            string id,
            string subject,
            MailAddress from,
            DateTimeOffset date
            )
        {
            Id = id;
            Subject = subject;
            From = from;
            Date = date;
        }

        public string Id { get; }
        public string Subject { get; }
        public MailAddress From { get; }
        public DateTimeOffset Date { get; }

        public static SMTPImpostorMessageInfo FromContent(string content, string messageId = null)
        {
            var headers = SMTPImpostorMessage.GetHeaders(content);

            return new SMTPImpostorMessageInfo(
                headers.TryGetValue(SMTPImpostorMessageHeader.MESSAGE_ID) ?? messageId ?? Guid.NewGuid().ToString(),
                headers.TryGetValue(SMTPImpostorMessageHeader.SUBJECT) ?? string.Empty,
                headers.TryGetValue(SMTPImpostorMessageHeader.FROM)?.ToMailAddress(),
                DateTimeOffset.Parse(headers.TryGetValue(SMTPImpostorMessageHeader.DATE))
                );
        }
    }
}
