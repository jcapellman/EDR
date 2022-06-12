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
    }
}