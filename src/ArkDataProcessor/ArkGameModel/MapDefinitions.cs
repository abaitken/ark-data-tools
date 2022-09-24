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

        public static void UpdateDefinition(MapDefinition definition)
        {
            if (!Instance._definitions.ContainsKey(definition.Name))
                Instance._definitions.Add(definition.Name, definition);
            else
                Instance._definitions[definition.Name] = definition;
        }

        internal readonly Dictionary<string, MapDefinition> _definitions;

        private MapDefinitions()
        {
            _definitions = DefaultDefinitions.ToDictionary(k => k.Name, v => v);
        }

        private static IEnumerable<MapDefinition> DefaultDefinitions
        {
            get
            {
                yield return new MapDefinition("TheIsland", 50.0f, 8000.0f, 50.0f, 8000.0f);
                yield return new MapDefinition("TheCenter", 30.34223747253418f, 9584.0f, 55.10416793823242f, 9600.0f);
                yield return new MapDefinition("ScorchedEarth_P", 50.0f, 8000.0f, 50.0f, 8000.0f);
                yield return new MapDefinition("Aberration_P", 50.0f, 8000.0f, 50.0f, 8000.0f);
                yield return new MapDefinition("Extinction", 50.0f, 8000.0f, 50.0f, 8000.0f);
                yield return new MapDefinition("ShigoIslands", 50.001777870738339260f, 9562.0f, 50.001777870738339260f, 9562.0f);
                yield return new MapDefinition("Ragnarok", 50.009388f, 13100f, 50.009388f, 13100f);
                yield return new MapDefinition("TheVolcano", 50.0f, 9181.0f, 50.0f, 9181.0f);
                yield return new MapDefinition("CrystalIsles", 50.0f, 13718.0f, 50.0f, 13718.0f);
                yield return new MapDefinition("Valguero_P", 50.0f, 8161.0f, 50.0f, 8161.0f);
                yield return new MapDefinition("Genesis", 50.0f, 10500.0f, 50.0f, 10500.0f);
                yield return new MapDefinition("Valhalla", 48.813560485839844f, 14750.0f, 48.813560485839844f, 14750.0f);
                yield return new MapDefinition("MortemTupiu", 32.479148864746094f, 20000.0f, 40.59893798828125f, 16000.0f);
                yield return new MapDefinition("PGARK", 0.0f, 6080.0f, 0.0f, 6080.0f);
                yield return new MapDefinition("Fjordur", 50.009388f, 13100f, 50.009388f, 13100f);
                yield return new MapDefinition("Genesis2", 50.009388f, 13100f, 50.009388f, 13100f);
            }
        }
    }
}
