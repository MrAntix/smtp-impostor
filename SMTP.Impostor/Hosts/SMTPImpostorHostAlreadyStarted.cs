using System;
using System.Runtime.Serialization;

namespace SMTP.Impostor.Hosts
{
    [Serializable]
    public class SMTPImpostorHostAlreadyStarted : Exception
    {
        public SMTPImpostorHostAlreadyStarted(
            SMTPImpostorHostSettings hostSettings)
            : base($"{hostSettings.Name} was already started")
        {
            HostSettings = hostSettings;
        }

        protected SMTPImpostorHostAlreadyStarted(
            SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public SMTPImpostorHostSettings HostSettings { get; }
    }
}
