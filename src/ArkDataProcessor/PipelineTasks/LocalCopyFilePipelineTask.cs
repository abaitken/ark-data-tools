namespace ArkDataProcessor
{
    class LocalCopyFilePipelineTask : DataProcessingPipelineTaskNoResult<IEnumerable<UploadItem>, UploadTarget>
    {
        internal override Task Execute(IEnumerable<UploadItem> uploadItems, UploadTarget uploadTarget)
        {
            foreach (var item in uploadItems)
            {
                var directoryName = Path.GetDirectoryName(item.RemotePath);
                if (!string.IsNullOrWhiteSpace(directoryName) && !Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);
                File.Copy(item.LocalPath, item.RemotePath, true);
            }
            return Task.CompletedTask;
        }
    }
}
