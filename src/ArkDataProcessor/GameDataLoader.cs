using ArkDataProcessor.ArkGameModel;
using SavegameToolkit;
using SavegameToolkitAdditions;

namespace ArkDataProcessor
{
    class GameDataLoader
    {
        public ArkGameData Load(string filepath, MapIdentity mapIdentity)
        {

            if (new FileInfo(filepath).Length > int.MaxValue)
            {
                throw new Exception("Input file is too large.");
            }

            var arkSavegame = new ArkSavegame();

            bool PredicateCreatures(GameObject o) => !o.IsItem && (o.Parent != null || o.Components.Any());
            bool PredicateCreaturesAndCryopods(GameObject o) => (!o.IsItem && (o.Parent != null || o.Components.Any())) || o.ClassString.Contains("Cryopod") || o.ClassString.Contains("SoulTrap_");

            using var stream = new MemoryStream(File.ReadAllBytes(filepath));
            using var archive = new ArkArchive(stream);

            arkSavegame.ReadBinary(archive, ReadingOptions.Create()
                .WithDataFiles(false)
                .WithEmbeddedData(false)
                .WithDataFilesObjectMap(false)
                //.WithObjectFilter(new Predicate<GameObject>(PredicateCreaturesAndCryopods))
                .WithCryopodCreatures(true)
                .WithBuildComponentTree(true));

            if (!arkSavegame.HibernationEntries.Any())
            {
                return new ArkGameData(arkSavegame, mapIdentity);
            }

            List<GameObject> combinedObjects = arkSavegame.Objects;

            foreach (HibernationEntry entry in arkSavegame.HibernationEntries)
            {
                var collector = new ObjectCollector(entry, 1);
                combinedObjects.AddRange(collector.Remap(combinedObjects.Count));
            }

            return new ArkGameData(new GameObjectContainer(combinedObjects), mapIdentity);
        }
    }
}
