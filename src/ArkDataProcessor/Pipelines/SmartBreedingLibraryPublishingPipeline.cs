using ArkDataProcessor.Extensions;
using ArkSavegameToolkitNet.Domain;

namespace ArkDataProcessor
{
    class SmartBreedingLibraryPublishingPipeline : DataProcessingPipeline
    {
        public override string Id => "import_smart_breeding_library";

        internal override async Task Execute(ArkGameData data, MonitoringSource configuration, List<SharedSetting> sharedSettings)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals(Id)).ToList();
            if (uploadTargets.Count == 0)
                return;

            var arkStatsExtractorPath = sharedSettings.GetValueOrDefault<string>("ARKStatsExtractorPath");
            if (string.IsNullOrEmpty(arkStatsExtractorPath) || !File.Exists(arkStatsExtractorPath))
                return;

            var timeout = (int)Convert.ChangeType(sharedSettings.GetValueOrDefault<object>("ARKStatsExtractorTimeout"), typeof(int));
            if (timeout < 1)
                return;

            var tempPaths = new List<string>();
            var filename = Path.GetFileName(configuration.FilePath);
            var tempArkFolder = TemporaryFileServices.GenerateFolderName();
            Directory.CreateDirectory(tempArkFolder);
            var tempArkPath = Path.Combine(tempArkFolder, filename);
            File.Copy(configuration.FilePath, tempArkPath);


            var publishFactory = new PublishFilePipelineTaskFactory();
            foreach (var uploadTarget in uploadTargets)
            {
                var serverName = uploadTarget.Custom?.Value<string>("ServerName");
                var filePath = string.IsNullOrWhiteSpace(serverName) ? tempArkPath : serverName + ";" + tempArkPath;

                var tempPath = TemporaryFileServices.GenerateFileName(".asb");
                tempPaths.Add(tempPath);

                var process = new ProcessManager();
                var exitCode = process.Run(string.Empty, $@"-g ""{filePath}"" -o ""{tempPath}""", arkStatsExtractorPath, TimeSpan.FromSeconds(timeout));

                if (!exitCode.HasValue || exitCode.Value != 0)
                    continue;

                var uploadItems = new[]{ new UploadItem{
                    LocalPath = tempPath,
                    RemotePath = uploadTarget.RemoteTarget
                } };
                var task = publishFactory.Create(uploadItems, uploadTarget);
                await task.Execute(uploadItems, uploadTarget);
            }

            foreach (var tempPath in tempPaths)
                _ = new RemoveFilePipelineTask().Execute(tempPath);

            await new RemoveFilePipelineTask().Execute(tempArkPath);
            try
            {
                Directory.Delete(tempArkFolder, true);
            }
            catch (Exception)
            {
            }
        }
    }
}
