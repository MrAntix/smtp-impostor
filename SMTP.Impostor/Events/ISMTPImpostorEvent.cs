using System;

namespace SMTP.Impostor.Events
{
    public interface ISMTPImpostorEvent
    {
        Guid Id { get; }
        DateTimeOffset On { get; }
        Guid HostId { get; }
    }
}
