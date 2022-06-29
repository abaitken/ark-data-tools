using System;
using System.Diagnostics;

namespace ArkDataProcessor
{
    static class ProcessExtensions
    {
        public static bool WaitForExit(this Process process, TimeSpan timeout)
        {
            return process.WaitForExit((int)timeout.TotalMilliseconds);
        }

        public static bool WaitForExit(this Process process, TimeSpan? timeout)
        {
            if (timeout.HasValue)
                return process.WaitForExit((int)timeout.Value.TotalMilliseconds);
            process.WaitForExit();
            return true;
        }
    }
}