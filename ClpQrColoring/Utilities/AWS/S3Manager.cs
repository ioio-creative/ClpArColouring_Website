using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Web.Configuration;

namespace ClpQrColoring.Utilities.AWS
{
    public class S3Manager
    {
        private static RegionEndpoint awsRegion = RegionEndpoint.APSoutheast1;
        private static string bucketName = WebConfigurationManager.AppSettings["AWSS3BucketName"];


        public static void UploadObject(string keyName, string localFilePath, string contentType,
            bool isPublicReadAllowed)
        {
            using (IAmazonS3 client = S3ClientFactory.CreateAmazonS3Client(awsRegion))
            {
                //Console.WriteLine("Uploading object " + keyName);
                UploadObject(client, keyName, localFilePath, contentType, isPublicReadAllowed);
            }

            //Console.WriteLine("Object " + keyName + " uploaded.");
        }

        // http://docs.aws.amazon.com/AmazonS3/latest/dev/UploadObjSingleOpNET.html
        public static void UploadObject(IAmazonS3 client, string keyName, string localFilePath,
            string contentType, bool isPublicReadAllowed)
        {
            try
            {
                // Put object-set ContentType and add metadata.
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    FilePath = localFilePath,
                    ContentType = contentType                   
                };

                if (isPublicReadAllowed)
                {
                    //https://stackoverflow.com/questions/7616810/amazon-s3-upload-with-public-permissions
                    putRequest.CannedACL = S3CannedACL.PublicRead;
                }

                //putRequest.Metadata.Add("x-amz-meta-title", "someTitle");

                PutObjectResponse response = client.PutObject(putRequest);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                ThrowFormattedAmazonS3Exception(amazonS3Exception, "writing an object");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string GeneratePreSignedUrl(string keyName, int lifeTimeOfUrlInMins, HttpVerb verb)
        {
            string urlString = "";

            using (IAmazonS3 client = S3ClientFactory.CreateAmazonS3Client(awsRegion))
            {
                //Console.WriteLine("Generating pre-signed URL for " + keyName);
                urlString = GeneratePreSignedUrl(client, keyName, lifeTimeOfUrlInMins, verb);
            }

            //Console.WriteLine("Pre-signed URL for " + keyName + ": " + urlString);

            return urlString;
        }

        // http://docs.aws.amazon.com/AmazonS3/latest/dev/ShareObjectPreSignedURLDotNetSDK.html
        public static string GeneratePreSignedUrl(IAmazonS3 client, string keyName,
            int lifeTimeOfUrlInMins, HttpVerb verb)
        {
            string urlString = "";

            GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = keyName,
                Expires = DateTime.Now.AddMinutes(lifeTimeOfUrlInMins),
                Verb = verb
            };

            try
            {
                urlString = client.GetPreSignedURL(request1);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                ThrowFormattedAmazonS3Exception(amazonS3Exception, "listing objects");                
            }
            catch (Exception ex)
            {
                throw(ex);
            }

            return urlString;
        }


        /* error handling */

        private static void ThrowFormattedAmazonS3Exception(AmazonS3Exception amazonS3Exception,
            string functionDesc)
        {
            string errMsg;

            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                ||
                amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {                
                errMsg = "Check the provided AWS Credentials." + Environment.NewLine +
                    "For service sign up go to http://aws.amazon.com/s3";
            }
            else
            {                
                errMsg = String.Format("Error occurred. Message:'{0}' when " + functionDesc,
                    amazonS3Exception.Message);
            }

            throw new Exception(errMsg);
        }

        /* end of error handling */
    }
}