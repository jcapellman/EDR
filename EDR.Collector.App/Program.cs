using EDR.Collector.lib;

namespace EDR.Collector.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ManualResetEvent mreShutdown = new(false);

            var collector = new CollectorRunner();

            Console.CancelKeyPress += (sender, eventArgs) => {
                eventArgs.Cancel = true;
                mreShutdown.Set();
            };

            collector.Start();

            mreShutdown.WaitOne();

            collector.Stop();
        }
    }
}