using System;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostMessageRemoved : HostMessageIdentity
    {
        public HostMessageRemoved(
            Guid hostId,
            string messageId) :
            base(hostId, messageId)
        {
        }
    }
}
