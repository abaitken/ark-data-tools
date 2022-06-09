using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class StructureLocationsPublishingPipeline : DataProcessingPipeline
    {
        public override string Id => "structure_locations";

        internal override async void Execute(ArkGameData data, MonitoringSource configuration)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals(Id)).ToList();
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
                await new PublishFilePipelineTask().ExecuteAsync(new[]{ new UploadItem{
                    LocalPath = tempPath,
                    RemotePath = uploadTarget.RemoteTarget
                } }, uploadTarget);
            _ = new RemoveFilePipelineTask().ExecuteAsync(tempPath);
        }
    }
}
