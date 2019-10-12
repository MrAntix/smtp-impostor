using System;
using System.Collections.Immutable;

namespace SMTP.Impostor.Hosts
{
    [Serializable]
    public class SMTPImpostorHostSettings :
        IEquatable<SMTPImpostorHostSettings>
    {
        public SMTPImpostorHostSettings(
            string ip, int port = 25,
            string name = null)
        {
            IP = ip ?? throw new ArgumentNullException(nameof(ip));
            Port = port;
            Name = name ?? $"{ip}:{port}";
        }

        public string IP { get; }
        public int Port { get; }
        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }

        bool IEquatable<SMTPImpostorHostSettings>
            .Equals(SMTPImpostorHostSettings other)
        {
            return other != null
                && string.Equals(IP, other.IP, StringComparison.InvariantCultureIgnoreCase)
                && Port == other.Port;
        }

        public static IImmutableList<SMTPImpostorHostSettings> Default
            = new[]{
                new SMTPImpostorHostSettings(
                        ip: "127.0.0.1",
                        port: 25)
                }.ToImmutableList();
    }
}
