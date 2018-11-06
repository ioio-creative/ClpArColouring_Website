using ClpQrColoring.Globals;
using System.Collections.Generic;
using System;
using System.IO;
using System.Web;

namespace ClpQrColoring.Utilities
{
    public class FileUtilities
    {
        private static Dictionary<string, string> FileExtensionMimeTypeMap
            = new Dictionary<string, string>()
        {
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".mp4", "video/mp4" }
        };

        public static string GetFileExtension(HttpPostedFileBase postedFile)
        {
            return Path.GetExtension(postedFile.FileName);
        }

        public static string IdentifyMimeType(HttpPostedFileBase postedFile)
        {
            return IdentifyMimeType(postedFile.FileName);
        }

        public static string IdentifyMimeType(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            return FileExtensionMimeTypeMap[fileExtension.ToLower()];
        }

        public static string SavePostedFile(HttpPostedFileBase postedFile)
        {
            return SavePostedFile(postedFile, SiteGlobal.FileUploadDirectoryPath);
        }

        public static string SavePostedFile(HttpPostedFileBase postedFile, string uploadsDirPath)
        {
            // use name of posted file
            return SavePostedFile(postedFile, uploadsDirPath, Path.GetFileName(postedFile.FileName));
        }

        public static string SavePostedFile(HttpPostedFileBase postedFile, string uploadsDirPath, string fileName)
        {
            string savedFileFullPath = Path.Combine(uploadsDirPath, fileName);

            if (!Directory.Exists(uploadsDirPath))
            {
                Directory.CreateDirectory(uploadsDirPath);
            }

            if (postedFile != null)
            {                
                postedFile.SaveAs(savedFileFullPath);
            }

            return savedFileFullPath;
        }        
    }
}