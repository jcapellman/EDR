using EDR.Collector.lib.DynamicObjects.Base;

namespace EDR.Collector.lib.Helpers
{
    public static class DynamicLoader
    {
        public static T? InitializeGeneric<T>(string objectName) where T : BaseDynamicObject
        {
            var type = typeof(DynamicLoader).Assembly.GetTypes().FirstOrDefault(a => a == typeof(T) && a.Name == objectName && !a.IsAbstract);

            if (type == null)
            {
                return null;
            }

            return (T?)Activator.CreateInstance(type);
        }
    }
}