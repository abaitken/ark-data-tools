namespace ArkDataProcessor
{
    abstract class DataProcessingPipelineTaskNoResult<TArg1, TArg2>
    {
        internal abstract Task Execute(TArg1 arg1, TArg2 arg2);
    }
}
