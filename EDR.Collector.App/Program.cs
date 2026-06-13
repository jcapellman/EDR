using EDR.Collector.lib;

namespace EDR.Collector.App
{
    internal static class Program
    {
        static void Main()
        {
            using var mreShutdown = new ManualResetEvent(false);
            using var collector = new CollectorRunner();

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                mreShutdown.Set();
            };

            Console.WriteLine("EDR Collector starting...");

            collector.Start();

            Console.WriteLine("EDR Collector running... (Control + C to stop)");

            mreShutdown.WaitOne();

            Console.WriteLine("EDR Collector stopping...");

            collector.Stop();

            Console.WriteLine("EDR Collector stopped.");
        }
    }
}