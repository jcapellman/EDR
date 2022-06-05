using Amazon.S3;
using Amazon.S3.Model;

using EDR.Collector.lib.DynamicObjects.StorageTypes.Base;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace EDR.Collector.lib.DynamicObjects.StorageTypes
{
    public class AWSS3Storage : BaseStorageType
    {
        public class AWSConfig
        {
            public Amazon.RegionEndpoint Region { get; set; }

            public string BucketName { get; set; }

            public string Key { get; set; }

            public string FilePath { get; set; }

            public AWSConfig()
            {
                Region = Amazon.RegionEndpoint.USEast1;

                BucketName = string.Empty;

                Key = string.Empty;

                FilePath = string.Empty;
            }

            [JsonConstructor]
            public AWSConfig(Amazon.RegionEndpoint region, string bucketName, string key, string filePath) => (Region, BucketName, Key, FilePath) = (region, bucketName, key, filePath);
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
            var client = new AmazonS3Client(_config.Region);

            PutObjectRequest putRequest = new()
            {
                BucketName = _config.BucketName,
                Key = _config.Key,
                FilePath = _config.FilePath,
                ContentType = "text/plain"
            };

            PutObjectResponse response = await client.PutObjectAsync(putRequest);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}