using EDR.Collector.lib.DynamicObjects.Base;

using System.Text.Json;

namespace EDR.Collector.lib.DynamicObjects.StorageTypes.Base
{
    public abstract class BaseStorageType : BaseDynamicObject
    {
        public abstract bool Initialize(string configStr);

        public abstract Task<bool> StoreEventAsync(string output);

        protected static T? ParseJSON<T>(string jsonStr) => JsonSerializer.Deserialize<T>(jsonStr);
    }
}