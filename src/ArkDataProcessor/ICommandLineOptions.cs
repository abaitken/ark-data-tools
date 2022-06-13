namespace ArkDataProcessor
{
    public interface ICommandLineOptions
    {
        string ConfigurationFile { get; }
        bool Help { get; }
        bool NoLogo { get; }
        bool InstallService { get; }
        bool UninstallService { get; }
        string? ServiceName { get; }
        string? DisplayName { get; }
        bool UseUserAccount { get; }
        bool LogToConsole { get; }
        bool DumpKeys { get; }
        bool RunOnce { get; }
    }
}
