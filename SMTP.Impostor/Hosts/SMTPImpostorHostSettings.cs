using System;

namespace SMTP.Impostor.Hosts
{
    [Serializable]
    public class SMTPImpostorHostSettings :
        IEquatable<SMTPImpostorHostSettings>
    {
        public SMTPImpostorHostSettings(
            string hostName, int port = 25,
            string friendlyName = null)
        {
            HostName = hostName ?? throw new ArgumentNullException(nameof(hostName));
            Port = port;
            FriendlyName = friendlyName ?? $"{hostName}:{port}";
        }

        public string HostName { get; }
        public int Port { get; }
        public string FriendlyName { get; }

        public override string ToString()
        {
            return FriendlyName;
        }

        bool IEquatable<SMTPImpostorHostSettings>
            .Equals(SMTPImpostorHostSettings other)
        {
            return other != null
                && string.Equals(HostName, other.HostName, StringComparison.InvariantCultureIgnoreCase)
                && Port == other.Port;
        }
    }
}
