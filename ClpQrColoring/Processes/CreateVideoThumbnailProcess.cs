using ClpQrColoring.Globals;
using ClpQrColoring.Utilities;
using System;
using System.Diagnostics;

namespace ClpQrColoring.Processes
{
    public class CreateVideoThumbnailProcess
    {
        // https://trac.ffmpeg.org/wiki/Create%20a%20thumbnail%20image%20every%20X%20seconds%20of%20the%20video
        // Output a single frame from the video into an image file:
        // ffmpeg -i input.flv -ss 00:00:14.435 -vframes 1 out.png
        private static string ffmpegExePath =
            SiteGlobal.FfmpegExePath;
        private const string createVideoThumbnailCmdArgumentsFormat =
            "-i \"{0}\" -ss {1} -vframes 1 \"{2}\"";
        private static string createVideoThumbnailWorkDir = "";


        // thumbnailTimePosition format: 00:00:14.435
        public static int StartCreateVideoThumbnailProcess(string inVideoFilePath, 
            string thumbnailTimePosition, string outImgFilePath)
        {
            int exitCode = 0;

            string createVideoThumbnailCmdArguments =
                String.Format(createVideoThumbnailCmdArgumentsFormat,
                    inVideoFilePath, thumbnailTimePosition, outImgFilePath);

            ProcessStartInfo startInfo = ProcessUtilities.CreateProcessStartInfo(ffmpegExePath,
                createVideoThumbnailCmdArguments, createVideoThumbnailWorkDir);

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