namespace ArkDataProcessor
{
    class LocalCopyFilePipelineTask : DataProcessingPipelineTaskNoResult<IEnumerable<UploadItem>, UploadTarget>
    {
        internal override Task Execute(IEnumerable<UploadItem> uploadItems, UploadTarget uploadTarget)
        {
            foreach (var item in uploadItems)
                File.Copy(item.LocalPath, item.RemotePath, true);
            return Task.CompletedTask;
        }
    }
}
