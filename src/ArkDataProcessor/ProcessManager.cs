using System;
using System.Diagnostics;

namespace ArkDataProcessor
{
    class ProcessManager
    {
        public int? Run(string workingDirectory, string commandLine, string program, TimeSpan? timeout, bool shellExecute = false)
        {
            var startInfo = !string.IsNullOrWhiteSpace(commandLine)
                                ? new ProcessStartInfo(program, commandLine)
                                : new ProcessStartInfo(program);
            if (!string.IsNullOrWhiteSpace(workingDirectory))
                startInfo.WorkingDirectory = workingDirectory;

            startInfo.UseShellExecute        = shellExecute;
            if (!shellExecute)
            {
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardOutput = true;
            }

            using (var process = new Process
                                 {
                                     StartInfo = startInfo
                                 })
            {
                if (!shellExecute)
                {
                    process.OutputDataReceived += Process_OutputDataReceived;
                    process.ErrorDataReceived += Process_ErrorDataReceived;
                }

                process.Start();

                if (!shellExecute)
                    process.BeginOutputReadLine();

                var exited = process.WaitForExit(timeout);
                if (!exited)
                    process.Kill();
                var exitCode = exited ? process.ExitCode : (int?)null;

                process.Close();
                return exitCode;
            }
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                Console.Error.WriteLine(e.Data);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                Console.Out.WriteLine(e.Data);
        }
    }
}