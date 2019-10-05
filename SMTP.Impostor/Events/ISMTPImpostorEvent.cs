using System;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Events
{
    public interface ISMTPImpostorEvent
    {
        Guid Id { get; }
        DateTimeOffset On { get; }
        SMTPImpostorHostSettings HostSettings { get; }
    }
}
