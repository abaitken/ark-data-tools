using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    sealed class FileMonitoringService : BackgroundService
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<FileMonitoringService> _logger;
        private readonly Configuration _configuration;

        public FileMonitoringService(ILoggerFactory loggerFactory, ILogger<FileMonitoringService> logger, Configuration configuration)
        {
            _loggerFactory = loggerFactory;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var longWait = TimeSpan.FromSeconds(_configuration.LongDelay);
            var shortWait = TimeSpan.FromSeconds(_configuration.ShortDelay);
            var fileServices = _configuration.MonitoringSources.Select(v => new FileService(v.FilePath)).ToList();
            var lastModifieds = fileServices.Select(v => v.GetLastModified()).ToList();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking for updated file...");
                var queue = new Queue<int>(Enumerable.Range(0, lastModifieds.Count));

                while(!(queue.Count == 0 || stoppingToken.IsCancellationRequested))
                {
                    var index = queue.Dequeue();
                    var fileService = fileServices[index];
                    var configuration = _configuration.MonitoringSources[index];
                    var lastModified = lastModifieds[index];

                    var modifiedTime = fileService.GetLastModified();
                    var action = DetermineServiceAction(lastModified, modifiedTime);

                    if (action == ServiceAction.Wait)
                        continue;

                    if (action == ServiceAction.Execute && !fileService.CanRead())
                        action = ServiceAction.Retry;

                    if (action == ServiceAction.Retry)
                    {
                        _logger.LogInformation("File not ready. Retrying shortly...");
                        await Task.Delay(shortWait, stoppingToken);
                        queue.Enqueue(index);
                        continue;
                    }

                    if (action == ServiceAction.Execute)
                    {
                        _logger.LogInformation("File has changed. Executing pipelines...");
                        var fileHandler = new SaveGameFileHandler(_loggerFactory.CreateLogger<SaveGameFileHandler>(), configuration, _configuration.SharedSettings);
                        var factory = new DataProcessingPipelineFactory();
                        fileHandler.Process(configuration.FilePath, factory.CreatePipelines());
                        lastModified = modifiedTime;
                        lastModifieds[index] = modifiedTime;
                        continue;
                    }

                    throw new InvalidOperationException($"Unexpected action: {action}");
                }

                _logger.LogInformation("Waiting...");
                await Task.Delay(longWait, stoppingToken);
            }
        }

        private ServiceAction DetermineServiceAction(DateTime? last, DateTime? current)
        {
            // If file is now missing
            if (!current.HasValue)
                // Was it present
                if(last.HasValue)
                    return ServiceAction.Retry; // Retry, it might be back
                else
                    return ServiceAction.Wait; // Wait longer

            if (!last.HasValue)
                return ServiceAction.Execute;

            if (last.Value < current.Value)
                return ServiceAction.Execute;

            return ServiceAction.Wait;
        }

        enum ServiceAction
        {
            Wait,
            Retry,
            Execute
        }

        class FileService
        {
            private readonly string _filePath;

            public FileService(string filePath)
            {
                _filePath = filePath;
            }

            public DateTime? GetLastModified()
            {
                if (!File.Exists(_filePath))
                    return null;
                return File.GetLastWriteTime(_filePath);
            }

            public bool CanRead()
            {
                // TODO : Implement properly! Check can read, just in case we are mid-read... 
                return true;
            }
        }
    }
}
