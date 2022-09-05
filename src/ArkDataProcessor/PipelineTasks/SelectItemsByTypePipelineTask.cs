using ArkDataProcessor.ArkGameModel;

namespace ArkDataProcessor
{
    class SelectItemsByTypePipelineTask : DataProcessingPipelineTask<IEnumerable<ArkItem>, Dictionary<string, List<ArkItem>>>
    {
        internal override Dictionary<string, List<ArkItem>> Execute(IEnumerable<ArkItem> arg)
        {
            var itemsByType = from item in arg
                                  group item by item.ClassName into g
                                  select new
                                  {
                                      g.Key,
                                      Items = g.ToList()
                                  };

            return itemsByType.ToDictionary(k => k.Key, v => v.Items);
        }
    }
}
