using FileHelperDLL;
using System;
using System.Web;
using System.Web.Configuration;


namespace ClpQrColoring.Globals
{
    public class SiteGlobal
    {
        private const string LongTimeStampFormat = "{0:yyyyMMddHHmmssffff}";


        /* enabling / disabling listening to ArCharacters Create request */

        private static bool _isListeningToArCharactersCreateRequest = true;
        public static bool IsListeningToArCharactersCreateRequest
        {
            get
            {
                return _isListeningToArCharactersCreateRequest;
            }
        }

        private static void SetIsListeningToArCharactersCreateRequest(bool isListening)
        {
            _isListeningToArCharactersCreateRequest = isListening;
        }

        public static void EnableListeningToArCharactersCreateReqeust()
        {
            SetIsListeningToArCharactersCreateRequest(true);
        }

        public static void DisableListeningToCreateReqeust()
        {
            SetIsListeningToArCharactersCreateRequest(false);
        }

        /* end of enabling / disabling listening to ArCharacters Create request */


        /* web.config settings */

        public static string[] AllowedUploadFileExtensions { get; }    
        public static int NumOfSecsToStayInLandingPage { get; }
        public static string ErrorNotificationEmailRecipients { get; }

        // Local file paths
        public static string ErrorLogFilePath { get; }
        public static string FileUploadDirectoryPath { get; }
        public static string WarpedImageDirectoryPath { get; }
        public static string VideoFileDirectoryPath { get; }
        public static string VideoThumbnailDirectoryPath { get; }

        public static bool IsLoadStaticResourceFromAwsS3 { get; }
        public static string WebAppVirtualDirectoryName { get; }

        // AWS
        public static string AWSS3StaticResourceBucketDomain { get; }  // for storing .js, .css, website images
        public static string StaticResourceUrlPrefix { get; }

        public static string AwsS3BucketDomain { get; }  // for storing user created images, videos       

        // Blender process, OBSOLETE
        //public static string BlenderVideoExePath { get; }

        // VideoOutputExtension information still required -->
        public static string VideoOutputExtension { get; }
        public static int VideoOutputWidth { get; }
        public static int VideoOutputHeight { get; }

        // WarpImageByMarkers process
        public static string WarpImageByMarkersExePath { get; }
        public static string WarpedImageFileNameSuffix { get; }

        // CreateVideoThumbnail process
        public static string FfmpegExePath { get; }
        public static string VideoThumbnailTimePosition { get; }
        public static string VideoThumbnailFileNameSuffix { get; }
        public static string VideoThumbnailFileExtension { get; }

        public static string FacebookAppId { get; }
        public static string WebsiteKeywords { get; }

        /* end of web.config settings */


        static SiteGlobal()
        {
            AllowedUploadFileExtensions =
                WebConfigurationManager.AppSettings["AllowedUploadFileExtensions"]
                .Split(',');
            NumOfSecsToStayInLandingPage = int.Parse(
                WebConfigurationManager.AppSettings["NumOfSecsToStayInLandingPage"]);
            ErrorNotificationEmailRecipients =
                WebConfigurationManager.AppSettings["ErrorNotificationEmailRecipients"];

            ErrorLogFilePath = GetFullPath(
                WebConfigurationManager.AppSettings["ErrorLogFilePath"]);
            FileUploadDirectoryPath = GetFullPath(
                WebConfigurationManager.AppSettings["FileUploadDirectoryPath"]);
            WarpedImageDirectoryPath = GetFullPath(
                WebConfigurationManager.AppSettings["WarpedImageDirectoryPath"]);
            VideoFileDirectoryPath = GetFullPath(
                WebConfigurationManager.AppSettings["VideoFileDirectoryPath"]);
            VideoThumbnailDirectoryPath = GetFullPath(
                WebConfigurationManager.AppSettings["VideoThumbnailDirectoryPath"]);

            IsLoadStaticResourceFromAwsS3 = bool.Parse(
                WebConfigurationManager.AppSettings["IsLoadStaticResourceFromAwsS3"]);
            WebAppVirtualDirectoryName =
                WebConfigurationManager.AppSettings["WebAppVirtualDirectoryName"];
            
            AWSS3StaticResourceBucketDomain =
                WebConfigurationManager.AppSettings["AWSS3StaticResourceBucketDomain"];
            StaticResourceUrlPrefix = IsLoadStaticResourceFromAwsS3 ?
                AWSS3StaticResourceBucketDomain : (String.IsNullOrEmpty(WebAppVirtualDirectoryName)) ? "/" : "/" + WebAppVirtualDirectoryName + "/";

            AwsS3BucketDomain = WebConfigurationManager.AppSettings["AwsS3BucketDomain"];            

            //BlenderVideoExePath = GetFullPath(
            //    WebConfigurationManager.AppSettings["BlenderVideoExePath"]);
            VideoOutputExtension = WebConfigurationManager.AppSettings["VideoOutputExtension"];
            VideoOutputWidth = int.Parse(WebConfigurationManager.AppSettings["VideoOutputWidth"]);
            VideoOutputHeight = int.Parse(WebConfigurationManager.AppSettings["VideoOutputHeight"]);

            WarpImageByMarkersExePath = GetFullPath(
                WebConfigurationManager.AppSettings["WarpImageByMarkersExePath"]);
            WarpedImageFileNameSuffix = WebConfigurationManager.AppSettings["WarpedImageFileNameSuffix"];

            FfmpegExePath = WebConfigurationManager.AppSettings["FfmpegExePath"];
            VideoThumbnailTimePosition = WebConfigurationManager.AppSettings["VideoThumbnailTimePosition"];
            VideoThumbnailFileNameSuffix = WebConfigurationManager.AppSettings["VideoThumbnailFileNameSuffix"];
            VideoThumbnailFileExtension = WebConfigurationManager.AppSettings["VideoThumbnailFileExtension"];

            FacebookAppId = WebConfigurationManager.AppSettings["FacebookAppId"];
            WebsiteKeywords = WebConfigurationManager.AppSettings["WebsiteKeywords"];
        }

        private static string GetFullPath(string path)
        {
            return FileHelper.IsAbsolutePath(path) ?
                path : HttpContext.Current.Server.MapPath(path);
        }

        public static string LongTimeStampString()
        {
            return String.Format(LongTimeStampFormat, DateTime.Now);
        }
    }
}