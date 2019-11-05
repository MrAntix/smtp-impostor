using SMTP.Impostor.Messages;
using System;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostMessagesLoaded
    {
        public HostMessagesLoaded(
            Guid hostId,
            SMTPImpostorMessageStoreSearchResult result)
        {
            HostId = hostId;
            Result = result;
        }

        public Guid HostId { get; }
        public SMTPImpostorMessageStoreSearchResult Result { get; }
    }
}
