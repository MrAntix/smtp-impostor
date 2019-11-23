using SMTP.Impostor.Messages;
using System;
using System.Collections.Immutable;

namespace SMTP.Impostor.Hosts
{
    [Serializable]
    public class SMTPImpostorHostSettings :
        IEquatable<SMTPImpostorHostSettings>
    {
        public SMTPImpostorHostSettings(
            Guid? id = null,
            string ip = null, int? port = 25,
            string name = null,
            string storeType = null,
            bool start = false,
            SMTPImpostorMessagesStoreSettings store = null)
        {
            Id = id ?? Guid.NewGuid();
            IP = ip ?? "127.0.0.1";
            Port = port ?? 25;
            StoreType = storeType;
            Name = string.IsNullOrWhiteSpace(name) ? $"{IP}:{Port}" : name;
            Start = start;
            Store = store ?? SMTPImpostorMessagesStoreSettings.Default;
        }

        public Guid Id { get; }
        public string IP { get; }
        public int Port { get; }
        public string StoreType { get; }
        public string Name { get; }
        public bool Start { get; }
        public SMTPImpostorMessagesStoreSettings Store { get; }

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
