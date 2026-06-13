using EDR.Collector.lib.DynamicObjects.Base;

namespace EDR.Collector.lib.Helpers
{
    /// <summary>
    /// Helper class for dynamically loading plugin types at runtime.
    /// </summary>
    public static class DynamicLoader
    {
        /// <summary>
        /// Initialize a generic dynamic object by name.
        /// </summary>
        /// <typeparam name="T">Type of dynamic object to create.</typeparam>
        /// <param name="objectName">Name of the object class to instantiate.</param>
        /// <returns>Instance of the dynamic object, or null if not found.</returns>
        public static T? InitializeGeneric<T>(string objectName) where T : BaseDynamicObject
        {
            var type = typeof(DynamicLoader).Assembly.GetTypes().FirstOrDefault(a => a.BaseType == typeof(T) && a.Name == objectName && !a.IsAbstract);

            if (type == null)
            {
                return null;
            }

            return (T?)Activator.CreateInstance(type);
        }
    }
}