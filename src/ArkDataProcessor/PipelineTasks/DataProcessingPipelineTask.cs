namespace ArkDataProcessor
{
    abstract class DataProcessingPipelineTask<TArg>
    {
        internal abstract void Execute(TArg arg);

        internal Task ExecuteAsync(TArg arg)
        {
            return Task.Run(() => Execute(arg));
        }
    }
}
