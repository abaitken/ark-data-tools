namespace ArkDataProcessor
{
    class RemoveFilePipelineTask : DataProcessingPipelineTask<string>
    {
        internal override void Execute(string filename)
        {
            try
            {
                File.Delete(filename);
            }
            catch (Exception)
            {
            }
        }
    }
}
