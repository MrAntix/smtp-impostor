using System;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostIdentity
    {
        public HostIdentity(
            Guid hostId)
        {
            HostId = hostId;
        }

        public Guid HostId { get; }
    }
}
