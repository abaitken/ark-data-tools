using ArkDataProcessor.ArkGameModel;

namespace ArkDataProcessor
{
    abstract class DataProcessingPipeline
    {
        public abstract string Id { get; }

        internal abstract Task Execute(ArkGameData data, MonitoringSource configuration, List<SharedSetting> sharedSettings);
    }
}
