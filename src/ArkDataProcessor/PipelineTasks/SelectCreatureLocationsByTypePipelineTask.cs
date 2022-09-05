using ArkDataProcessor.ArkGameModel;

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
                             let location = i.Location
                             where location != null
                             let lat = location.Latitude
                             let lon = location.Longitude
                             where lat.HasValue && lon.HasValue
                             select new Coordinate
                             {
                                 Lat = Math.Round((double)lat, 1),
                                 Lon = Math.Round((double)lon, 1),
                                 Level = i.BaseStats.Sum(i => i)
                             };

                result.Add(item.Key, coords.Distinct().ToList());
            }

            return result;
        }
    }
}
