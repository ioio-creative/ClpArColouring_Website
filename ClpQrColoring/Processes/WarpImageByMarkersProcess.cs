using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using ClpQrColoring.Utilities;
using ClpQrColoring.Globals;

namespace ClpQrColoring.Processes
{
    public class WarpImageByMarkersProcess
    {
        private static string warpImageByMarkersExePath = SiteGlobal.WarpImageByMarkersExePath;
        private const string warpImageByMarkersCmdArgumentsFormat =
            "\"{0}\" \"{1}\"";  // add quotes around tstrhe argument values in case the values contain space
        private static string warpImageByMarkersWorkDir = "";  // working directory same as where this process is called?


        public static int StartWarpImageByMarkersProcess(string inImgFilePath,
            string outImgFilePath)
        {
            int exitCode = 0;

            string warpImageByMarkersCmdArguments = String.Format(warpImageByMarkersCmdArgumentsFormat,
                inImgFilePath, outImgFilePath);

            ProcessStartInfo startInfo = ProcessUtilities.CreateProcessStartInfo(warpImageByMarkersExePath,
                warpImageByMarkersCmdArguments, warpImageByMarkersWorkDir);

            using (Process proc = Process.Start(startInfo))
            {
                ProcessUtilities.OutputFromProcess(proc);

                // !!! Important !!!
                // wait for process to exit
                proc.WaitForExit();
                exitCode = proc.ExitCode;
            }

            return exitCode;
        }
    }
}