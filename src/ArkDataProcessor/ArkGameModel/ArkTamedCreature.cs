using Newtonsoft.Json;
using SavegameToolkit;
using SavegameToolkit.Types;

namespace ArkDataProcessor.ArkGameModel
{
    class ArkTamedCreature : ArkCreature
    {
        public ArkTamedCreature(GameObject entity, MapIdentity mapIdentity)
            : base(entity, mapIdentity)
        {
        }

        public string Name { get => Entity.GetPropertyValue<string>("TamedName"); }
        public string OwningPlayerName { get => string.IsNullOrWhiteSpace(ImprinterName) ? Entity.GetPropertyValue<string>("TamerString") : ImprinterName; }
        public string ImprinterName { get => Entity.GetPropertyValue<string>("ImprinterName"); }
        public string TribeName { get => Entity.GetPropertyValue<string>("TribeName"); }
        [JsonConverter(typeof(ByteArrayConverter))]
        public int[] TamedStats
        {
            get
            {
                var result = new int[Stats.StatsCount];
                for (int i = 0; i < Stats.StatsCount; i++)
                {
                    if (StatusObject == null)
                        result[i] = -1;
                    else
                        result[i] = StatusObject.GetPropertyValue<ArkByteValue>("NumberOfLevelUpPointsAppliedTamed", i)?.ByteValue ?? 0;
                }

                return result;
            }
        }
        public double? TamedAtTime { get => Entity.GetPropertyValue<double>("TamedAtTime"); }
        public bool IsNeutered { get => Entity.GetPropertyValue<bool>("bNeutered"); }
        public float? TamedIneffectivenessModifier { get => StatusObject?.GetPropertyValue("TamedIneffectivenessModifier", defaultValue: float.NaN); }
        public float? DinoImprintingQuality { get => StatusObject?.GetPropertyValue<float>("DinoImprintingQuality"); }
        public int RandomMutationsFemale { get => Entity.GetPropertyValue<int>("RandomMutationsFemale"); }
        public int RandomMutationsMale { get => Entity.GetPropertyValue<int>("RandomMutationsMale"); }
        public int RandomMutationTotal => RandomMutationsMale + RandomMutationsFemale;

    }
}
