using EDR.Collector.lib.DynamicObjects.OutputFormatTypes;
using EDR.Collector.lib.DynamicObjects.StorageTypes;

namespace EDR.Collector.lib.Common
{
    public static class Constants
    {
        public const string DEFAULT_CONFIG_FILENAME = "config.json";

        public const string DEFAULT_CONFIG_OUTPUTFORMAT = nameof(SysLog);

        public const string DEFAULT_CONFIG_STORAGETYPE = nameof(AWSS3Storage);
    }
}