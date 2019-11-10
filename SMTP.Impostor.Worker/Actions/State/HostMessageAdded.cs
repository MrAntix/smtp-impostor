using System;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostMessageAdded : HostMessageIdentity
    {
        public HostMessageAdded(
            Guid hostId,
            string messageId) :
            base(hostId, messageId)
        {
        }
    }
}
