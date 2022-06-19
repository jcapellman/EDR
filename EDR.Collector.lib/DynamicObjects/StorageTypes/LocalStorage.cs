using EDR.Collector.lib.DynamicObjects.StorageTypes.Base;
using System.Text.Json;

namespace EDR.Collector.lib.DynamicObjects.StorageTypes
{
    public class LocalStorage : BaseStorageType
    {
        public class LocalStorageConfig
        {
            public string FilePath { get; set; }

            public LocalStorageConfig()
            {
                FilePath = AppContext.BaseDirectory;
            }
        }

        private LocalStorageConfig _config = new();

        public override string Name => "LocalStorage";

        static readonly ReaderWriterLock locker = new();

        private string CurrentFileName => Path.Combine(_config.FilePath, $"{Environment.MachineName.ToLower()}_{DateTime.Today:MM_dd_yyyy}.log");

        public override bool Initialize(string configStr)
        {
            if (string.IsNullOrWhiteSpace(configStr))
            {
                logger.Debug("LocalStorage::Initialize - configStr parameter was null");

                _config = new LocalStorageConfig();

                ValidateDirectoryPath(_config.FilePath);

                return true;
            }

            try
            {
                logger.Debug($"LocalStorage::Initialize - configStr parameter ({configStr})");

                var result = ParseJSON<LocalStorageConfig>(configStr);

                if (result == null)
                {
                    logger.Debug("LocalStorage::Initialize - JSON Parsing of configStr was null");

                    return false;
                }

                _config = result;

                ValidateDirectoryPath(_config.FilePath);

                return true;
            } catch (JsonException jex)
            {
                logger.Error($"LocalStorage::Initialize: Failed to parse {configStr} with exception: {jex}");

                return false;
            }
        }

        public override Task<bool> StoreEventAsync(string output)
        {
            lock (locker)
            {
                logger.Debug($"LocalStorage::StoreEventAsync - {output}");

                File.AppendAllText(CurrentFileName, output + System.Environment.NewLine);
            }

            return Task.FromResult(true);
        }
    }
}
