using EDR.Collector.lib;

namespace EDR.Collector.UnitTest.lib
{
    [TestClass]
    public class CollectorRunnerTests
    {
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void EmptyConstructorTest()
        {
            var runner = new CollectorRunner();

            runner.Start();

            Task.Delay(500);

            runner.Stop();
        }
    }
}