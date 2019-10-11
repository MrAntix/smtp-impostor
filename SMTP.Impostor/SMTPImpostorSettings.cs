using System.Collections.Immutable;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor
{
    public class SMTPImpostorSettings : ISMTPImpostorSettings
    {
        public IImmutableList<SMTPImpostorHostSettings> Hosts { get; set; }
            = ImmutableList<SMTPImpostorHostSettings>.Empty;

        public static ISMTPImpostorSettings Default = new SMTPImpostorSettings
        {
            Hosts = new[]{
                new SMTPImpostorHostSettings(
                        ip: "127.0.0.1",
                        port: 25)
                }.ToImmutableList()
        };
    }
}
