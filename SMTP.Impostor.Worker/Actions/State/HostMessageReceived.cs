using System;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostMessageReceived
    {
        public HostMessageReceived(
            Guid hostId,
            HostMessageInfo message)
        {
            HostId = hostId;
            Message = message;
        }

        public Guid HostId { get; }
        public HostMessageInfo Message { get; }
     }
}
