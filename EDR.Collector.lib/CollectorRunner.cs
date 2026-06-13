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
    /// <summary>
    /// Main collector runner that orchestrates ETW event monitoring and storage.
    /// </summary>
    public class CollectorRunner : IDisposable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Config _config;
        private readonly ETWMonitor _wet = new();

        private readonly BaseOutputFormatType _outputFormatter;
        private readonly BaseStorageType _storage;

        private bool _disposed;

        /// <summary>
        /// Create a new collector runner with default configuration file.
        /// </summary>
        /// <param name="configFileName">Configuration file name (default: config.json)</param>
        public CollectorRunner(string configFileName = Constants.DEFAULT_CONFIG_FILENAME) : this(ConfigParser.LoadConfig(configFileName))
        {
        }

        /// <summary>
        /// Create a new collector runner with specific configuration.
        /// </summary>
        /// <param name="config">Configuration object</param>
        public CollectorRunner(Config config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

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

        /// <summary>
        /// Start collecting ETW events.
        /// </summary>
        public void Start()
        {
            _wet.OnEvent += Wet_OnEvent;

            _wet.Start();

            logger.Info("CollectorRunner started successfully");
        }

        /// <summary>
        /// Stop collecting ETW events.
        /// </summary>
        public void Stop()
        {
            _wet.OnEvent -= Wet_OnEvent;

            _wet.Stop();

            logger.Info("CollectorRunner stopped successfully");
        }

        private async void Wet_OnEvent(object? sender, WET.lib.Containers.ETWEventContainerItem e)
        {
            try
            {
                var outputStr = _outputFormatter.Format(e);

                if (_config.EnableStdOutToConsole)
                {
                    Console.WriteLine(outputStr);
                }

                logger.Debug($"Wet_OnEvent: {outputStr}");

                await _storage.StoreEventAsync(outputStr);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error processing ETW event: {ex.Message}");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Stop();
                    _storage?.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}