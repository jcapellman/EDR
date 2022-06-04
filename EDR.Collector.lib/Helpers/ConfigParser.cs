using EDR.Collector.lib.Objects;

using System.Text.Json;

namespace EDR.Collector.lib.Helpers
{
    public class ConfigParser
    {
        public static Config LoadConfig(string? fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return new Config();
            }

            if (!File.Exists(fileName))
            {
                return new Config();
            }

            var json = File.ReadAllText(fileName);

            if (string.IsNullOrEmpty(json))
            {
                return new Config();
            }

            var config = JsonSerializer.Deserialize<Config>(json);

            if (config == null)
            {
                config = new Config();
            }

            return config;
        }
    }
}