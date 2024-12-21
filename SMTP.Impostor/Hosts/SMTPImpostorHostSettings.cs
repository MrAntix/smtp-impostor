using SMTP.Impostor.Messages;
using System;
using System.Collections.Immutable;

namespace SMTP.Impostor.Hosts
{
    [Serializable]
    public record SMTPImpostorHostSettings :
        IEquatable<SMTPImpostorHostSettings>
    {
        public static ImmutableList<SMTPImpostorHostSettings> Default = [new()];

        public Guid Id { get; init; } = Guid.NewGuid();
        public string IP { get; init; } = "127.0.0.1";
        public int Port { get; init; } = 25;
        public string StoreType { get; init; }
        public string Name { get; init; }
        public bool Start { get; init; }
        public SMTPImpostorMessagesStoreSettings Store { get; init; } = SMTPImpostorMessagesStoreSettings.Default;

        public override string ToString()
        {
            return Name ?? $"{IP}:{Port}";
        }

        bool IEquatable<SMTPImpostorHostSettings>
            .Equals(SMTPImpostorHostSettings other)
        {
            return other != null
                && string.Equals(IP, other.IP, StringComparison.InvariantCultureIgnoreCase)
                && Port == other.Port;
        }
    }
}
