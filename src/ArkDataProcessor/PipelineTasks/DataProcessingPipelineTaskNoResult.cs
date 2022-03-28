namespace ArkDataProcessor
{
    abstract class DataProcessingPipelineTaskNoResult<TArg1, TArg2>
    {
        internal abstract void Execute(TArg1 arg1, TArg2 arg2);

        internal Task ExecuteAsync(TArg1 arg1, TArg2 arg2)
        {
            return Task.Run(() => Execute(arg1, arg2));
        }
    }
}
