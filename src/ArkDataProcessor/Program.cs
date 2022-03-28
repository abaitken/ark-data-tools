using System;
using System.Diagnostics;

namespace ArkDataProcessor
{
    static class Program
    {
        static int Main(string[] args)
        {
            var exitCode = new App().Run(args);
#if DEBUG
            if (Environment.UserInteractive && Debugger.IsAttached)
            {
                Console.WriteLine("Debuggeer attached. Press any key to exit");
                Console.ReadKey();
            }
#endif
            return exitCode;
        }

    }
}
