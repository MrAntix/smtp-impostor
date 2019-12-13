using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Stores.FileSystem.HostSettings
{
    public class SMTPImpostorFileSystemHostSettingsStore : ISMTPImpostorHostSettingsStore
    {
        readonly SMTPImpostorSerialization _serialization;
        readonly ILogger<SMTPImpostorFileSystemHostSettingsStore> _logger;

        public SMTPImpostorFileSystemHostSettingsStore(
            SMTPImpostorSerialization serialization,
            ILogger<SMTPImpostorFileSystemHostSettingsStore> logger)
        {
            _serialization = serialization;
            _logger = logger;
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

        CancellationTokenSource _cancel;
        async Task ISMTPImpostorHostSettingsStore
            .SaveAsync(IEnumerable<SMTPImpostorHostSettings> value)
        {
            var json = _serialization.Serialize(value);

            if (_cancel != null) _cancel.Cancel();
            _cancel = new CancellationTokenSource();

            try
            {
                await Task.Delay(10000, _cancel.Token);
                await Delegates.RetryAsync(
                    async () => await File.WriteAllTextAsync(FilePath, json),
                    _logger);
            }
            catch (TaskCanceledException) { }

            _cancel = null;
        }
    }
}
