namespace EDR.Collector.lib.StorageTypes.Base
{
    public abstract class BaseStorageType
    {
        public abstract string Name { get; }

        public abstract bool StoreEvent(string output);
    }
}