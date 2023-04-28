using EDR.Collector.lib.DynamicObjects.Base;

using System.Text.Json;

namespace EDR.Collector.lib.DynamicObjects.StorageTypes.Base
{
    public abstract class BaseStorageType : BaseDynamicObject
    {
        protected static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public abstract bool Initialize(string configStr);

        public abstract Task<bool> StoreEventAsync(string output);

        protected static T? ParseJSON<T>(string jsonStr) => JsonSerializer.Deserialize<T>(jsonStr);

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
            } catch (UnauthorizedAccessException uae)
            {
                logger.Error($"Failed to create path ({path}) due to a Unauthorized Access Exception ({uae})");

                return false;
            }

            return true;
        }
    }
}