using EDR.Collector.lib.Common;
using EDR.Collector.lib.DynamicObjects.OutputFormatTypes;
using EDR.Collector.lib.DynamicObjects.OutputFormatTypes.Base;
using EDR.Collector.lib.DynamicObjects.StorageTypes;
using EDR.Collector.lib.DynamicObjects.StorageTypes.Base;
using EDR.Collector.lib.Helpers;
using EDR.Collector.lib.Objects;

using WET.lib;

namespace EDR.Collector.lib
{
    public class CollectorRunner
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

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

            logger.Debug($"CollectorRunner: {_outputFormatter.Name} was initialized successfully");

            _storage = InitializeStorageType(_config.StorageType, _config.StorageTypeConfig);

            logger.Debug($"CollectorRunner: {_storage.Name} was initialized successfully");
        }

        private static BaseOutputFormatType InitializeOutputFormat(string formatType) =>
            DynamicLoader.InitializeGeneric<BaseOutputFormatType>(formatType) ?? new SysLog();

        private static BaseStorageType InitializeStorageType(string storageType, string storageTypeConfig)
        {
            var storage = DynamicLoader.InitializeGeneric<BaseStorageType>(storageType) ?? new AWSS3Storage();

            var initializeResult = storage.Initialize(storageTypeConfig);

            if (!initializeResult)
            {
                throw new TypeInitializationException(storageType, new Exception($"Failed to initialize {storageType} - failing to start"));
            }

            return storage;
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

        private async void Wet_OnEvent(object? sender, WET.lib.Containers.ETWEventContainerItem e)
        {
            var outputStr = _outputFormatter.Format(e);

            if (_config.EnableStdOutToConsole)
            {
                Console.WriteLine(outputStr);
            }

            logger.Debug($"Wet_OnEvent: {outputStr}");

            await _storage.StoreEventAsync(outputStr);
        }
    }
}