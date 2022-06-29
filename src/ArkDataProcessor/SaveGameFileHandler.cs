using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    class SaveGameFileHandler
    {
        private readonly ILogger<SaveGameFileHandler> _logger;
        private readonly MonitoringSource _configuration;
        private readonly List<SharedSetting> _sharedSettings;

        public SaveGameFileHandler(ILogger<SaveGameFileHandler> logger, MonitoringSource configuration, List<SharedSetting> sharedSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _sharedSettings = sharedSettings;
        }

        public void Process(string originalPath, IEnumerable<DataProcessingPipeline> pipelines)
        {
            var ext = Path.GetExtension(originalPath);
            var tempPath = TemporaryFileServices.GenerateFileName(ext);
            File.Copy(originalPath, tempPath);
            var data = new GameDataLoader().Load(tempPath);

            var tasks = new List<Task>();
            foreach (var pipeline in pipelines)
            {
                _logger.LogInformation($"Executing pipeline '{pipeline.Id}'");
                tasks.Add(pipeline.Execute(data, _configuration, _sharedSettings));
            }

            tasks.Add(new RemoveFilePipelineTask().Execute(tempPath));

            Task.WaitAll(tasks.ToArray());

        }
    }
}
