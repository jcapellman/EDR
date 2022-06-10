using EDR.Collector.lib.DynamicObjects.StorageTypes;

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
    }
}