using ArkSavegameToolkitNet.Domain;
using Newtonsoft.Json;

namespace ArkDataProcessor
{
    class EntityKeysPublishingPipeline : DataProcessingPipeline
    {
        private readonly EntityKeysPublishingPipelineMode _mode;

        public enum EntityKeysPublishingPipelineMode
        {
            Automated,
            Manual
        }

        public EntityKeysPublishingPipeline(EntityKeysPublishingPipelineMode mode)
        {
            _mode = mode;
        }

        public override string Id => _mode == EntityKeysPublishingPipelineMode.Automated ? "entity_keys" : "dump_keys";

        internal override async Task Execute(ArkGameData data, MonitoringSource configuration)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals(Id)).ToList();
            if (uploadTargets.Count == 0)
                return;

            var structuresByType = await new SelectStructuresByTypePipelineTask().ExecuteAsync(data.Structures);
            var tamedCreaturesByType = await new SelectCreaturesByTypePipelineTask().ExecuteAsync(data.TamedCreatures);
            var wildCreaturesByType = await new SelectCreaturesByTypePipelineTask().ExecuteAsync(data.WildCreatures);
            var itemsByType = await new SelectItemsByTypePipelineTask().ExecuteAsync(data.Items);

            var structureKeys = structuresByType.Keys.OrderBy(i => i).ToList();
            var creatureKeys = tamedCreaturesByType.Keys.Concat(wildCreaturesByType.Keys).Distinct().OrderBy(i => i).ToList();
            var itemKeys = itemsByType.Keys.OrderBy(i => i).ToList();
            var outputData = new
            {
                CreatureClasses = creatureKeys,
                StructureClasses = structureKeys,
                ItemClasses = itemKeys,
                LastUpdated = DateTime.UtcNow.ToString()
            };

            var tempPath = TemporaryFileServices.GenerateFileName(".json");
            File.WriteAllText(tempPath, JsonConvert.SerializeObject(outputData, Formatting.Indented));
            //await new StoreJsonDataPipelineTask<dynamic>().ExecuteAsync(outputData, tempPath);
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
