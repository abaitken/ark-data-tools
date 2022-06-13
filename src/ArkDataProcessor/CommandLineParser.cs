namespace ArkDataProcessor
{
    internal class CommandLineParser
    {
        internal IDictionary<string, string?> Parse(string[] args)
        {
            var result = new Dictionary<string, string?>();

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                string key;
                string? value = null;

                if(arg.StartsWith('/') || arg.StartsWith('-'))
                {
                    key = arg[1..];

                    i++;
                    if(i < args.Length)
                    {
                        var nextArg = args[i];
                        var nextArgIsSwitch = nextArg.StartsWith('/') || nextArg.StartsWith('-');

                        if (!nextArgIsSwitch)
                            value = nextArg;
                        else
                            i--;
                    }
                }
                else
                {
                    key = string.Empty;
                    value = arg;
                }

                if (!result.ContainsKey(key))
                    result.Add(key, value);
                else
                    result[key] = value;
            }

            return result;
        }
    }
}