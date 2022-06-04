using EDR.Collector.lib.OutputTypes.Base;

using WET.lib.Containers;

namespace EDR.Collector.lib.OutputFormatTypes
{
    public class SysLog : BaseOutputFormatType
    {
        public override string Name => "SysLog";

        public override string Format(ETWEventContainerItem item) => $"<1>{item.Timestamp.ToString("MMM dd H:m:ss")} {item.hostname} {item.MonitorType}:{item.Payload}";
    }
}