using System;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostMessage : HostMessageInfo
    {
        public HostMessage(
            string id,
            DateTimeOffset date,
            string from,
            string subject,
            string content) : base(id, date, from, subject)
        {
            Content = content;
        }

        public string Content { get; }
    }
}
