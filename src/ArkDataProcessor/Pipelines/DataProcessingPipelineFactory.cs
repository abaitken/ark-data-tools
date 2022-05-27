using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    class DataProcessingPipelineFactory
    {
        internal IEnumerable<DataProcessingPipeline> CreatePipelines()
        {
            yield return new WildCreatureLocationsPublishingPipeline();
            yield return new TamedCreatureLocationsPublishingPipeline();
            yield return new TamedCreatureBreedingDataPublishingPipeline();
            yield return new StructureLocationsPublishingPipeline();
            yield return new SaveFilePublishingPipeline();
        }
    }
}
