namespace ArkDataProcessor
{
    class NoOperationPublishPipelineTask : DataProcessingPipelineTaskNoResult<IEnumerable<UploadItem>, UploadTarget>
    {
        internal override Task Execute(IEnumerable<UploadItem> uploadItems, UploadTarget uploadTarget)
        {
            return Task.CompletedTask;
        }
    }
}
