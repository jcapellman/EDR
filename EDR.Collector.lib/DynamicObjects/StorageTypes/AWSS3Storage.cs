using Amazon.S3;
using Amazon.S3.Model;

using EDR.Collector.lib.DynamicObjects.StorageTypes.Base;
using System.Text.Json;
using static EDR.Collector.lib.DynamicObjects.StorageTypes.LocalStorage;

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

        private readonly LocalStorage _localStorage = new();

        private readonly string AWSLogPath = Path.Combine(AppContext.BaseDirectory, "AWSLogs");

        private readonly string AWSLogArchivePath = Path.Combine(AppContext.BaseDirectory, "AWSLogArchive");

        public override string Name => "AWS S3";

        private readonly System.Timers.Timer _timerUpload = new(60000);

        public override bool Initialize(string configStr)
        {
            var result = ParseJSON<AWSConfig>(configStr);

            if (result == null)
            {
                return false;
            }

            _config = result;

            _localStorage.Initialize(JsonSerializer.Serialize(new LocalStorageConfig { FilePath = AWSLogPath }));

            if (!Directory.Exists(AWSLogPath))
            {
                Directory.CreateDirectory(AWSLogPath);
            }

            if (!Directory.Exists(AWSLogArchivePath))
            {
                Directory.CreateDirectory(AWSLogArchivePath);
            }

            // Configure the Timer
            _timerUpload.Elapsed += _timerUpload_Elapsed;
            _timerUpload.AutoReset = true;
            _timerUpload.Enabled = true;

            _timerUpload.Start();

            return true;
        }

        private async void _timerUpload_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            var files = Directory.GetFiles(AWSLogPath).Where(a => !a.Contains($"{DateTime.Today:MM_dd_yyyy}.log")).ToArray();

            if (files.Length == 0)
            {
                return;
            }

            _timerUpload.Enabled = false;

            try
            {
                var client = new AmazonS3Client(_config.IAMUser, _config.IAMSecret, Amazon.RegionEndpoint.USEast2);

                foreach (var file in files)
                {
                    PutObjectRequest putRequest = new()
                    {
                        BucketName = _config.BucketName,
                        Key = $"{Environment.MachineName}_{DateTime.Now.Date.ToShortDateString}",
                        FilePath = file,
                        ContentType = "text/plain"
                    };

                    PutObjectResponse response = await client.PutObjectAsync(putRequest);

                    if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                    {
                        Console.WriteLine($"Failed to upload {file}, status code: {response.HttpStatusCode}");
                    }
                    else
                    {
                        var fileInfo = new FileInfo(file);

                        File.Move(file, Path.Combine(AWSLogArchivePath, fileInfo.Name));
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            _timerUpload.Enabled = true;
        }

        public override async Task<bool> StoreEventAsync(string output) => await _localStorage.StoreEventAsync(output);
    }
}