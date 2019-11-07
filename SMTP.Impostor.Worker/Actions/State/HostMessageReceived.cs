using System;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostMessageReceived
    {
        public HostMessageReceived(
            Guid hostId,
            MessageInfo message)
        {
            HostId = hostId;
            Message = message;
        }

        public Guid HostId { get; }
        public MessageInfo Message { get; }
     }
}
