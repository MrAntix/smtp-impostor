using SMTP.Impostor.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace SMTP.Impostor
{
    public static class MessageExtensions
    {
        public static readonly Regex EMAIL_MATCHER
            = new Regex(@"([""']?(?<name>.*?)[""']?\s*<(?<email>.*?@[^\s,]*)>|(?<email>.*?@[^\s,]*))\s*,?\s*",
                RegexOptions.Compiled);

        public static MailAddress ToMailAddress(this string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            var match = EMAIL_MATCHER.Match(text);
            if (!match.Success) throw new SMTPImpostorInvalidMailAddressException(text);

            return string.IsNullOrEmpty(match.Groups["name"].Value)
                       ? new MailAddress(match.Groups["email"].Value)
                       : new MailAddress(match.Groups["email"].Value, match.Groups["name"].Value);
        }

        public static IEnumerable<MailAddress> ToMailAddresses(this string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            var list = new List<MailAddress>();
            var match = EMAIL_MATCHER.Match(text);
            while (match.Success)
            {
                list.Add(new MailAddress(
                             match.Groups["email"].Value,
                             match.Groups["name"].Value));

                match = match.NextMatch();
            }

            return list.ToArray();
        }

        public static string TryGetValue(
            this IEnumerable<SMTPImpostorMessageHeader> message,
            string name, string part = null,
            Func<string, string> process = null)
        {
            var header = message.FirstOrDefault(h => h.Name == name);
            if (header == null) return null;

            var value = part == null
                ? header.Value
                : header.Value.Split(";")
                    .Select(p => p.Split("="))
                    .Where(p => p[0].Trim() == part)
                    .Select(p => p.ElementAtOrDefault(1))
                    .FirstOrDefault();

            return process == null ? value : process(value);
        }

        public static Encoding TryGetEncoding(
            this SMTPImpostorMessage message)
        {
            var charmap = message.Headers
                .TryGetValue(SMTPImpostorMessageHeader.CONTENT_TYPE, SMTPImpostorMessageHeader.PART_CHAR_SET);
            return charmap == null ? null : Encoding.GetEncoding(charmap);
        }

        public static Encoding GetEncodingOrUTF8(
            this SMTPImpostorMessage message)
        {
            return message.TryGetEncoding()
                ?? Encoding.GetEncoding("utf-8");
        }

        public static string DecodeContent(
            this SMTPImpostorMessage message)
        {
            var encoding = message.GetEncodingOrUTF8();

            return SMTPImpostorDecoder.FromQuotedPrintable(message.Content, encoding);
        }
    }
}
