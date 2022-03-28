namespace ArkDataProcessor
{
    internal class UninstallServiceCommand : ApplicationCommand
    {
        public override int Run(ICommandLineOptions options)
        {
            Console.WriteLine("NOT IMPLEMENTED");
            return ExitCodes.RemovalFailed;
        }
    }
}
