using System.Reflection;

namespace ArkDataProcessor
{
    internal class App
    {
        internal int Run(string[] args)
        {
            var options = new CommandLineOptionsBuilder().Build(args);

            if (!options.NoLogo)
                DisplayLogo();

            var task = CreateTask(options);
            return task.Run(options);
        }

        private ApplicationCommand CreateTask(ICommandLineOptions options)
        {
            if (options == null)
                return new LaunchServiceCommand();

            if (options.Help)
                return new DisplayHelpCommand();

            if (options.InstallService)
                return new InstallServiceCommand();

            if (options.UninstallService)
                return new UninstallServiceCommand();

            return new LaunchServiceCommand();
        }

        private void DisplayLogo()
        {
            var assemblyInfo = new AssemblyInformation(Assembly.GetExecutingAssembly());
            Console.WriteLine($@"{assemblyInfo.Product} {assemblyInfo.InformationalVersion} {assemblyInfo.Configuration}
{assemblyInfo.Copyright} {assemblyInfo.Trademark}");
        }

    }
}
