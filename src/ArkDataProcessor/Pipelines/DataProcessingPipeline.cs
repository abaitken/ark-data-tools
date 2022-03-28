using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    abstract class DataProcessingPipeline
    {
        internal abstract void Execute(ArkGameData data, Configuration configuration);

        internal Task ExecuteAsync(ArkGameData data, Configuration configuration)
        {
            return Task.Run(() => Execute(data, configuration));
        }
    }
}
