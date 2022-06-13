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
            return result;
        }
    }
}