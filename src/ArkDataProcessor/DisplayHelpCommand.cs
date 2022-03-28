namespace ArkDataProcessor
{
    internal class DisplayHelpCommand : ApplicationCommand
    {
        public override int Run(ICommandLineOptions options)
        {
            Console.WriteLine(@"
   -h                     Display this help
   -nologo                Hide logo on launch
   -console               Log to console
   -c <path>              Configuration file
   -install               Install as a service
   -uninstall             Uninstall service
   -dn <name>             Service display name
   -sn <name>             Service name
   -asuser                Install as current user

 Examples:
    Run from console:
    > arkdataprocessor -console -c ""config.json""

");
            return ExitCodes.OK;
        }
    }
}
