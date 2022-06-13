using EDR.Collector.lib.DynamicObjects.StorageTypes;
using static EDR.Collector.lib.DynamicObjects.StorageTypes.LocalStorage;

using System.Text.Json;

namespace EDR.Collector.UnitTest.lib.StorageTypes
{
    [TestClass]
    public class LocalStorageTests
    {
        [TestMethod]
        public void EmptyCheck()
        {
            var localStorage = new LocalStorage();

            var result = localStorage.Initialize("");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GarbageInputCheck()
        {
            var localStorage = new LocalStorage();

            var result = localStorage.Initialize("BabyYaga");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidInputButEmptyFilenameCheck()
        {
            var localStorage = new LocalStorage();

            var result = localStorage.Initialize(JsonSerializer.Serialize(new LocalStorageConfig()));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidInputAndExistingFile()
        {
            var localStorage = new LocalStorage();

            var config = new LocalStorageConfig
            {
                FilePath = Path.Combine(AppContext.BaseDirectory, "config.json")
            };

            var json = JsonSerializer.Serialize(config);

            File.WriteAllText(config.FilePath, json);

            var result = localStorage.Initialize(json);

            Assert.IsTrue(result);

            File.Delete(config.FilePath);
        }

        [TestMethod]
        public void ValidInputAndNonExistingFile()
        {
            var localStorage = new LocalStorage();

            var config = new LocalStorageConfig
            {
                FilePath = Path.Combine(AppContext.BaseDirectory, "config.json")
            };

            var json = JsonSerializer.Serialize(config);

            var result = localStorage.Initialize(json);

            Assert.IsTrue(result);
        }
    }
}