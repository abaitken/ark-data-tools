using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class SelectStructureLocationsByTypePipelineTask : DataProcessingPipelineTask<Dictionary<string, List<ArkStructure>>, Dictionary<string, List<Coordinate>>>
    {
        internal override Dictionary<string, List<Coordinate>> Execute(Dictionary<string, List<ArkStructure>> arg)
        {
            var result = new Dictionary<string, List<Coordinate>>();

            foreach (var item in arg)
            {
                if (!IsValid(item.Key))
                    continue;
                var coords = from i in item.Value
                             where i.Location != null && i.Location.Latitude.HasValue && i.Location.Longitude.HasValue
                             select new Coordinate
                             {
                                 Lat = Math.Round((double)i.Location.Latitude, 1),
                                 Lon = Math.Round((double)i.Location.Longitude, 1),
                                 Level = 0
                             };

                result.Add(item.Key, coords.Distinct().ToList());
            }

            return result;
        }

        private readonly string[] _validKeys = { "WyvernNest_C" };

        private bool IsValid(string key)
        {
            return _validKeys.Contains(key);
        }
    }
}
