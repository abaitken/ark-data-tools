namespace ArkDataProcessor
{
    class FilterKeysPipelineTask<T> : DataProcessingPipelineTask<Dictionary<string, T>, Filter?, Dictionary<string, T>>
    {
        internal override Dictionary<string, T> Execute(Dictionary<string, T> arg1, Filter? arg2)
        {
            if(arg2 == null)
                return arg1;

            var includeAll = arg2.Include == null || arg2.Include.Count == 0;
            var excludeNone = arg2.Exclude == null || arg2.Exclude.Count == 0;

            if (includeAll && excludeNone)
                return arg1;

            var result = new Dictionary<string, T>();

            foreach (var item in arg1)
            {
                if (arg2.Exclude != null && arg2.Exclude.Contains(item.Key))
                    continue;

                if (includeAll || (arg2.Include != null && arg2.Include.Contains(item.Key)))
                    result.Add(item.Key, item.Value);
            }

            return result;
        }
    }
}
