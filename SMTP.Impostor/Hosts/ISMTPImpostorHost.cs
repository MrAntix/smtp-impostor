using SMTP.Impostor.Events;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Sockets;
using System;

namespace SMTP.Impostor.Hosts
{
    public interface ISMTPImpostorHost :
        IDisposable
    {
        SMTPImpostorHostSettings Settings { get; }
        SMTPImpostorHostStatus State { get; }

        void Start();
        void Stop();

        IObservable<ISMTPImpostorEvent> Events { get; }
        ISMTPImpostorMessagesStore Messages { get; }
    }
}
