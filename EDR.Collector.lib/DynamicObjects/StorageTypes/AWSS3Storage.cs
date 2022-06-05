using EDR.Collector.lib.DynamicObjects.StorageTypes.Base;

namespace EDR.Collector.lib.DynamicObjects.StorageTypes
{
    public class AWSS3Storage : BaseStorageType
    {
        public override string Name => "AWS S3";

        public override bool StoreEvent(string output)
        {
            //TODO: Actually store in an S3 bucket

            return true;
        }
    }
}