using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class StructureLocationsPublishingPipeline : DataProcessingPipeline
    {
        public override string Id => "structure_locations";

        internal override async Task Execute(ArkGameData data, MonitoringSource configuration, List<SharedSetting> sharedSettings)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals(Id)).ToList();
            if (uploadTargets.Count == 0)
                return;

            var structuresByType = await new SelectStructuresByTypePipelineTask().ExecuteAsync(data.Structures);
            var locationsByType = await new SelectStructureLocationsByTypePipelineTask().ExecuteAsync(structuresByType);
            var filter = configuration.Filters?.FirstOrDefault(i => i.Id.Equals(Id));
            var filteredLocationsByType = await new FilterKeysPipelineTask<List<Coordinate>>().ExecuteAsync(locationsByType, filter);
            var creatureLocationData = new
            {
                CreatureClasses = filteredLocationsByType.Keys,
                Locations = filteredLocationsByType,
                LastUpdated = DateTime.UtcNow.ToString()
            };

            var tempPath = TemporaryFileServices.GenerateFileName(".json");
            await new StoreJsonDataPipelineTask<dynamic>().Execute(creatureLocationData, tempPath);
            var publishFactory = new PublishFilePipelineTaskFactory();
            foreach (var uploadTarget in uploadTargets)
            {
                var uploadItems = new[]{ new UploadItem{
                    LocalPath = tempPath,
                    RemotePath = uploadTarget.RemoteTarget
                } };
                var task = publishFactory.Create(uploadItems, uploadTarget);
                await task.Execute(uploadItems, uploadTarget);
            }
            _ = new RemoveFilePipelineTask().Execute(tempPath);
        }
    }
}
