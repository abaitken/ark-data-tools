using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    class SaveGameFileHandler
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Configuration _configuration;

        public SaveGameFileHandler(ILoggerFactory loggerFactory, Configuration configuration)
        {
            _loggerFactory = loggerFactory;
            _configuration = configuration;
        }

        public void Process(string originalPath)
        {
            var ext = Path.GetExtension(originalPath);
            var tempPath = TemporaryFileServices.GenerateFileName(ext);
            File.Copy(originalPath, tempPath);
            var data = new GameDataLoader().Load(tempPath);

            var factory = new DataProcessingPipelineFactory(_configuration);

            foreach (var pipeline in factory.CreatePipelines())
                pipeline.ExecuteAsync(data, _configuration);

            new RemoveFilePipelineTask().ExecuteAsync(tempPath);
        }
    }
}
