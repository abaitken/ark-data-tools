﻿using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    internal class DumpKeysCommand : ApplicationCommand
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

            foreach (var monitoringSource in configuration.MonitoringSources)
                new SaveGameFileHandler(loggerFactory, monitoringSource)
                    .Process(monitoringSource.FilePath, 
                        new[] { new EntityKeysPublishingPipeline(EntityKeysPublishingPipeline.EntityKeysPublishingPipelineMode.Manual) }
                       );
            
            return ExitCodes.OK;
        }
    }
}