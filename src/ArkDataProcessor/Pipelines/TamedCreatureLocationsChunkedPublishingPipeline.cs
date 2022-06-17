using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class TamedCreatureLocationsChunkedPublishingPipeline : DataProcessingPipeline
    {
        public override string Id => "tamed_creature_locations_chunked";

        internal override async Task Execute(ArkGameData data, MonitoringSource configuration)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals(Id)).ToList();
            if (uploadTargets.Count == 0)
                return;

            var creaturesByType = await new SelectCreaturesByTypePipelineTask().ExecuteAsync(data.TamedCreatures);
            var locationsByType = await new SelectCreatureLocationsByTypePipelineTask().ExecuteAsync(creaturesByType);

            var publishFactory = new PublishFilePipelineTaskFactory();
            foreach (var uploadTarget in uploadTargets)
            {
                var uploadItems = new List<UploadItem>();

                // SCOPE : Store header
                {
                    var header = new
                    {
                        CreatureClasses = locationsByType.Keys,
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

                foreach (var typeLocation in locationsByType)
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
