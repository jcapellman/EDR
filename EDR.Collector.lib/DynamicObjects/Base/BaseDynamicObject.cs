namespace EDR.Collector.lib.DynamicObjects.Base
{
    /// <summary>
    /// Base class for all dynamically loadable objects.
    /// </summary>
    public abstract class BaseDynamicObject
    {
        /// <summary>
        /// Gets the name of this dynamic object.
        /// </summary>
        public abstract string Name { get; }
    }
}