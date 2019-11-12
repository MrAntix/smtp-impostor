using System;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostMessageInfo
    {
        public HostMessageInfo(
            string id,
            DateTimeOffset date,
            string from,
            string subject)
        {
            Id = id;
            Date = date;
            From = from;
            Subject = subject;
        }

        public string Id { get; }
        public DateTimeOffset Date { get; }
        public string From { get; }
        public string Subject { get; }
    }
}
