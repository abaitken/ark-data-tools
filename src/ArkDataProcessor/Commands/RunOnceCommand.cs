using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    internal class RunOnceCommand : ApplicationCommand
    {
        public override int Run(ICommandLineOptions options)
        {
            var loggerFactory = LoggerFactory.Create(builder => {
                builder.AddFilter("Microsoft", LogLevel.Warning)
                       .AddFilter("System", LogLevel.Warning)
                       .AddFilter("ArkDataProcessor", LogLevel.Debug)
                       .AddConsole();
            }
            );

            var configuration = new ConfigurationReader().Load(options.ConfigurationFile);
            new ConfigurationValidation(loggerFactory.CreateLogger<ConfigurationValidation>()).Validate(configuration);


            var factory = new DataProcessingPipelineFactory();
            var pipelines = factory.CreatePipelines();

            foreach (var monitoringSource in configuration.MonitoringSources)
            {
                var logger = loggerFactory.CreateLogger<SaveGameFileHandler>();
                var fileHandler = new SaveGameFileHandler(logger, monitoringSource, configuration.SharedSettings);

                try
                {
                    fileHandler.Process(monitoringSource.FilePath, pipelines);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An exception was thrown whilst processing pipelines for a monitoring source");
                }
            }

            return ExitCodes.OK;
        }
    }
}
