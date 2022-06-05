using EDR.Collector.lib.DynamicObjects.OutputFormatTypes.Base;
using WET.lib.Containers;

namespace EDR.Collector.lib.DynamicObjects.OutputFormatTypes
{
    public class SysLog : BaseOutputFormatType
    {
        public override string Name => "SysLog";

        public override string Format(ETWEventContainerItem item) => $"<1>{item.Timestamp.ToString("MMM dd H:m:ss")} {item.hostname} {item.MonitorType}:{item.Payload}";
    }
}