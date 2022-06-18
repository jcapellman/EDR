using Amazon.S3;
using Amazon.S3.Model;

using EDR.Collector.lib.DynamicObjects.StorageTypes.Base;

namespace EDR.Collector.lib.DynamicObjects.StorageTypes
{
    public class AWSS3Storage : BaseStorageType
    {
        public class AWSConfig
        {
            public string Region { get; set; }

            public string BucketName { get; set; }

            public string FilePath { get; set; }

            public string IAMUser { get; set; }

            public string IAMSecret { get; set; }

            public AWSConfig()
            {
                Region = Amazon.RegionEndpoint.USEast2.ToString();

                IAMUser = string.Empty;

                IAMSecret = string.Empty;

                BucketName = string.Empty;

                FilePath = string.Empty;
            }
        }

        private AWSConfig _config = new();

        public override string Name => "AWS S3";

        public override bool Initialize(string configStr)
        {
            var result = ParseJSON<AWSConfig>(configStr);

            if (result == null)
            {
                return false;
            }

            _config = result;

            return true;
        }

        public override async Task<bool> StoreEventAsync(string output)
        {
            var client = new AmazonS3Client(_config.IAMUser, _config.IAMSecret, Amazon.RegionEndpoint.USEast2);

            PutObjectRequest putRequest = new()
            {
                BucketName = _config.BucketName,
                Key = $"{Environment.MachineName}_{DateTime.Now.Date.ToShortDateString}",
                FilePath = _config.FilePath,
                ContentType = "text/plain"
            };

            PutObjectResponse response = await client.PutObjectAsync(putRequest);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}