using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class SelectCreatureLocationsByTypePipelineTask : DataProcessingPipelineTask<Dictionary<string, List<ArkCreature>>, Dictionary<string, List<Coordinate>>>
    {
        internal override Dictionary<string, List<Coordinate>> Execute(Dictionary<string, List<ArkCreature>> arg)
        {
            var result = new Dictionary<string, List<Coordinate>>();

            foreach (var item in arg)
            {
                var coords = from i in item.Value
                             where i.Location != null && i.Location.Latitude.HasValue && i.Location.Longitude.HasValue
                             select new Coordinate
                             {
                                 Lat = Math.Round((double)i.Location.Latitude, 1),
                                 Lon = Math.Round((double)i.Location.Longitude, 1),
                                 Level = i.BaseStats.Sum(i => i)
                             };

                result.Add(item.Key, coords.Distinct().ToList());
            }

            return result;
        }
    }
}
