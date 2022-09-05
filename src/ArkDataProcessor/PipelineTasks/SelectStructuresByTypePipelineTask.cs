using ArkDataProcessor.ArkGameModel;

namespace ArkDataProcessor
{
    class SelectStructuresByTypePipelineTask : DataProcessingPipelineTask<IEnumerable<ArkStructure>, Dictionary<string, List<ArkStructure>>>
    {
        internal override Dictionary<string, List<ArkStructure>> Execute(IEnumerable<ArkStructure> arg)
        {
            var structuresByType = from item in arg
                                  group item by item.ClassName into g
                                  select new
                                  {
                                      g.Key,
                                      Items = g.ToList()
                                  };

            return structuresByType.ToDictionary(k => k.Key, v => v.Items);
        }
    }
}
