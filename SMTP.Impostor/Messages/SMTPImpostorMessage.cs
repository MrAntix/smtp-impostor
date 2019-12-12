using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessage : SMTPImpostorMessageInfo
    {
        internal const string LINE_TERMINATOR = "\r\n";
        internal const string HEADERS_TERMINATOR = "\r\n\r\n";
        internal const string DATA_TERMINATOR = "\r\n.\r\n";

        internal static readonly Regex UNFOLD = new Regex($@"{LINE_TERMINATOR}\s+");


        public const string BREAK_CHARS = "[\\s\\.@]";
        public static readonly Regex TRIM = new Regex($"^{BREAK_CHARS}+", RegexOptions.Compiled);

        public SMTPImpostorMessage(
            string id,
            IEnumerable<SMTPImpostorMessageHeader> headers,
            string subject,
            MailAddress from,
            IEnumerable<MailAddress> to,
            IEnumerable<MailAddress> cc,
            DateTimeOffset date,
            string content) : base(id, subject, from, date)
        {
            Headers = headers?.ToImmutableList() ?? ImmutableList<SMTPImpostorMessageHeader>.Empty;
            To = to?.ToImmutableList() ?? ImmutableList<MailAddress>.Empty;
            Cc = cc?.ToImmutableList() ?? ImmutableList<MailAddress>.Empty;
            Content = content;
        }

        public IImmutableList<SMTPImpostorMessageHeader> Headers { get; }
        public IImmutableList<MailAddress> To { get; }

        public IImmutableList<MailAddress> Cc { get; }
        public string Content { get; }

        public static SMTPImpostorMessage Parse(string content, string messageId = null)
        {
            if (content == null)
                return null;

            var headers = GetHeaders(content);

            return new SMTPImpostorMessage(
                headers.TryGetValue(SMTPImpostorMessageHeader.MESSAGE_ID) ?? messageId ?? Guid.NewGuid().ToString(),
                headers,
                headers.TryGetValue(SMTPImpostorMessageHeader.SUBJECT) ?? string.Empty,
                headers.TryGetValue(SMTPImpostorMessageHeader.FROM)?.ToMailAddress(),
                headers.TryGetValue(SMTPImpostorMessageHeader.TO)?.ToMailAddresses(),
                headers.TryGetValue(SMTPImpostorMessageHeader.CC)?.ToMailAddresses(),
                DateTimeOffset.Parse(headers.TryGetValue(SMTPImpostorMessageHeader.DATE)),
                content);
        }

        public static SMTPImpostorMessageInfo ParseInfo(string content, string messageId = null)
        {
            var headers = GetHeaders(content);

            return new SMTPImpostorMessageInfo(
                headers.TryGetValue(SMTPImpostorMessageHeader.MESSAGE_ID) ?? messageId ?? Guid.NewGuid().ToString(),
                headers.TryGetValue(SMTPImpostorMessageHeader.SUBJECT) ?? string.Empty,
                headers.TryGetValue(SMTPImpostorMessageHeader.FROM)?.ToMailAddress(),
                DateTimeOffset.Parse(headers.TryGetValue(SMTPImpostorMessageHeader.DATE))
                );
        }

        internal static IEnumerable<SMTPImpostorMessageHeader> GetHeaders(string content)
        {
            var headers = new List<SMTPImpostorMessageHeader>();
            if (!content.Contains(HEADERS_TERMINATOR)) return headers;

            // get the headers part of the data and un-fold 
            var rawHeaders = UNFOLD.Replace(
                content.Head(HEADERS_TERMINATOR), " ");

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
