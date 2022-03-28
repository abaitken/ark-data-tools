using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class GameDataLoader
    {
        static GameDataLoader()
        {
            // initialize default settings (maps etc.)
            ArkToolkitDomain.Initialize();
        }

        public ArkGameData Load(string filepath)
        {
            var domainOnly = true; //true: optimize loading of the domain model, false: load everything and keep references in memory

            //prepare
            //var cd = new ArkClusterData(clusterPath, loadOnlyPropertiesInDomain: domainOnly);
            var gd = new ArkGameData(filepath, /*cd, */loadOnlyPropertiesInDomain: domainOnly);

            //extract savegame
            if (gd.Update(CancellationToken.None, null, true)?.Success == true)
            {
                //extract cluster data
                //var clusterResult = cd.Update(CancellationToken.None);

                //assign the new data to the domain model
                gd.ApplyPreviousUpdate(domainOnly);

                return gd;
            }

            throw new InvalidOperationException("Failed to load save game data");
        }
    }
}
