using ArkDataProcessor.ArkGameModel;

namespace ArkDataProcessor
{
    class WildCreatureLocationsChunkedPublishingPipeline : DataProcessingPipeline
    {
        public override string Id => "wild_creature_locations_chunked";

        internal override async Task Execute(ArkGameData data, MonitoringSource configuration, List<SharedSetting> sharedSettings)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals(Id)).ToList();
            if (uploadTargets.Count == 0)
                return;

            var creaturesByType = await new SelectCreaturesByTypePipelineTask().ExecuteAsync(data.WildCreatures);
            var locationsByType = await new SelectCreatureLocationsByTypePipelineTask().ExecuteAsync(creaturesByType);
            var filter = configuration.Filters?.FirstOrDefault(i => i.Id.Equals(Id));
            var filteredLocationsByType = await new FilterKeysPipelineTask<List<Coordinate>>().ExecuteAsync(locationsByType, filter);

            var publishFactory = new PublishFilePipelineTaskFactory();
            foreach (var uploadTarget in uploadTargets)
            {
                var uploadItems = new List<UploadItem>();

                // SCOPE : Store header
                {
                    var header = new
                    {
                        CreatureClasses = filteredLocationsByType.Keys,
                        LastUpdated = DateTime.UtcNow.ToString()
                    };

                    var tempPath = TemporaryFileServices.GenerateFileName(".json");
                    await new StoreJsonDataPipelineTask<dynamic>().Execute(header, tempPath);

                    uploadItems.Add(new UploadItem
                    {
                        LocalPath = tempPath,
                        RemotePath = uploadTarget.RemoteTarget + "classes.json"
                    });
                }

                foreach (var typeLocation in filteredLocationsByType)
                {
                    var tempPath = TemporaryFileServices.GenerateFileName(".json");
                    await new StoreJsonDataPipelineTask<dynamic>().Execute(typeLocation.Value, tempPath);

                    uploadItems.Add(new UploadItem
                    {
                        LocalPath = tempPath,
                        RemotePath = uploadTarget.RemoteTarget + typeLocation.Key + ".json"
                    });

                }

                var task = publishFactory.Create(uploadItems, uploadTarget);
                await task.Execute(uploadItems, uploadTarget);

                foreach (var item in uploadItems)
                    _ = new RemoveFilePipelineTask().Execute(item.LocalPath);
            }
        }
    }
}
