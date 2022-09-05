namespace ArkDataProcessor.ArkGameModel
{
    internal class MapDefinitions
    {
        private static MapDefinitions _instance;
        private static MapDefinitions Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MapDefinitions();

                return _instance;
            }
        }

        public static MapDefinition? GetDefinition(MapIdentity mapkey)
        {
            if (Instance._definitions.TryGetValue(mapkey.MapKey, out var result))
                return result;
            return null;
        }

        private MapDefinitions()
        {

        }

        internal readonly Dictionary<string, MapDefinition> _definitions = new()
        {
            {"TheIsland", new MapDefinition(50.0f, 8000.0f, 50.0f, 8000.0f)},
            {"TheCenter", new MapDefinition(30.34223747253418f, 9584.0f, 55.10416793823242f, 9600.0f)},
            {"ScorchedEarth_P", new MapDefinition(50.0f, 8000.0f, 50.0f, 8000.0f)},
            {"Aberration_P", new MapDefinition(50.0f, 8000.0f, 50.0f, 8000.0f)},
            {"Extinction", new MapDefinition(50.0f, 8000.0f, 50.0f, 8000.0f)},
            {"ShigoIslands", new MapDefinition(50.001777870738339260f, 9562.0f, 50.001777870738339260f, 9562.0f)},
            {"Ragnarok", new MapDefinition(50.009388f, 13100f, 50.009388f, 13100f)},
            {"TheVolcano", new MapDefinition(50.0f, 9181.0f, 50.0f, 9181.0f)},
            {"CrystalIsles", new MapDefinition(50.0f, 13718.0f, 50.0f, 13718.0f)},
            {"Valguero_P", new MapDefinition(50.0f, 8161.0f, 50.0f, 8161.0f)},
            {"Genesis", new MapDefinition(50.0f, 10500.0f, 50.0f, 10500.0f)},
            {"Valhalla", new MapDefinition(48.813560485839844f, 14750.0f, 48.813560485839844f, 14750.0f)},
            {"MortemTupiu", new MapDefinition(32.479148864746094f, 20000.0f, 40.59893798828125f, 16000.0f)},
            {"PGARK", new MapDefinition(0.0f, 6080.0f, 0.0f, 6080.0f)}
        };
    }
}
