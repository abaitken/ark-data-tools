using ArkSavegameToolkitNet.Domain;
using Newtonsoft.Json;
using System.Reflection;

namespace ArkDataProcessor
{

    class TamedCreatureBreedingDataPublishingPipeline : DataProcessingPipeline
    {
        private static readonly Dictionary<string, CreatureClassData> _mapping;

        public override string Id => "tamed_breeding_data";

        public class CreatureClassData
        {
            public string ClassName { get; set; }
            public string BlueprintPath { get; set; }
            public string Name { get; set; }
            public string DescriptiveName { get; set; }
        }

        static TamedCreatureBreedingDataPublishingPipeline()
        {
            var embeddedResource = new EmbeddedResource(Assembly.GetExecutingAssembly(), "ArkDataProcessor.Resources.ClassMapping.json");
            var stream = embeddedResource.Read();
            if (stream == null)
                throw new InvalidOperationException("Failed to read class mapping resource data");
            using var reader = new StreamReader(stream);
            var classData = JsonConvert.DeserializeObject<CreatureClassData[]>(reader.ReadToEnd());
            _mapping = classData.ToDictionary(k => k.ClassName, v => v);
        }

        internal override async Task Execute(ArkGameData data, MonitoringSource configuration, List<SharedSetting> sharedSettings)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals(Id)).ToList();
            if (uploadTargets.Count == 0)
                return;

            var tamedCreatures = data.TamedCreatures;


            //public enum CreatureStatus
            //{
            //    Available,
            //    Dead,
            //    Unavailable,
            //    Obelisk,
            //    Cryopod
            //};

            // speciesName, speciesBlueprintPath, name, owner, imprinter, tribe, server, note, sex [m/w], status, isBred [bool], neutered [bool], mutagen [bool],
            // taming effectiveness [double], imprinting bonus [double], mutations maternal [int], mutations paternal [int], 12 stat levels wild [int], 12 stat levels dom [int], 6 color ids [int]
            // in total: 10 string fields, 3 bool fields, 2 double fields (each two regex groups), 32 int fields

            var records = from item in tamedCreatures
                          let blueprintPath = CreateSpeciesBlueprintPath(item)
                          where !string.IsNullOrWhiteSpace(blueprintPath)
                          select new object[]
                          {
                            /* speciesName */ CreateSpeciesName(item),
                            /* speciesBlueprintPath */ blueprintPath,
                            /* name */ item.Name,
                            /* owner */ item.OwningPlayerName,
                            /* imprinter */ item.ImprinterName,
                            /* tribe */ item.TribeName,
                            /* server */ item.TamedOnServerName,
                            /* note */ string.Empty,
                            /* sex [m/w] */ item.Gender == ArkCreatureGender.Male ? "m" : "w",
                            /* status */  "Available",
                            /* isBred [bool] */ item.TamedAtTime.HasValue ? "false" : "true",
                            /* neutered [bool] */ item.IsNeutered ? "true" : "false",
                            /* mutagen [bool] */ "false",
                            /* taming effectiveness [double] */ Math.Round((item.TamedIneffectivenessModifier.HasValue ? item.TamedIneffectivenessModifier.Value : 1.0) * 100, 1),
                            /* imprinting bonus [double] */ Math.Round((item.DinoImprintingQuality.HasValue ? item.DinoImprintingQuality.Value : 0.0) * 100, 1),
                            /* mutations maternal [int] */ item.RandomMutationsMale,
                            /* mutations paternal [int] */ item.RandomMutationsFemale,
                            /* (Wild) Health [int] */ item.BaseStats[0],
                            /* (Wild) Stamina [int] */ item.BaseStats[1],
                            /* (Wild) Torpidity [int] */ item.BaseStats[2],
                            /* (Wild) Oxygen [int] */ item.BaseStats[3],
                            /* (Wild) Food [int] */ item.BaseStats[4],
                            /* (Wild) Water [int] */ item.BaseStats[5],
                            /* (Wild) Temperature [int] */ item.BaseStats[6],
                            /* (Wild) Weight [int] */ item.BaseStats[7],
                            /* (Wild) MeleeDamageMultiplier [int] */ item.BaseStats[8],
                            /* (Wild) SpeedMultiplier [int] */ item.BaseStats[9],
                            /* (Wild) TemperatureFortitude [int] */ item.BaseStats[10],
                            /* (Wild) CraftingSpeedMultiplier [int] */ item.BaseStats[11],
                            /* (Domesticated) Health [int] */ item.TamedStats[0],
                            /* (Domesticated) Stamina [int] */ item.TamedStats[1],
                            /* (Domesticated) Torpidity [int] */ item.TamedStats[2],
                            /* (Domesticated) Oxygen [int] */ item.TamedStats[3],
                            /* (Domesticated) Food [int] */ item.TamedStats[4],
                            /* (Domesticated) Water [int] */ item.TamedStats[5],
                            /* (Domesticated) Temperature [int] */ item.TamedStats[6],
                            /* (Domesticated) Weight [int] */ item.TamedStats[7],
                            /* (Domesticated) MeleeDamageMultiplier [int] */ item.TamedStats[8],
                            /* (Domesticated) SpeedMultiplier [int] */ item.TamedStats[9],
                            /* (Domesticated) TemperatureFortitude [int] */ item.TamedStats[10],
                            /* (Domesticated) CraftingSpeedMultiplier [int] */ item.TamedStats[11],
                            item.Colors[0],
                            item.Colors[1],
                            item.Colors[2],
                            item.Colors[3],
                            item.Colors[4],
                            item.Colors[5]
                          };


            var tempPath = TemporaryFileServices.GenerateFileName(".tsv");
            await new StoreTsvDataPipelineTask().Execute(records, tempPath);
            var publishFactory = new PublishFilePipelineTaskFactory();
            foreach (var uploadTarget in uploadTargets)
            {
                var uploadItems = new[]{ new UploadItem{
                    LocalPath = tempPath,
                    RemotePath = uploadTarget.RemoteTarget
                } };
                var task = publishFactory.Create(uploadItems, uploadTarget);
                await task.Execute(uploadItems, uploadTarget);
            }
            _ = new RemoveFilePipelineTask().Execute(tempPath);
        }

        private string CreateSpeciesBlueprintPath(ArkTamedCreature item)
        {
            if (!_mapping.TryGetValue(item.ClassName, out var data))
                return string.Empty;
            return data.BlueprintPath;
        }

        private string CreateSpeciesName(ArkTamedCreature item)
        {
            if (!_mapping.TryGetValue(item.ClassName, out var data))
                return string.Empty;
            return data.Name;
        }
    }
}
