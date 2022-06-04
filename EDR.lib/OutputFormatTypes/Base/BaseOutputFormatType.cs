using WET.lib.Containers;

namespace EDR.Collector.lib.OutputTypes.Base
{
    public abstract class BaseOutputFormatType
    {
        public abstract string Name { get; }

        public abstract string Format(ETWEventContainerItem item);
    }
}