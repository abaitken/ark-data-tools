namespace ArkDataProcessor
{
    class PublishFilePipelineTaskFactory
    {
        public DataProcessingPipelineTaskNoResult<IEnumerable<UploadItem>, UploadTarget> Create(IEnumerable<UploadItem> uploadItems, UploadTarget uploadTarget)
        {
            var uploadItemsList = uploadItems.ToList();

            if (uploadItemsList.Count == 0)
                return new NoOperationPublishPipelineTask();

            return uploadTarget.Scheme switch
            {
                "sftp" => new SecureFileUploadPipelineTask(),
                "copy" => new LocalCopyFilePipelineTask(),
                _ => throw new ArgumentOutOfRangeException(nameof(uploadTarget.Scheme)),
            };
        }

    }
}
