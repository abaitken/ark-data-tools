using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class TamedCreatureLocationsPublishingPipeline : DataProcessingPipeline
    {
        public override string Id => "tamed_creature_locations";

        internal override async void Execute(ArkGameData data, MonitoringSource configuration)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals(Id)).ToList();
            if (uploadTargets.Count == 0)
                return;

            var creaturesByType = await new SelectCreaturesByTypePipelineTask().ExecuteAsync(data.TamedCreatures);
            var locationsByType = await new SelectCreatureLocationsByTypePipelineTask().ExecuteAsync(creaturesByType);
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
