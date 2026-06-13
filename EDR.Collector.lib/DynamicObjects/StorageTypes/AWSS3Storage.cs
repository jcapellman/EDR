using Amazon.S3;
using Amazon.S3.Model;
using Amazon;

using EDR.Collector.lib.DynamicObjects.StorageTypes.Base;
using System.Text.Json;
using static EDR.Collector.lib.DynamicObjects.StorageTypes.LocalStorage;

namespace EDR.Collector.lib.DynamicObjects.StorageTypes
{
    /// <summary>
    /// AWS S3 storage implementation for EDR events.
    /// Buffers events locally then uploads to S3 on a timer.
    /// </summary>
    public class AWSS3Storage : BaseStorageType
    {
        /// <summary>
        /// Configuration for AWS S3 storage.
        /// </summary>
        public class AWSConfig
        {
            /// <summary>
            /// AWS region endpoint (e.g., us-east-2).
            /// </summary>
            public string Region { get; set; }

            /// <summary>
            /// S3 bucket name for storing logs.
            /// </summary>
            public string BucketName { get; set; }

            /// <summary>
            /// Local file path for buffering before upload.
            /// </summary>
            public string FilePath { get; set; }

            /// <summary>
            /// AWS IAM user access key.
            /// </summary>
            public string IAMUser { get; set; }

            /// <summary>
            /// AWS IAM user secret key.
            /// </summary>
            public string IAMSecret { get; set; }

            /// <summary>
            /// Upload interval in milliseconds (default: 60000 = 1 minute).
            /// </summary>
            public int UploadIntervalMs { get; set; }

            public AWSConfig()
            {
                Region = RegionEndpoint.USEast2.SystemName;

                IAMUser = string.Empty;

                IAMSecret = string.Empty;

                BucketName = string.Empty;

                FilePath = string.Empty;

                UploadIntervalMs = 60000;
            }
        }

        private AWSConfig _config = new();

        private LocalStorage? _localStorage;

        private readonly string AWSLogPath = Path.Combine(AppContext.BaseDirectory, "AWSLogs");

        private readonly string AWSLogArchivePath = Path.Combine(AppContext.BaseDirectory, "AWSLogArchive");

        public override string Name => "AWS S3";

        private System.Timers.Timer? _timerUpload;

        public override bool Initialize(string configStr)
        {
            var result = ParseJSON<AWSConfig>(configStr);

            if (result == null)
            {
                logger.Error($"Initialization of {nameof(AWSS3Storage)} failed due to bad config json ({configStr})");

                return false;
            }

            _config = result;

            _localStorage = new LocalStorage();
            _localStorage.Initialize(JsonSerializer.Serialize(new LocalStorageConfig { FilePath = AWSLogPath }));

            ValidateDirectoryPath(AWSLogPath);
            ValidateDirectoryPath(AWSLogArchivePath);

            _timerUpload = new System.Timers.Timer(_config.UploadIntervalMs);
            _timerUpload.Elapsed += _timerUpload_Elapsed;
            _timerUpload.AutoReset = true;
            _timerUpload.Enabled = true;

            _timerUpload.Start();

            logger.Debug($"AWSS3Storage initialized with region {_config.Region}, upload interval {_config.UploadIntervalMs}ms");

            return true;
        }

        private async void _timerUpload_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            var files = Directory.GetFiles(AWSLogPath).Where(a => !a.Contains($"{DateTime.Today:MM_dd_yyyy}.log")).ToArray();

            if (files.Length == 0)
            {
                return;
            }

            if (_timerUpload == null)
            {
                return;
            }

            _timerUpload.Enabled = false;

            try
            {
                var region = RegionEndpoint.GetBySystemName(_config.Region);
                using var client = new AmazonS3Client(_config.IAMUser, _config.IAMSecret, region);

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);

                    PutObjectRequest putRequest = new()
                    {
                        BucketName = _config.BucketName,
                        Key = fileInfo.Name,
                        FilePath = file,
                        ContentType = "text/plain"
                    };

                    PutObjectResponse response = await client.PutObjectAsync(putRequest);

                    if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                    {
                        logger.Error($"Failed to upload {file}, status code: {response.HttpStatusCode}");
                    }
                    else
                    {
                        var archivePath = Path.Combine(AWSLogArchivePath, fileInfo.Name);
                        File.Move(file, archivePath, overwrite: true);
                        logger.Debug($"Successfully uploaded and archived {fileInfo.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error during S3 upload: {ex.Message}");
            }

            if (_timerUpload != null)
            {
                _timerUpload.Enabled = true;
            }
        }

        public override async Task<bool> StoreEventAsync(string output)
        {
            if (_localStorage == null)
            {
                logger.Error("LocalStorage is not initialized");
                return false;
            }

            return await _localStorage.StoreEventAsync(output);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_timerUpload != null)
                {
                    _timerUpload.Stop();
                    _timerUpload.Elapsed -= _timerUpload_Elapsed;
                    _timerUpload.Dispose();
                    _timerUpload = null;
                }

                _localStorage?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}