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
    }
}
