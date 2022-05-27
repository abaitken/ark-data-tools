using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class SaveFilePublishingPipeline : DataProcessingPipeline
    {
        public override string Id => "raw_save_file";

        internal override async void Execute(ArkGameData data, MonitoringSource configuration)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals(Id)).ToList();
            if (uploadTargets.Count == 0)
                return;

            var tempPath = TemporaryFileServices.GenerateFileName(".ark");
            File.Copy(configuration.FilePath, tempPath);
            foreach (var uploadTarget in uploadTargets)
                await new PublishFilePipelineTask().ExecuteAsync(tempPath, uploadTarget);
            _ = new RemoveFilePipelineTask().ExecuteAsync(tempPath);
        }
    }
}
