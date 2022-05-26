using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    abstract class DataProcessingPipeline
    {
        public abstract string Id { get; }

        internal abstract void Execute(ArkGameData data, MonitoringSource configuration);

        internal Task ExecuteAsync(ArkGameData data, MonitoringSource configuration)
        {
            return Task.Run(() => Execute(data, configuration));
        }
    }
}
