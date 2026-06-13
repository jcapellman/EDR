using EDR.Collector.lib.DynamicObjects.OutputFormatTypes;
using EDR.Collector.lib.DynamicObjects.StorageTypes;
using static EDR.Collector.lib.DynamicObjects.StorageTypes.LocalStorage;

using System.Text.Json;

namespace EDR.Collector.lib.Common
{
    /// <summary>
    /// Application-wide constants and default values.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Default configuration file name.
        /// </summary>
        public const string DEFAULT_CONFIG_FILENAME = "config.json";

        /// <summary>
        /// Default output format type.
        /// </summary>
        public const string DEFAULT_CONFIG_OUTPUTFORMAT = nameof(SysLog);

        /// <summary>
        /// Default storage type.
        /// </summary>
        public const string DEFAULT_CONFIG_STORAGETYPE = nameof(LocalStorage);

        /// <summary>
        /// Default storage configuration (LocalStorage with default path).
        /// </summary>
        public static readonly string DEFAULT_CONFIG_STORAGECONFIG = JsonSerializer.Serialize(new LocalStorageConfig());
    }
}