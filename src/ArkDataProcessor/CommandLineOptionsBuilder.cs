namespace ArkDataProcessor
{
    class CommandLineOptionsBuilder
    {
        private class CommandLineOptions : ICommandLineOptions
        {
            public string ConfigurationFile { get; internal set; }
            public bool Help { get; internal set; }
            public bool NoLogo { get; internal set; }
            public bool InstallService { get; internal set; }
            public bool UninstallService { get; internal set; }
            public string? ServiceName { get; internal set; }
            public string? DisplayName { get; internal set; }
            public bool UseUserAccount { get; internal set; }
            public bool LogToConsole { get; internal set; }
        }

        public ICommandLineOptions Build(string[] args)
        {
            var commandLineParser = new CommandLineParser();
            var commandLineTokens = commandLineParser.Parse(args);

            var options = new CommandLineOptions
            {
                ConfigurationFile = commandLineTokens["c"].ThrowIfNullOrEmpty(),
                Help = commandLineTokens.ContainsKey("h"),
                NoLogo = commandLineTokens.ContainsKey("nologo"),
                InstallService = commandLineTokens.ContainsKey("install"),
                UninstallService = commandLineTokens.ContainsKey("uninstall"),
                ServiceName = commandLineTokens.GetValueOrDefault("sn"),
                DisplayName = commandLineTokens.GetValueOrDefault("dn"),
                UseUserAccount = commandLineTokens.ContainsKey("asuser"),
                LogToConsole = commandLineTokens.ContainsKey("console")
            };

            return options;
        }
    }
}
