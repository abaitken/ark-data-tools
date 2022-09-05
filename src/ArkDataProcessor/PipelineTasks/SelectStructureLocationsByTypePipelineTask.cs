using ArkDataProcessor.ArkGameModel;

namespace ArkDataProcessor
{
    class SelectStructureLocationsByTypePipelineTask : DataProcessingPipelineTask<Dictionary<string, List<ArkStructure>>, Dictionary<string, List<Coordinate>>>
    {
        internal override Dictionary<string, List<Coordinate>> Execute(Dictionary<string, List<ArkStructure>> arg)
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
                                 Level = 0
                             };

                result.Add(item.Key, coords.Distinct().ToList());
            }

            return result;
        }
    }
}
