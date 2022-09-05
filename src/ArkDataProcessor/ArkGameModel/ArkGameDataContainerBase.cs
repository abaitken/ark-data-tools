using SavegameToolkit;

namespace ArkDataProcessor.ArkGameModel
{
    class ArkGameDataContainerBase
    {
        private GameObject _entity;
        private readonly MapIdentity _mapIdentity;

        public ArkGameDataContainerBase(GameObject entity, MapIdentity mapIdentity)
        {
            _entity = entity;
            _mapIdentity = mapIdentity;
        }

        protected GameObject Entity { get => _entity; }

        public string ClassName { get => Entity.ClassName.Name; }

        public ArkLocation Location
        {
            get
            {
                var definition = MapDefinitions.GetDefinition(_mapIdentity);
                if (definition == null)
                    throw new InvalidOperationException();
                return new ArkLocation(Entity.Location, definition);
            }
        }
    }
}
