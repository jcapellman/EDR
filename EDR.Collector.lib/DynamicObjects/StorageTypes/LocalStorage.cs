using EDR.Collector.lib.DynamicObjects.StorageTypes.Base;

namespace EDR.Collector.lib.DynamicObjects.StorageTypes
{
    internal class LocalStorage : BaseStorageType
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

        private string CurrentFileName => Path.Combine(_config.FilePath, $"{DateTime.Today.ToString("MM_dd_yyyy")}.log");

        public override bool Initialize(string configStr)
        {
            if (string.IsNullOrWhiteSpace(configStr))
            {
                _config = new LocalStorageConfig();

                return true;
            }

            var result = ParseJSON<LocalStorageConfig>(configStr);

            if (result == null)
            {
                return false;
            }

            _config = result;

            return true;
        }

        public override Task<bool> StoreEventAsync(string output)
        {
            lock (locker)
            {
                File.AppendAllText(CurrentFileName, output + System.Environment.NewLine);
            }

            return Task.FromResult(true);
        }
    }
}
