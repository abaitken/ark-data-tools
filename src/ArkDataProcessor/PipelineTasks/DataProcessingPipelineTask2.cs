namespace ArkDataProcessor
{
    abstract class DataProcessingPipelineTask<TArg1, TArg2, TResult>
    {
        internal abstract TResult Execute(TArg1 arg1, TArg2 arg2);

        internal Task<TResult> ExecuteAsync(TArg1 arg1, TArg2 arg2)
        {
            return Task.Run(() => Execute(arg1, arg2));
        }
    }
}
