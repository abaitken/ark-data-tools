namespace ArkDataProcessor
{
    internal abstract class ApplicationCommand
    {
        public abstract int Run(ICommandLineOptions options);
    }
}
