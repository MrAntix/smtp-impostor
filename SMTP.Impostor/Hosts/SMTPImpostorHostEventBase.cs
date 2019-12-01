using SMTP.Impostor.Events;
using System;

namespace SMTP.Impostor.Hosts
{
    public abstract class SMTPImpostorHostEventBase :
        SMTPImpostorEventBase, ISMTPImpostorHostEvent
    {
        public SMTPImpostorHostEventBase(
            Guid hostId)
        {
            HostId = hostId;
        }

        public Guid HostId { get; }
    }

    public abstract class SMTPImpostorHostEventBase<TData> :
        SMTPImpostorHostEventBase
    {
        public SMTPImpostorHostEventBase(
            Guid hostId,
            TData data) : base(hostId)
        {
            Data = data;
        }

        public TData Data { get; }
    }

}
