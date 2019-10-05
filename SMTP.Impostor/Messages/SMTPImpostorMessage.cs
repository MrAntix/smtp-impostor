using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Mail;
using SMTP.Impostor.Sockets;

namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessage
    {
        public SMTPImpostorMessage(
            string id,
            IEnumerable<SMTPImpostorMessageHeader> headers,
            string subject,
            MailAddress from,
            IEnumerable<MailAddress> to,
            IEnumerable<MailAddress> cc,
            DateTimeOffset date,
            string content)
        {
            Id = id;
            Headers = headers?.ToImmutableList() ?? ImmutableList<SMTPImpostorMessageHeader>.Empty;
            Subject = subject;
            From = from;
            To = to?.ToImmutableList() ?? ImmutableList<MailAddress>.Empty;
            Cc = cc?.ToImmutableList() ?? ImmutableList<MailAddress>.Empty;
            Date = date;
            Content = content;
        }

        public string Id { get; }
        public IImmutableList<SMTPImpostorMessageHeader> Headers { get; }
        public string Subject { get; }
        public MailAddress From { get; }
        public IImmutableList<MailAddress> To { get; }

        public IImmutableList<MailAddress> Cc { get; }
        public DateTimeOffset Date { get; }
        public string Content { get; }

        public static SMTPImpostorMessage FromContent(string content)
        {
            var headers = SocketHandler.GetHeaders(content);

            return new SMTPImpostorMessage(
                headers.TryGetValue(SMTPImpostorMessageHeader.MESSAGE_ID) ?? Guid.NewGuid().ToString(),
                headers,
                headers.TryGetValue(SMTPImpostorMessageHeader.SUBJECT) ?? string.Empty,
                headers.TryGetValue(SMTPImpostorMessageHeader.FROM)?.ToMailAddress(),
                headers.TryGetValue(SMTPImpostorMessageHeader.TO)?.ToMailAddresses(),
                headers.TryGetValue(SMTPImpostorMessageHeader.CC)?.ToMailAddresses(),
                DateTimeOffset.Parse(headers.TryGetValue(SMTPImpostorMessageHeader.DATE)),
                content);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Subject, From);
        }
    }
}
