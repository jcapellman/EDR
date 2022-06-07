using EDR.Collector.lib.DynamicObjects.OutputFormatTypes;
using EDR.Collector.lib.DynamicObjects.StorageTypes;
using static EDR.Collector.lib.DynamicObjects.StorageTypes.LocalStorage;

using System.Text.Json;

namespace EDR.Collector.lib.Common
{
    public static class Constants
    {
        public const string DEFAULT_CONFIG_FILENAME = "config.json";

        public const string DEFAULT_CONFIG_OUTPUTFORMAT = nameof(SysLog);

        public const string DEFAULT_CONFIG_STORAGETYPE = nameof(LocalStorage);

        public static string DEFAULT_CONFIG_STORAGECONFIG = JsonSerializer.Serialize(new LocalStorageConfig());
    }
}