using Newtonsoft.Json;

namespace ArkDataProcessor
{
    internal class ConfigurationReader
    {
        internal Configuration Load(string configurationFile)
        {
            var serializer = new JsonSerializer();
            using var reader = new StreamReader(configurationFile);

            var result = serializer.Deserialize(reader, typeof(Configuration)) as Configuration;
            if (result == null)
                throw new InvalidOperationException("Failed to load configuration");

            if(result.MonitoringSources != null && result.MonitoringSources.Count != 0)
            {
                foreach (var item in result.MonitoringSources)
                    LoadAndMerge(configurationFile, item);
            }

            return result;
        }

        private void LoadAndMerge(string parentConfigurationFile, MonitoringSource item)
        {
            if (string.IsNullOrEmpty(item.ExternalConfiguration))
                return;

            var configurationFile = ResolveFilePath(item.ExternalConfiguration, parentConfigurationFile);

            var serializer = new JsonSerializer();
            using var reader = new StreamReader(configurationFile);

            var external = serializer.Deserialize(reader, typeof(MonitoringSource)) as MonitoringSource;
            if (external == null)
                throw new InvalidOperationException("Failed to load monitoring source");
            item.FilePath = external.FilePath ?? item.FilePath;
            item.Filters = external.Filters ?? item.Filters;
            item.UploadTargets = external.UploadTargets ?? item.UploadTargets;
        }

        private string ResolveFilePath(string filePath, string relativeTo)
        {
            if (Path.IsPathRooted(filePath))
                return filePath;

            var fullPath = Path.GetFullPath(relativeTo);
            var directory = Path.GetDirectoryName(fullPath);
            var subjectPath = Path.Combine(directory ?? "." , filePath);
            var resolvedPath = Path.GetFullPath(subjectPath);

            return resolvedPath;
        }
    }
}