using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Mail;

namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessage
    {
        internal const string LINE_TERMINATOR = "\r\n";
        internal const string HEADERS_TERMINATOR = "\r\n\r\n";
        internal const string DATA_TERMINATOR = "\r\n.\r\n";

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
            var headers = GetHeaders(content);

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

        internal static IEnumerable<SMTPImpostorMessageHeader> GetHeaders(string content)
        {
            var headers = new List<SMTPImpostorMessageHeader>();
            if (!content.Contains(HEADERS_TERMINATOR)) return headers;

            // get the headers part of the data an un-fold 
            var rawHeaders = content.Head(HEADERS_TERMINATOR)
                .Replace(LINE_TERMINATOR + "\t", string.Empty)
                .Replace(LINE_TERMINATOR + " ", string.Empty);

            // parse into collection
            foreach (var header in rawHeaders.Split('\n'))
            {
                var value = header;
                var name = StringExtensions.Head(ref value, ":")
                    .Trim().ToLower();

                value = value.Trim();

                if (name.Length > 0)
                {
                    // add as is
                    headers.Add(new SMTPImpostorMessageHeader(
                        name,
                        SMTPImpostorDecoder.FromQuotedWord(value)
                        ));
                }
            }

            return headers;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Subject, From);
        }
    }
}
