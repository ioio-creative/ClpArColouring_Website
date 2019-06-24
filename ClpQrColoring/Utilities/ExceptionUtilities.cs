using ClpQrColoring.Globals;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace ClpQrColoring.Utilities
{
    // https://msdn.microsoft.com/en-us/library/bb397417.aspx
    // Create our own utility for exceptions 
    public sealed class ExceptionUtilities
    {
        private static string ErrorLogFilePath = SiteGlobal.ErrorLogFilePath;
        private static string[] ErrorNotificationEmailRecipients = SiteGlobal.ErrorNotificationEmailRecipients;

        // All methods are static, so this can be private 
        private ExceptionUtilities()
        { }

        // Log an Exception 
        public static void LogException(Exception exc, HttpRequest request)
        {
            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(ErrorLogFilePath, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);
            if (exc.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exc.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.StackTrace);
                }
            }
            sw.Write("Exception Type: ");
            sw.WriteLine(exc.GetType().ToString());
            sw.WriteLine("Exception: " + exc.Message);
            sw.WriteLine("Source: " + exc.Source);
            sw.WriteLine("Stack Trace: ");
            if (exc.StackTrace != null)
            {
                sw.WriteLine(exc.StackTrace);
                sw.WriteLine();
            }
            sw.Close();
        }

        // Notify System Operators about an exception 
        public async static Task NotifySystemOps(Exception exc, HttpRequest request)
        {
            await EmailUtilities.SendInternalErrorNotificationAsync(
                ErrorNotificationEmailRecipients, exc);
        }
    }
}