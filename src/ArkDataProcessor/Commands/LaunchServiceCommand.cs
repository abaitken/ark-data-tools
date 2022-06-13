using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    internal class LaunchServiceCommand : ApplicationCommand
    {
        public override int Run(ICommandLineOptions options)
        {
            using IHost host = Host.CreateDefaultBuilder(Array.Empty<string>())
               .UseWindowsService(options =>
               {
                   options.ServiceName = "ARK Data Processing Service";
               })
               .ConfigureServices(services =>
               {
                   services.AddHostedService<FileMonitoringService>((provider) =>
                   {
                       var loggerFactory = provider.GetService<ILoggerFactory>();
                       if (loggerFactory == null)
                           throw new NullReferenceException($"The value of '{nameof(loggerFactory)}' is null where an object instance was expected");
                       var configuration = new ConfigurationReader().Load(options.ConfigurationFile);
                       new ConfigurationValidation(loggerFactory.CreateLogger<ConfigurationValidation>()).Validate(configuration);
                       return new FileMonitoringService(loggerFactory, loggerFactory.CreateLogger<FileMonitoringService>(), configuration);
                   });
               })
               .Build();

            host.Run();

            return ExitCodes.OK;
        }
    }
}
