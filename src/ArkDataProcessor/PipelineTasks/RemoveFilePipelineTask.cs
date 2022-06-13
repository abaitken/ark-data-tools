namespace ArkDataProcessor
{
    class RemoveFilePipelineTask : DataProcessingPipelineTask<string>
    {
        internal override Task Execute(string filename)
        {
            try
            {
                File.Delete(filename);
            }
            catch (Exception)
            {
            }

            return Task.CompletedTask;
        }
    }
}
