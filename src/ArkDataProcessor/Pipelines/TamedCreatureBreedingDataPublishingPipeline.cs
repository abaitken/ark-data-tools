using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class TamedCreatureBreedingDataPublishingPipeline : DataProcessingPipeline
    {
        internal override async void Execute(ArkGameData data, Configuration configuration)
        {
            var uploadTarget = configuration.UploadTargets.FirstOrDefault(i => i.Id.Equals("tamed_breeding_data"));
            if (uploadTarget == null)
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

            var records = from item in tamedCreatures
                          select new object[]
                          {
                            /* speciesName */ CreateSpeciesName(item),
                            /* speciesBlueprintPath */ CreateSpeciesBlueprintPath(item),
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
            await new StoreTsvDataPipelineTask().ExecuteAsync(records, tempPath);
            await new PublishFilePipelineTask().ExecuteAsync(tempPath, uploadTarget);
            _ = new RemoveFilePipelineTask().ExecuteAsync(tempPath);
        }

        private string CreateSpeciesBlueprintPath(ArkTamedCreature item)
        {
            return item.ClassName;
        }

        private string CreateSpeciesName(ArkTamedCreature item)
        {
            return string.Empty;
        }
    }
}
