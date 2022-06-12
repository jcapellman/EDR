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
    }
}