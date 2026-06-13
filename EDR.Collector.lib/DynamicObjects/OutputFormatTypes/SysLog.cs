using EDR.Collector.lib.DynamicObjects.OutputFormatTypes.Base;

using WET.lib.Containers;

namespace EDR.Collector.lib.DynamicObjects.OutputFormatTypes
{
    /// <summary>
    /// SysLog format implementation for EDR events.
    /// Formats events according to SysLog RFC 3164 style.
    /// </summary>
    public class SysLog : BaseOutputFormatType
    {
        public override string Name => "SysLog";

        public override string Format(ETWEventContainerItem item) => $"<1>{item.Timestamp:MMM dd H:m:ss} {item.hostname} {item.MonitorType}:{item.Payload}";
    }
}