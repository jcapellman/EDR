using EDR.Collector.lib;
using EDR.Collector.lib.Helpers;

namespace EDR.Collector.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ManualResetEvent mreShutdown = new(false);

            var collector = new CollectorRunner(ConfigParser.LoadConfig(args.Length == 0 ? null : args[0]));

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