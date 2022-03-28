using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    class DataProcessingPipelineFactory
    {
        private Configuration _configuration;

        public DataProcessingPipelineFactory(Configuration configuration)
        {
            _configuration = configuration;
        }

        internal IEnumerable<DataProcessingPipeline> CreatePipelines()
        {
            yield return new WildCreatureLocationsPublishingPipeline();
            yield return new TamedCreatureLocationsPublishingPipeline();
            yield return new TamedCreatureBreedingDataPublishingPipeline();
        }
    }
}
