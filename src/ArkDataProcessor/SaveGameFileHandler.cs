using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    class SaveGameFileHandler
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly MonitoringSource _configuration;

        public SaveGameFileHandler(ILoggerFactory loggerFactory, MonitoringSource configuration)
        {
            _loggerFactory = loggerFactory;
            _configuration = configuration;
        }

        public void Process(string originalPath, IEnumerable<DataProcessingPipeline> pipelines)
        {
            var ext = Path.GetExtension(originalPath);
            var tempPath = TemporaryFileServices.GenerateFileName(ext);
            File.Copy(originalPath, tempPath);
            var data = new GameDataLoader().Load(tempPath);

            foreach (var pipeline in pipelines)
                pipeline.ExecuteAsync(data, _configuration);

            new RemoveFilePipelineTask().ExecuteAsync(tempPath);
        }
    }
}
