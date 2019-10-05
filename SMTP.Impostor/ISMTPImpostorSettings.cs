using System.Collections.Immutable;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor
{
    public interface ISMTPImpostorSettings
    {
        IImmutableList<SMTPImpostorHostSettings> Hosts { get; }
    }
}
