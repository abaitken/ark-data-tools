using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class TamedCreatureLocationsPublishingPipeline : DataProcessingPipeline
    {
        internal override async void Execute(ArkGameData data, Configuration configuration)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals("tamed_creature_locations")).ToList();
            if (uploadTargets.Count == 0)
                return;

            var creaturesByType = await new SelectCreaturesByTypePipelineTask().ExecuteAsync(data.TamedCreatures);
            var locationsByType = await new SelectLocationsByTypePipelineTask().ExecuteAsync(creaturesByType);
            var creatureLocationData = new
            {
                CreatureClasses = locationsByType.Keys,
                Locations = locationsByType,
                LastUpdated = DateTime.UtcNow.ToString()
            };

            var tempPath = TemporaryFileServices.GenerateFileName(".json");
            await new StoreJsonDataPipelineTask<dynamic>().ExecuteAsync(creatureLocationData, tempPath);
            foreach (var uploadTarget in uploadTargets)
                await new PublishFilePipelineTask().ExecuteAsync(tempPath, uploadTarget);
            _ = new RemoveFilePipelineTask().ExecuteAsync(tempPath);
        }
    }
}
