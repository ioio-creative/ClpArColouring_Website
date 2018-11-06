using System;
using System.Diagnostics;
using System.Linq;

namespace ClpQrColoring.Utilities
{
    public class ProcessUtilities
    {
        public static void OutputFromProcess(Process proc)
        {
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                if (!String.IsNullOrEmpty(line))
                {
                    Console.WriteLine(line);
                }
            }
        }

        public static bool IsProcessRunning(string processName)
        {
            Process[] correspondingProcesses = Process.GetProcessesByName(processName);
            return correspondingProcesses.Length > 0;
        }

        public static bool IsProcessRunningInBackground(string processName)
        {
            bool isProcessRunningInBackground = false;
            Process correspondingProcess = Process.GetProcessesByName(processName).FirstOrDefault();
            if (correspondingProcess != null)
            {
                isProcessRunningInBackground = IsRunningProcessInBackground(correspondingProcess);
            }
            return isProcessRunningInBackground;
        }

        public static bool IsProcessRunningButNotInBackground(string processName)
        {
            bool isProcessRunningButNotInBackground = false;
            Process correspondingProcess = Process.GetProcessesByName(processName).FirstOrDefault();
            if (correspondingProcess != null)
            {
                isProcessRunningButNotInBackground = !IsRunningProcessInBackground(correspondingProcess);             
            }
            return isProcessRunningButNotInBackground;
        }

        // assume process != null
        public static bool IsRunningProcessInBackground(Process process)
        {
            // https://stackoverflow.com/questions/39708184/net-how-to-check-if-a-windows-process-is-running-as-an-app-or-as-a-backgroun
            return process.MainWindowHandle == IntPtr.Zero;
        }

        public static ProcessStartInfo CreateProcessStartInfo(string exeFileName,
            string cmdArguments, string workingDirectory)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = exeFileName,
                Arguments = cmdArguments,
                WorkingDirectory = workingDirectory,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                RedirectStandardError = false
            };
            return startInfo;
        }

        // https://stackoverflow.com/questions/5255086/when-do-we-need-to-set-useshellexecute-to-true
        public static ProcessStartInfo CreateProcessStartInfoUseShellExecute(string exeFileName,
            string cmdArguments, string workingDirectory)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = exeFileName,
                Arguments = cmdArguments,
                WorkingDirectory = workingDirectory,
                CreateNoWindow = false,
                UseShellExecute = true,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false                
            };

            //startInfo.UserName = "IOIO";
            //string pw = "askmeioio";
            //foreach (char c in pw)
            //{
            //    startInfo.Password.AppendChar(c);
            //}

            return startInfo;
        }
    }
}