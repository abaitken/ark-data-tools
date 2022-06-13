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
                var fileHandler = new SaveGameFileHandler(loggerFactory, monitoringSource);
                fileHandler.Process(monitoringSource.FilePath, pipelines);
            }

            return ExitCodes.OK;
        }
    }
}
