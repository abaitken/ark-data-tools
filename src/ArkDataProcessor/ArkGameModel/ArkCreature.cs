using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SavegameToolkit;
using SavegameToolkit.Types;
using SavegameToolkitAdditions;

namespace ArkDataProcessor.ArkGameModel
{
    class ArkCreature : ArkGameDataContainerBase
    {
        private readonly GameObject _statusObject;

        public ArkCreature(GameObject entity, MapIdentity mapIdentity)
            : base(entity, mapIdentity)
        {

            _statusObject = entity.CharacterStatusComponent();
        }

        protected GameObject StatusObject { get => _statusObject; }

        [JsonConverter(typeof(ByteArrayConverter))]
        public byte[] Colors
        {
            get
            {
                const int ColorRegionCount = 6;
                var result = new byte[ColorRegionCount];
                for (int i = 0; i < ColorRegionCount; i++)
                {
                    result[i] = Entity.GetPropertyValue<ArkByteValue>("ColorSetIndices", i)?.ByteValue ?? 0;
                }

                return result;
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public ArkCreatureGender Gender { get => Entity.IsFemale() ? ArkCreatureGender.Female : ArkCreatureGender.Male; }

        [JsonConverter(typeof(ByteArrayConverter))]
        public int[] BaseStats
        {
            get
            {
                var result = new int[Stats.StatsCount];
                for (int i = 0; i < Stats.StatsCount; i++)
                {
                    if (StatusObject == null)
                        result[i] = -1;
                    else
                        result[i] = StatusObject.GetPropertyValue<ArkByteValue>("NumberOfLevelUpPointsApplied", i)?.ByteValue ?? 0;
                }
                if (StatusObject != null)
                    result[Stats.Torpidity] = StatusObject.GetPropertyValue("BaseCharacterLevel", defaultValue: 1) - 1; // torpor

                return result;
            }
        }

    }
}
