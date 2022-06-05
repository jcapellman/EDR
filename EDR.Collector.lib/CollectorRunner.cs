using EDR.Collector.lib.Common;
using EDR.Collector.lib.Helpers;
using EDR.Collector.lib.Objects;
using EDR.Collector.lib.OutputFormatTypes;
using EDR.Collector.lib.OutputTypes.Base;
using EDR.Collector.lib.StorageTypes;
using EDR.Collector.lib.StorageTypes.Base;

using WET.lib;

namespace EDR.Collector.lib
{
    public class CollectorRunner
    {
        private readonly Config _config;
        private readonly ETWMonitor _wet = new();

        private readonly BaseOutputFormatType _outputFormatter;
        private readonly BaseStorageType _storage;

        public CollectorRunner(string configFileName = Constants.DEFAULT_CONFIG_FILENAME) : this(ConfigParser.LoadConfig(configFileName))
        {
        }

        public CollectorRunner(Config config)
        {
            _config = config;

            _outputFormatter = InitializeOutputFormat(_config.OutputFormat);
            _storage = InitializeStorageType(_config.StorageType, _config.StorageTypeConfig);
        }

        private BaseOutputFormatType InitializeOutputFormat(string formatType)
        {
            // TODO: Use reflection to load output formatters and return
            return new SysLog();
        }

        private BaseStorageType InitializeStorageType(string storageType, string storageTypeConfig)
        {
            // TODO: Use reflection to load storage types and return
            return new AWSS3Storage();
        }

        public void Start()
        {
            _wet.OnEvent += Wet_OnEvent;

            _wet.Start();
        }

        public void Stop()
        {
            _wet.OnEvent -= Wet_OnEvent;

            _wet.Stop();
        }

        private void Wet_OnEvent(object? sender, WET.lib.Containers.ETWEventContainerItem e)
        {
            var outputStr = _outputFormatter.Format(e);

            if (_config.EnableStdOutToConsole)
            {
                Console.WriteLine(outputStr);
            }

            _storage.StoreEvent(outputStr);
        }
    }
}