using EDR.Collector.lib.Common;

namespace EDR.Collector.lib.Objects
{
    public class Config
    {
        public string StorageType { get; set; }

        public string StorageTypeConfig { get; set; }

        public bool EnableStdOutToConsole { get; set; }

        public string OutputFormat { get; set; }

        public Config()
        {
            EnableStdOutToConsole = true;

            StorageType = Constants.DEFAULT_CONFIG_STORAGETYPE;

            OutputFormat = Constants.DEFAULT_CONFIG_OUTPUTFORMAT;

            StorageTypeConfig = Constants.DEFAULT_CONFIG_STORAGECONFIG;
        }
    }
}