namespace ArkDataProcessor
{
    abstract class DataProcessingPipelineTask<TArg>
    {
        internal abstract Task Execute(TArg arg);
    }
}
