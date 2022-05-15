using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class StructureLocationsPublishingPipeline : DataProcessingPipeline
    {
        internal override async void Execute(ArkGameData data, Configuration configuration)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals("structure_locations")).ToList();
            if (uploadTargets.Count == 0)
                return;

            var structuresByType = await new SelectStructuresByTypePipelineTask().ExecuteAsync(data.Structures);
            var locationsByType = await new SelectStructureLocationsByTypePipelineTask().ExecuteAsync(structuresByType);
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
