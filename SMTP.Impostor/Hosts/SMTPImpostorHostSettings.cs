using System;
using System.Collections.Immutable;

namespace SMTP.Impostor.Hosts
{
    [Serializable]
    public class SMTPImpostorHostSettings :
        IEquatable<SMTPImpostorHostSettings>
    {
        public SMTPImpostorHostSettings(
            string ip = null, int? port = 25,
            string name = null,
            bool start = false)
        {
            IP = ip ?? "127.0.0.1";
            Port = port ?? 25;
            Name = string.IsNullOrWhiteSpace(name) ? $"{IP}:{Port}" : name;
            Start = start;
        }

        public string IP { get; }
        public int Port { get; }
        public string Name { get; }
        public bool Start { get; }

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
                new SMTPImpostorHostSettings()
                }.ToImmutableList();
    }
}
