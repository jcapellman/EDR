using EDR.Collector.lib.DynamicObjects.Base;

using WET.lib.Containers;

namespace EDR.Collector.lib.DynamicObjects.OutputFormatTypes.Base
{
    public abstract class BaseOutputFormatType : BaseDynamicObject
    {
        public abstract string Format(ETWEventContainerItem item);
    }
}