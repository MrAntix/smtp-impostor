using System;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostMessageIdentity
    {
        public HostMessageIdentity(
            Guid hostId,
            string messageId)
        {
            HostId = hostId;
            MessageId = messageId;
        }

        public Guid HostId { get; }
        public string MessageId { get; }
    }
}
