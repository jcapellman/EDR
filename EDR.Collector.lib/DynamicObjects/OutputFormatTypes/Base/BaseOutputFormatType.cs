using EDR.Collector.lib.DynamicObjects.Base;

using WET.lib.Containers;

namespace EDR.Collector.lib.DynamicObjects.OutputFormatTypes.Base
{
    /// <summary>
    /// Base class for all output format implementations.
    /// </summary>
    public abstract class BaseOutputFormatType : BaseDynamicObject
    {
        /// <summary>
        /// Format an ETW event into a string representation.
        /// </summary>
        /// <param name="item">The ETW event to format.</param>
        /// <returns>Formatted event string.</returns>
        public abstract string Format(ETWEventContainerItem item);
    }
}