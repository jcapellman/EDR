using EDR.Collector.lib.DynamicObjects.Base;

using System.Text.Json;

namespace EDR.Collector.lib.DynamicObjects.StorageTypes.Base
{
    /// <summary>
    /// Base class for all storage type implementations.
    /// Provides common functionality for JSON parsing and directory validation.
    /// </summary>
    public abstract class BaseStorageType : BaseDynamicObject, IDisposable
    {
        protected static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private bool _disposed;

        /// <summary>
        /// Initialize the storage type with configuration.
        /// </summary>
        /// <param name="configStr">JSON configuration string.</param>
        /// <returns>True if initialization succeeded, false otherwise.</returns>
        public abstract bool Initialize(string configStr);

        /// <summary>
        /// Store an event asynchronously.
        /// </summary>
        /// <param name="output">The formatted event output to store.</param>
        /// <returns>True if storage succeeded, false otherwise.</returns>
        public abstract Task<bool> StoreEventAsync(string output);

        /// <summary>
        /// Parse JSON string to typed object.
        /// </summary>
        protected static T? ParseJSON<T>(string jsonStr) => JsonSerializer.Deserialize<T>(jsonStr);

        /// <summary>
        /// Validate and create directory path if it doesn't exist.
        /// </summary>
        protected static bool ValidateDirectoryPath(string path)
        {
            if (Path.Exists(path))
            {
                return true;
            }

            try
            {
                Directory.CreateDirectory(path);

                logger.Debug($"Directories in path ({path}) did not exist, but was created successfully");
            }
            catch (UnauthorizedAccessException uae)
            {
                logger.Error($"Failed to create path ({path}) due to a Unauthorized Access Exception ({uae})");

                return false;
            }

            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}