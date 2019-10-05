using System;
using SMTP.Impostor.Events;

namespace SMTP.Impostor.Hosts
{
    public interface ISMTPImpostorHost :
        IDisposable
    {
        void Configure(SMTPImpostorHostSettings hostSettings);
        void Start();
        void Stop();

        IObservable<ISMTPImpostorEvent> Events { get; }
    }
}
