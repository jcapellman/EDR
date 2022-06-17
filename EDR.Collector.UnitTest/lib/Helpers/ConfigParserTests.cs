using EDR.Collector.lib.DynamicObjects.StorageTypes;
using EDR.Collector.lib.Objects;
using System.Text.Json;
using static EDR.Collector.lib.DynamicObjects.StorageTypes.LocalStorage;

namespace EDR.Collector.UnitTest.lib.Helpers
{
    [TestClass]
    public class ConfigParserTests
    {
        [TestMethod]
        public void EmptyCheck()
        {
            var result = EDR.Collector.lib.Helpers.ConfigParser.LoadConfig(string.Empty);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InitializedLocalStorage()
        {
            var localStorage = new LocalStorage();

            var localStorageConfig = new LocalStorageConfig
            {
                FilePath = AppContext.BaseDirectory               
            };

            var json = JsonSerializer.Serialize(localStorageConfig);

            var config = new Config();

            config.StorageType = "LocalStorage";
            config.StorageTypeConfig = json;
            config.OutputFormat = "syslog";

            var filePath = Path.Combine(AppContext.BaseDirectory, "config.json");

            File.WriteAllText(filePath, JsonSerializer.Serialize(config));

            var result = EDR.Collector.lib.Helpers.ConfigParser.LoadConfig(filePath);

            Assert.IsNotNull(result);
        }
    }
}