using System;
using SMTP.Impostor.Events;
using SMTP.Impostor.Sockets;

namespace SMTP.Impostor.Hosts
{
    public interface ISMTPImpostorHost :
        IDisposable
    {
        SMTPImpostorHostSettings Settings { get; }
        SMTPImpostorHostStates State { get; }

        void Start();
        void Stop();

        IObservable<ISMTPImpostorEvent> Events { get; }
    }
}
