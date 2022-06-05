using EDR.Collector.lib.Common;
using EDR.Collector.lib.Objects;

using System.Text.Json;

namespace EDR.Collector.lib.Helpers
{
    public static class ConfigParser
    {
        private static Config CreateDefaultConfigFile(string fileName = Constants.DEFAULT_CONFIG_FILENAME)
        {
            var config = new Config();

            var json = JsonSerializer.Serialize(config);

            File.WriteAllText(fileName, json);

            return config;
        }

        private static Config LoadConfigFile(string fileName)
        {
            var json = File.ReadAllText(fileName);

            if (string.IsNullOrEmpty(json))
            {
                return CreateDefaultConfigFile(fileName);
            }

            var config = JsonSerializer.Deserialize<Config>(json);

            if (config == null)
            {
                config = CreateDefaultConfigFile(fileName);
            }

            return config;
        }

        public static Config LoadConfig(string? fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Constants.DEFAULT_CONFIG_FILENAME;
            }

            if (!File.Exists(fileName))
            {
                return CreateDefaultConfigFile(fileName);
            }

            return LoadConfigFile(fileName);
        }
    }
}