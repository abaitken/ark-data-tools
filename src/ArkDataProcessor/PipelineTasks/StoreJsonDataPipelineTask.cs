using Newtonsoft.Json;

namespace ArkDataProcessor
{
    class StoreJsonDataPipelineTask<T> : DataProcessingPipelineTaskNoResult<T, string>
    {
        internal override void Execute(T arg1, string arg2)
        {
            File.WriteAllText(arg2, JsonConvert.SerializeObject(arg1, Formatting.None));
        }
    }
}
