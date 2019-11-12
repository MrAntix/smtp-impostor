using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostMessagesLoaded
    {
        public HostMessagesLoaded(
            Guid hostId,
            int index,
            int total,
            IEnumerable<HostMessageInfo> messages)
        {
            HostId = hostId;
            Index = index;
            Total = total;
            Messages = messages.ToImmutableList();
        }

        public Guid HostId { get; }
        public int Index { get; }
        public int Total { get; }
        public IImmutableList<HostMessageInfo> Messages { get; }
    }
}
