using Microsoft.Extensions.Logging;
using SMTP.Impostor.Hosts;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
            FilePath = Path.Combine(Path.GetTempPath(), "Impostor", "host.json");
        }

        public string FilePath { get; private set; }

        async Task<IImmutableList<SMTPImpostorHostSettings>> ISMTPImpostorHostSettingsStore.LoadAsync()
        {   
            var json = File.Exists(FilePath)
                ? await File.ReadAllTextAsync(FilePath)
                : null;

            return json == null
                ? SMTPImpostorHostSettings.Default
                : _serialization.Deserialize<ImmutableList<SMTPImpostorHostSettings>>(json);
        }

        static CancellationTokenSource _cancel;
        async Task ISMTPImpostorHostSettingsStore
            .SaveAsync(IEnumerable<SMTPImpostorHostSettings> value)
        {
            var json = _serialization.Serialize(value);

            _cancel?.Cancel();
            _cancel = new CancellationTokenSource();

            try
            {
                await Task.Delay(500, _cancel.Token);
                await Delegates.RetryAsync(
                    async () => await File.WriteAllTextAsync(FilePath, json),
                    _cancel.Token,
                    _logger);
            }
            catch (TaskCanceledException) { }
        }
    }
}
