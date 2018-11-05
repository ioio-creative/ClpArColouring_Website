using Amazon.S3;
using System.Web.Configuration;

namespace ClpQrColoring.Utilities.AWS
{
    public class S3ClientFactory
    {
        private static string AwsAccessKey = WebConfigurationManager.AppSettings["AWSAccessKey"];
        private static string AwsSecretKey = WebConfigurationManager.AppSettings["AWSSecretKey"];

        public static IAmazonS3 CreateAmazonS3Client(Amazon.RegionEndpoint awsRegion)
        {
            return new AmazonS3Client(AwsAccessKey, AwsSecretKey, awsRegion);
        }
    }
}