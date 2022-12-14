using ArkDataProcessor.ArkGameModel;
using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    class WildCreatureLocationsPublishingPipeline : DataProcessingPipeline
    {
        public override string Id => "wild_creature_locations";

        internal override async Task Execute(ArkGameData data, MonitoringSource configuration, List<SharedSetting> sharedSettings)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals(Id)).ToList();
            if (uploadTargets.Count == 0)
                return;

            var creaturesByType = await new SelectCreaturesByTypePipelineTask().ExecuteAsync(data.WildCreatures);
            var locationsByType = await new SelectCreatureLocationsByTypePipelineTask().ExecuteAsync(creaturesByType);
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
