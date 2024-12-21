using System;

namespace SMTP.Impostor.Events
{
    public interface ISMTPImpostorHostEvent : ISMTPImpostorEvent
    {
        Guid HostId { get; }
    }
}
