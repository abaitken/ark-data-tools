using ArkSavegameToolkitNet.Domain;
using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    class WildCreatureLocationsPublishingPipeline : DataProcessingPipeline
    {
        internal override async void Execute(ArkGameData data, Configuration configuration)
        {
            var uploadTarget = configuration.UploadTargets.FirstOrDefault(i => i.Id.Equals("wild_creature_locations"));
            if (uploadTarget == null)
                return;

            var creaturesByType = await new SelectCreaturesByTypePipelineTask().ExecuteAsync(data.WildCreatures);
            var locationsByType = await new SelectLocationsByTypePipelineTask().ExecuteAsync(creaturesByType);
            var creatureLocationData = new
            {
                CreatureClasses = locationsByType.Keys,
                Locations = locationsByType,
                LastUpdated = DateTime.UtcNow.ToString()
            };

            var tempPath = TemporaryFileServices.GenerateFileName(".json");
            await new StoreJsonDataPipelineTask<dynamic>().ExecuteAsync(creatureLocationData, tempPath);
            await new PublishFilePipelineTask().ExecuteAsync(tempPath, uploadTarget);
            _ = new RemoveFilePipelineTask().ExecuteAsync(tempPath);
        }
    }
}
