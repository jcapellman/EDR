using EDR.Collector.lib.DynamicObjects.StorageTypes.Base;

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

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private LocalStorageConfig _config = new();

        public override string Name => "LocalStorage";

        static readonly ReaderWriterLock locker = new();

        private string CurrentFileName => Path.Combine(_config.FilePath, $"{DateTime.Today.ToString("MM_dd_yyyy")}.log");

        public override bool Initialize(string configStr)
        {
            if (string.IsNullOrWhiteSpace(configStr))
            {
                logger.Debug("LocalStorage::Initialize - configStr parameter was null");

                _config = new LocalStorageConfig();

                return true;
            }

            logger.Debug($"LocalStorage::Initialize - configStr parameter ({configStr})");

            var result = ParseJSON<LocalStorageConfig>(configStr);

            if (result == null)
            {
                logger.Debug("LocalStorage::Initialize - JSON Parsing of configStr was null");

                return false;
            }

            _config = result;

            return true;
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
