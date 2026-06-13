using EDR.Collector.lib;

namespace EDR.Collector.UnitTest.lib
{
    [TestClass]
    public class CollectorRunnerTests
    {
        [Ignore("Until env can be replicated")]
        [TestMethod]
        public void EmptyConstructorTest()
        {
            try
            {
                using var runner = new CollectorRunner();

                runner.Start();

                Task.Delay(500).Wait();

                runner.Stop();

                Assert.Fail("Expected UnauthorizedAccessException was not thrown");
            }
            catch (UnauthorizedAccessException)
            {
                // Expected exception
            }
        }
    }
}