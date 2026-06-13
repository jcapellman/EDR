using EDR.Collector.lib.DynamicObjects.StorageTypes.Base;
using System.Text.Json;

namespace EDR.Collector.lib.DynamicObjects.StorageTypes
{
    /// <summary>
    /// Local file system storage implementation for EDR events.
    /// Stores events in daily log files named by machine and date.
    /// </summary>
    public class LocalStorage : BaseStorageType
    {
        /// <summary>
        /// Configuration for local storage.
        /// </summary>
        public class LocalStorageConfig
        {
            /// <summary>
            /// Directory path where log files will be stored.
            /// </summary>
            public string FilePath { get; set; }

            public LocalStorageConfig()
            {
                FilePath = AppContext.BaseDirectory;
            }
        }

        private LocalStorageConfig _config = new();
        private readonly ReaderWriterLockSlim _locker = new();

        public override string Name => "LocalStorage";

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
            }
            catch (JsonException jex)
            {
                logger.Error($"LocalStorage::Initialize: Failed to parse {configStr} with exception: {jex}");

                return false;
            }
        }

        public override Task<bool> StoreEventAsync(string output)
        {
            _locker.EnterWriteLock();
            try
            {
                logger.Debug($"LocalStorage::StoreEventAsync - {output}");

                File.AppendAllText(CurrentFileName, output + Environment.NewLine);

                return Task.FromResult(true);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _locker?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
