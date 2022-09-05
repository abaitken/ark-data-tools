using SavegameToolkit;
using SavegameToolkitAdditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkDataProcessor.ArkGameModel
{
    internal class ArkGameData
    {
        private GameObjectContainer _gameObjectContainer;
        private readonly MapIdentity _mapkey;

        public ArkGameData(GameObjectContainer gameObjectContainer, MapIdentity mapkey)
        {
            _gameObjectContainer = gameObjectContainer;
            _mapkey = mapkey;
        }

        public IEnumerable<ArkStructure> Structures
        {
            get => from entity in _gameObjectContainer
                   where !entity.IsCreature() && !entity.IsItem && entity.Location != null
                   select new ArkStructure(entity, _mapkey);
        }

        public IEnumerable<ArkTamedCreature> TamedCreatures
        {
            get => from entity in _gameObjectContainer
                   where entity.IsCreature() && entity.IsTamed()
                   select new ArkTamedCreature(entity, _mapkey);
        }

        public IEnumerable<ArkCreature> WildCreatures
        {
            get => from entity in _gameObjectContainer
                   where entity.IsCreature() && !entity.IsTamed()
                   select new ArkCreature(entity, _mapkey);
        }

        public IEnumerable<ArkItem> Items
        {
            get => from entity in _gameObjectContainer
                   where entity.IsItem
                   select new ArkItem(entity, _mapkey);
        }
    }
}
