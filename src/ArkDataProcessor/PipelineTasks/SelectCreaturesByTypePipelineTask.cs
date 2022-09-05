using ArkDataProcessor.ArkGameModel;

namespace ArkDataProcessor
{
    class SelectCreaturesByTypePipelineTask : DataProcessingPipelineTask<IEnumerable<ArkCreature>, Dictionary<string, List<ArkCreature>>>
    {
        internal override Dictionary<string, List<ArkCreature>> Execute(IEnumerable<ArkCreature> arg)
        {
            var creaturesByType = from item in arg
                                  group item by item.ClassName into g
                                  select new
                                  {
                                      g.Key,
                                      Items = g.ToList()
                                  };

            return creaturesByType.ToDictionary(k => k.Key, v => v.Items);
        }
    }
}
