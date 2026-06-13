using EDR.Collector.lib.Common;

namespace EDR.Collector.lib.Objects
{
    /// <summary>
    /// Configuration object for EDR collector.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Type of storage backend to use (e.g., LocalStorage, AWSS3Storage).
        /// </summary>
        public string StorageType { get; set; }

        /// <summary>
        /// JSON configuration string for the storage backend.
        /// </summary>
        public string StorageTypeConfig { get; set; }

        /// <summary>
        /// Enable output to console (stdout).
        /// </summary>
        public bool EnableStdOutToConsole { get; set; }

        /// <summary>
        /// Output format type (e.g., SysLog).
        /// </summary>
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