using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class SaveFilePublishingPipeline : DataProcessingPipeline
    {
        public override string Id => "raw_save_file";

        internal override async Task Execute(ArkGameData data, MonitoringSource configuration, List<SharedSetting> sharedSettings)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals(Id)).ToList();
            if (uploadTargets.Count == 0)
                return;

            var tempPath = TemporaryFileServices.GenerateFileName(".ark");
            File.Copy(configuration.FilePath, tempPath);
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
