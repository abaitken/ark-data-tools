using ArkSavegameToolkitNet.Domain;
using System.IO.Compression;

namespace ArkDataProcessor
{
    class RollingBackupPublishingPipeline : DataProcessingPipeline
    {
        const int keepNumber = 20;

        public override string Id => "rolling_backup";

        internal override async Task Execute(ArkGameData data, MonitoringSource configuration)
        {
            var uploadTargets = configuration.UploadTargets.Where(i => i.Id.Equals(Id)).ToList();
            if (uploadTargets.Count == 0)
                return;

            var tempPath = TemporaryFileServices.GenerateFileName(".zip");
            using (var archive = ZipFile.Open(tempPath, ZipArchiveMode.Create))
            {
                foreach (var file in SelectFilesToBackup(configuration.FilePath))
                    archive.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Fastest);
            }

            var destinationFileName = $"{DateTime.Now:yyyy-dd-MM-HH-mm-ss}.zip";
            foreach (var target in uploadTargets)
            {
                var destinationFolder = target.RemoteTarget;

                var backups = (from path in Directory.GetFiles(destinationFolder, "*.zip")
                              let info = new FileInfo(path)
                              orderby info.LastWriteTime
                              select path).ToList();
                while(backups.Count >= keepNumber)
                {
                    _ = new RemoveFilePipelineTask().Execute(backups[0]);
                    backups.RemoveAt(0);
                }

                var destinationPath = Path.Combine(destinationFolder, destinationFileName);

                var uploadItems = new[]{ new UploadItem{
                    LocalPath = tempPath,
                    RemotePath = destinationPath
                } };
                await new LocalCopyFilePipelineTask().Execute(uploadItems, target);
            }
            _ = new RemoveFilePipelineTask().Execute(tempPath);
        }

        private IEnumerable<string> SelectFilesToBackup(string filePath)
        {
            yield return filePath;

            var saveFolder = Path.GetDirectoryName(filePath);
            if (string.IsNullOrWhiteSpace(saveFolder))
                throw new InvalidOperationException($"The value of '{nameof(saveFolder)}' is null or empty where a valid value was expected");

            var filters = new[] { "*.arktribe", "*.arkprofile", "*.arktributetribe" };
            foreach (var filter in filters)
            {
                foreach (var file in Directory.EnumerateFiles(saveFolder, filter))
                {
                    yield return file;
                }
            }
        }
    }
}
