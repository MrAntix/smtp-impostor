using SMTP.Impostor.Hosts;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace SMTP.Impostor
{
    public interface ISMTPImpostorHostSettingsStore
    {
        Task<IImmutableList<SMTPImpostorHostSettings>> LoadAsync();
        Task SaveAsync(IEnumerable<SMTPImpostorHostSettings> value);
    }
}
