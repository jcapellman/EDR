using EDR.Collector.lib.DynamicObjects.Base;

namespace EDR.Collector.lib.DynamicObjects.StorageTypes.Base
{
    public abstract class BaseStorageType : BaseDynamicObject
    {
        public abstract bool StoreEvent(string output);
    }
}