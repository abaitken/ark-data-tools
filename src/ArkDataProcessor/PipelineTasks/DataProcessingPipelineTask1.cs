namespace ArkDataProcessor
{
    abstract class DataProcessingPipelineTask<TArg, TResult>
    {
        internal abstract TResult Execute(TArg arg);

        internal Task<TResult> ExecuteAsync(TArg arg)
        {
            return Task.Run(() => Execute(arg));
        }
    }
}
