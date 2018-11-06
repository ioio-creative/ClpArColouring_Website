using ClpQrColoring.Globals;
using ClpQrColoring.Utilities;
using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
using System.Linq;

namespace ClpQrColoring.DataAccess
{
    public class ColorAppDataAccess
    {
        public const int RequiredPrimaryKeyLength = 25;

        private static Random RandObj = new Random();
        private const string CharCollection = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const string LongTimeStampFormat = "{0:yyyyMMddHHmmssffff}";


        public static string InsertUser(Models.ArCharacters.User userView, string videoOutputExtension,
            string videoThumbnailFileExtension, string awsS3BucketDomain,
            out string awsS3OrigImgKey, out string awsS3WarpedImgKey, 
            out string awsS3VideoKey, out string awsS3VideoThumbnailKey)
        {
            // Construct userData (DB entity) from userView (view model)
            DateTime newUserCreateDT;
            string newUserID = GenerateNewUserId(out newUserCreateDT);

            string imgFileExtension = FileUtilities.GetFileExtension(userView.PostedFile);

            DataAccess.User userData = new DataAccess.User()
            {
                ID = newUserID,
                CreateDT = newUserCreateDT,
                Email = userView.Email,

                // TODO: for now, the object key in S3 bucket would be 
                // "UserID/UserID.extension"
                // where UserID is ID in DB.
                AwsS3OrigImgKey = newUserID + "/" + newUserID +
                    FileUtilities.GetFileExtension(userView.PostedFile),
                AwsS3WarpedImgKey = newUserID + "/" + newUserID + SiteGlobal.WarpedImageFileNameSuffix +
                    imgFileExtension,
                AwsS3VideoKey = newUserID + "/" + newUserID +
                    videoOutputExtension,
                AwsS3VideoThumbnailKey = newUserID + "/" + newUserID + SiteGlobal.VideoThumbnailFileNameSuffix +
                    videoThumbnailFileExtension,
                AwsS3BucketDomain = awsS3BucketDomain,

                IsVideoCreated = false
            };

            awsS3OrigImgKey = userData.AwsS3OrigImgKey;
            awsS3WarpedImgKey = userData.AwsS3WarpedImgKey;
            awsS3VideoKey = userData.AwsS3VideoKey;
            awsS3VideoThumbnailKey = userData.AwsS3VideoThumbnailKey;

            using (ColorAppDataContext database = new ColorAppDataContext())
            {
                database.Users.InsertOnSubmit(userData);

                // Don't forget to submit changes
                database.SubmitChanges();
            }

            return newUserID;
        }

        public static void UpdateUserWarpingImageResult(string userId, int warpingImageErrorCode)
        {
            using (ColorAppDataContext database = new ColorAppDataContext())
            {                
                // TODO: error handling for IEnumerable.Single()??
                DataAccess.User user = database.Users.Where(u => u.ID == userId).Single();
                user.WarpImageErrorCode = warpingImageErrorCode;

                database.SubmitChanges();
            }
        }

        public static void UpdateUserRenderingVideoResult(string userId, bool isVideoGenerated)
        {
            using (ColorAppDataContext database = new ColorAppDataContext())
            {
                // TODO: error handling for IEnumerable.Single()??
                DataAccess.User user = database.Users.Where(u => u.ID == userId).Single();
                user.IsVideoCreated = isVideoGenerated;
                
                database.SubmitChanges();
            }
        }


        public static Models.ArCharacters.User GetUserById(string id)
        {
            ColorAppDataContext database = new ColorAppDataContext();
            DataAccess.User userData = database.Users.Where(u => u.ID == id).FirstOrDefault();

            if (userData == null)
            {
                return null;
            }
            else
            {
                return new Models.ArCharacters.User()
                {
                    ID = userData.ID,
                    //Name = userData.Name,
                    Email = userData.Email,
                    AwsS3OrigImgKey = userData.AwsS3OrigImgKey,
                    AwsS3WarpedImgKey = userData.AwsS3WarpedImgKey,
                    AwsS3VideoKey = userData.AwsS3VideoKey,
                    AwsS3VideoThumbnailKey = userData.AwsS3VideoThumbnailKey,
                    AwsS3BucketDomain = userData.AwsS3BucketDomain
                };
            }
        }


        /* generating primary keys in database */

        private static string GenerateNewUserId(out DateTime createDT)
        {
            string newUserId = "U" + DateTimeString(out createDT);
            int lengthOfRandomSuffix = RequiredPrimaryKeyLength - newUserId.Length;
            return (lengthOfRandomSuffix > 0) ? newUserId + RandomString(lengthOfRandomSuffix) : newUserId;
        }

        // http://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings-in-c
        private static string RandomString(int length)
        {
            return new string(Enumerable.Repeat(CharCollection, length)
                .Select(s => s[RandObj.Next(s.Length)]).ToArray());
        }

        private static string DateTimeString(out DateTime createDT)
        {
            createDT = DateTime.Now;
            return String.Format(LongTimeStampFormat, createDT);
        }

        private static string DateTimeString()
        {
            DateTime tmpDT;
            return DateTimeString(out tmpDT);
        }

        /* end of generating primary keys in database */
    }
}