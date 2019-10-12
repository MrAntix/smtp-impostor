using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Stores.FileSystem.HostSettings
{
    public class SMTPImpostorFileSystemHostSettingsStore : ISMTPImpostorHostSettingsStore
    {
        readonly SMTPImpostorSerialization _serialization;

        public SMTPImpostorFileSystemHostSettingsStore(
            SMTPImpostorSerialization serialization)
        {
            _serialization = serialization;
            FilePath = Path.Combine(Path.GetTempPath(), "Impostor", "settings.json");
        }

        public string FilePath { get; private set; }

        async Task<IImmutableList<SMTPImpostorHostSettings>> ISMTPImpostorHostSettingsStore.LoadAsync()
        {
            var json = File.Exists(FilePath)
                ? await File.ReadAllTextAsync(FilePath)
                : null;

            return json == null ? SMTPImpostorHostSettings.Default
                : _serialization.Deserialize<IImmutableList<SMTPImpostorHostSettings>>(json);
        }

        async Task ISMTPImpostorHostSettingsStore.SaveAsync(IEnumerable<SMTPImpostorHostSettings> value)
        {
            var json = _serialization.Serialize(value);

            await File.WriteAllTextAsync(FilePath, json);
        }
    }
}
