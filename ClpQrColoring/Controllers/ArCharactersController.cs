using ClpQrColoring.Globals;
using ClpQrColoring.Hubs;
using ClpQrColoring.Models.ArCharacters;
using ClpQrColoring.Processes;
using ClpQrColoring.Utilities;
using ClpQrColoring.Utilities.AWS;
using FileHelperDLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace ClpQrColoring.Controllers
{
    //[Authorize(Users = "admin@ioio.com.hk")]    
    public class ArCharactersController : Controller
    {
        private static string EventNotificationEmailRecipients = SiteGlobal.ErrorNotificationEmailRecipients;

        // GET: ArCharacters
<<<<<<< HEAD
        [AllowAnonymous]
        public ActionResult Index()
        {            
=======
        //[AllowAnonymous]
        public ActionResult Index()
        {
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
            ViewBag.SecsToStayInIndex = SiteGlobal.NumOfSecsToStayInLandingPage;
            return View();
        }

        // GET: ArCharacters/Download
        public ActionResult Download()
        {
            return View();
        }

        // GET: ArCharacters/Create
        // http://southworks.com/blog/2010/04/26/working-together-with-antiforgerytoken-and-outputcache-on-aspnet-mvc/
        //[OutputCache(
        //    Location = OutputCacheLocation.Client,
        //    Duration = 600,  // 10 minutes
        //    VaryByParam = "None",
        //    VaryByCustom = "RequestVerificationTokenCookie")]
        [OutputCache(
            Location = OutputCacheLocation.None,
            NoStore = true)]
        public ActionResult Create()
        {
            // user email may be stored 
            // if user comes back to the create page from the error info page
            string cachedEmail = TempData["Email"] as string;

            return View(new User()
            {
                Email = cachedEmail,
                ConfirmEmail = cachedEmail
            });
        }

        // POST: ArCharacters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // http://codeclimber.net.nz/archive/2011/12/07/Set-the-AsyncTimeout-attribute-for-your-async-controllers/
        // 12000000 ms = 12000 s = 200 min
        // When setting timeout, check also executionTimeout attribute in httpRuntime in web.config
        // https://msdn.microsoft.com/en-us/library/e1f13641(v=vs.100).aspx
        // Also check the Idle Timeout attribute of AWS Elastic Load Balancer.
        // https://aws.amazon.com/blogs/aws/elb-idle-timeout-control/
<<<<<<< HEAD
        [AsyncTimeout(12000000)]        
=======
        [AsyncTimeout(12000000)]
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
        public async Task<ActionResult> Create([Bind(Include = "Email, ConfirmEmail, PostedFile, SignalrHubConnectionId")] User user)
        {
            if (SiteGlobal.IsListeningToArCharactersCreateRequest)
            {
                if (ModelState.IsValid)
                {
                    // Note: no need to store user.SignalrHubConnectionId into database
                    // because the connectionId changes every time the page is refreshed/accessed via browser's back button
                    // so I think there is no much use to store it.
                    // SignalrHubConnectionId is used for calling client javascript function from server
                    // via SignalR's Hub API in the ClpQrColoring.Hubs.ImageToVideoClientCaller class                

                    /*
                    * Important:
                    * Now I return a new URL for client to go to by themselves
                    * because I use ajax post in client now
                    */
                    UrlHelper urlHelper = UrlUtilities.GetUrlHelperFromControllerContext(this);


                    // save user info in DB
                    string awsS3OrigImgKey;
                    string awsS3WarpedImgKey;
                    string awsS3VideoKey;
                    string awsS3VideoThumbnailKey;
                    string newUserId = DataAccess.ColorAppDataAccess.InsertUser(user,
                        SiteGlobal.VideoOutputExtension, SiteGlobal.VideoThumbnailFileExtension,
                        SiteGlobal.AwsS3BucketDomain, out awsS3OrigImgKey,
                        out awsS3WarpedImgKey, out awsS3VideoKey, out awsS3VideoThumbnailKey);

                    // save posted image file to local directory
                    string assignedOrigImgName = Path.GetFileName(awsS3OrigImgKey);
                    string savedOrigImgFullPath = FileUtilities.SavePostedFile(user.PostedFile,
                        SiteGlobal.FileUploadDirectoryPath, assignedOrigImgName);

                    // post original image file saved in local to AWS S3
                    S3Manager.UploadObject(awsS3OrigImgKey, savedOrigImgFullPath,
                        FileUtilities.IdentifyMimeType(savedOrigImgFullPath), true);


                    /* original image to warped image process */

                    // warp posted image                
                    string outputWarpedImageFileFullPath = Path.Combine(
                        SiteGlobal.WarpedImageDirectoryPath,
                        Path.GetFileName(awsS3WarpedImgKey));

                    int warpedImgProcExitCode = WarpImageByMarkersProcess.StartWarpImageByMarkersProcess(
                        savedOrigImgFullPath, outputWarpedImageFileFullPath);

                    // update warping image result in database
                    DataAccess.ColorAppDataAccess.UpdateUserWarpingImageResult(newUserId, warpedImgProcExitCode);

                    if (warpedImgProcExitCode == 0)  // success case
                    {
                        // post warped image to AWS S3
                        S3Manager.UploadObject(awsS3WarpedImgKey, outputWarpedImageFileFullPath,
                            FileUtilities.IdentifyMimeType(outputWarpedImageFileFullPath), true);
                    }
                    else  // fail case
                    {
                        FileHelper.DeleteFileSafe(savedOrigImgFullPath);

                        // store user email so that user does not have to enter email again
                        // after coming back to the create page from the error info page
                        TempData["Email"] = user.Email;

                        // TODO: warping image process error handling                    

                        //return RedirectToAction("ErrorInfo", new { errorCode = warpedImgProcExitCode });
<<<<<<< HEAD
                        return Content(urlHelper.Action("ErrorInfo", new { errorCode = warpedImgProcExitCode }));                        
=======
                        return Content(urlHelper.Action("ErrorInfo", new { errorCode = warpedImgProcExitCode }));
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
                    }

                    /* end of original image to warped image process */


                    /* warped image to video process */

                    // Important!!! todo

                    //// if no instance of BlenderTrial.exe is running, start one
                    //string blenderVideoExePath = SiteGlobal.BlenderVideoExePath;
                    //if (!ProcessUtilities.IsProcessRunningButNotInBackground(
                    //    Path.GetFileNameWithoutExtension(blenderVideoExePath)))
                    //{
                    //    // TODO: better to UseShellExecute to start the process
                    //    // otherwise, the process will be in the "background"???
                    //    // and the process won't be given priority to CPU resource access
                    //    ProcessStartInfo blenderProcInfo =
                    //        ProcessUtilities.CreateProcessStartInfoUseShellExecute(
                    //            blenderVideoExePath,
                    //            "",
                    //            Path.GetDirectoryName(blenderVideoExePath));  // working directory same as exe directory

                    //    Process.Start(blenderProcInfo);
                    //}


                    // call client function to show progress to user
                    ImageToVideoClientCaller.WhenVideoRendering(user.SignalrHubConnectionId);


                    // look for (or wait for) output video in output directory
                    // Path.GetFullPath() will make all the file separators changed to Windows default "\\"
                    string outputVideoPath = Path.Combine(
                        Path.GetFullPath(SiteGlobal.VideoFileDirectoryPath),
                        Path.GetFileName(awsS3VideoKey));

                    // Note: The output file may be of zero size
                    // indicating the Blender render process has run into error
                    // and has stopped retrying.
                    // In this case, the input file would also be deleted,
                    // hence needs to ask user to re-upload photo.

                    while (true)
                    {
<<<<<<< HEAD
                        Thread.Sleep(5000);                        
=======
                        Thread.Sleep(5000);
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a

                        if (System.IO.File.Exists(outputVideoPath))
                        {
                            break;
                        }

                        // call client function to show progress to user
                        //ImageToVideoClientCaller.WhenVideoRendering(user.SignalrHubConnectionId);
                    }

                    // assign sleep time to allow release of file handle by previous handler
                    Thread.Sleep(1000);

                    // check if outputVideoFileInfo is of zero size
                    if (FileHelper.IsFileSizeZero(outputVideoPath))
                    {
                        FileHelper.DeleteFileSafe(savedOrigImgFullPath);
                        FileHelper.DeleteFileSafe(outputVideoPath);

                        // TODO: send error notification email to user

                        //return RedirectToAction("ErrorInfo", new { errorCode = -1 });
                        return Content(urlHelper.Action("ErrorInfo", new { errorCode = -1 }));
                    }


                    // create thumbnail from output video
                    string videoThumbnailFilePath = Path.Combine(
                        SiteGlobal.VideoThumbnailDirectoryPath,
                        Path.GetFileName(awsS3VideoThumbnailKey));

                    int createVideoThumbnailProcExitCode = CreateVideoThumbnailProcess
                        .StartCreateVideoThumbnailProcess(
                            outputVideoPath,
                            SiteGlobal.VideoThumbnailTimePosition,
                            videoThumbnailFilePath);

                    if (createVideoThumbnailProcExitCode == 0)  // success case
                    {
                        S3Manager.UploadObject(awsS3VideoThumbnailKey, videoThumbnailFilePath,
                            FileUtilities.IdentifyMimeType(videoThumbnailFilePath), true);
                    }
                    else  // fail case
                    {
                        // TODO: handle ffmpeg create video thumbnail error case
                        // silence error?
                        // because video thumbnail is immaterial?
                        await EmailUtilities.SendInternalEventNotificationAsync(
                            EventNotificationEmailRecipients, "Failed when creating video thumbnail",
                            "POST: ArCharacters/Create", newUserId);
                    }

                    // assign sleep time to allow thumbnail creation process to release handle of video file
                    Thread.Sleep(1000);

                    // post output video file to AWS S3
                    // don't need to use Multipart Upload API
                    // because object size < 100 MB
                    // http://docs.aws.amazon.com/AmazonS3/latest/dev/uploadobjusingmpu.html
                    Action uploadVideoToAwsS3Func = () => S3Manager.UploadObject(awsS3VideoKey, outputVideoPath,
                        FileUtilities.IdentifyMimeType(outputVideoPath), true);

                    /*
                     * Note: 
                     * IOException may occur if 
                     * the process S3Manager.UploadObject() cannot access
                     * the local video file becauuse it is being used by another
                     * process.
                     * In such cases, I will retry once.
<<<<<<< HEAD
                     * Added by Christopher Wong, 2018-07-18
=======
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
                     */
                    try
                    {
                        uploadVideoToAwsS3Func();
                    }
                    catch (IOException ex)  // catch only IOException
                    {
                        await EmailUtilities.SendInternalEventNotificationAsync(
                            EventNotificationEmailRecipients, "Retrying upload video to AWS S3",
                            "POST: ArCharacters/Create", newUserId);

                        // retry once
                        Thread.Sleep(5000);
                        uploadVideoToAwsS3Func();
                    }

                    // update rendering video result in database
                    DataAccess.ColorAppDataAccess.UpdateUserRenderingVideoResult(newUserId, true);

                    // delete local files as they are uploaded to AWS S3 already
                    // no need to delete outputWarpedImageFileFullPath because it's deleted in BlenderTrial process
                    FileHelper.DeleteFileSafe(savedOrigImgFullPath);
                    //FileHelper.DeleteFileSafe(outputWarpedImageFileFullPath);
                    FileHelper.DeleteFileSafe(outputVideoPath);
                    FileHelper.DeleteFileSafe(videoThumbnailFilePath);

                    /* end of image to video process */


                    // Send email to user
                    //await EmailUtilities.SendVideoCreatedNotificationAsync(@"szewa.wong@connect.polyu.hk", newUserId);
                    await EmailUtilities.SendVideoCreatedNotificationAsync(user.Email, newUserId);


                    // https://stackoverflow.com/questions/1257482/redirecttoaction-with-parameter
                    //return RedirectToAction("Details", new { id = newUserId });
<<<<<<< HEAD
                    return Content(urlHelper.Action("Details", new { id = newUserId }));                   
=======
                    return Content(urlHelper.Action("Details", new { id = newUserId }));
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
                }
                else
                {
                    IEnumerable<ModelError> errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (ModelError error in errors)
                    {
                        Console.WriteLine(error);
                    }
                }

                return View(user);
            }
            else
            {
                // TODO: Server stops listening to create request! Error handling
                throw new Exception("Server stops listening to create request!");
            }
        }

        // GET: ArCharacters/Details/id
        // sharingCode:
        // needed as I want to use different open graph images in different cases
        // 0 = facebook
        // 1 = whatsapp
<<<<<<< HEAD
        [AllowAnonymous]
=======
        //[AllowAnonymous]
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
        public ActionResult Details(string id, int sharingCode = 0)
        {
            if (String.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

<<<<<<< HEAD
            User userView = 
=======
            User userView =
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
                DataAccess.ColorAppDataAccess.GetUserById(id);
            if (userView == null)
            {
                return HttpNotFound();
            }

            string videoUrl = userView.AwsS3BucketDomain + userView.AwsS3VideoKey;
            ViewBag.OGVideoObject = new _OpenGraphVideoPartialViewModel()
            {
                OGVideoUrl = videoUrl,
                OGVideoSecureUrl = videoUrl,
                OGVideoType = "video/" + FileHelper.RemoveDotFromExtensionIfExists(SiteGlobal.VideoOutputExtension),
                OGVideoWidth = SiteGlobal.VideoOutputWidth,
                OGVideoHeight = SiteGlobal.VideoOutputHeight
            };

            ViewBag.SharingCode = sharingCode;

            return View(userView);
        }

        // GET: ArCharacters/ErrorInfo
        // errorCode = 0 falls into general error case
        public ActionResult ErrorInfo(int errorCode = 0)
        {
            ErrorInfoViewModel errorInfo;

            // store user email so that user does not have to enter email again
            // after coming back to the create page from the error info page
            // calling TempData.Keep() will keep the stored data stay for another request
            TempData.Keep("Email");

            // TODO: warping image process error handling
            switch (errorCode)
            {
                case 3:  // top-left marker missing                    
                case 4:  // top-right marker missing                    
                case 5:  // bottom-left marker missing                    
                case 6:  // bottom-right marker missing                                        
                case 7:  // control marker missing or displaced
                    errorInfo = new ErrorInfoViewModel();
                    errorInfo.PageTitle = "相片錯誤";
                    errorInfo.ImgErrTipElementId = "imgPhotoErrTip";

                    if (SiteGlobal.IsLoadStaticResourceFromAwsS3)
                    {
                        errorInfo.ImgErrStatusSrc = SiteGlobal.StaticResourceUrlPrefix + "Public/images/Latest/Error/PhotoErrorTitle_1x.png";
                        errorInfo.ImgErrStatusSrcset = SiteGlobal.StaticResourceUrlPrefix + "Public/images/Latest/Error/PhotoErrorTitle_1x.png" + " 88w, " +
                            SiteGlobal.StaticResourceUrlPrefix + "Public/images/Latest/Error/PhotoErrorTitle_2x.png" + " 175w";
<<<<<<< HEAD
                        
=======

>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
                        errorInfo.ImgErrTipSrc = SiteGlobal.StaticResourceUrlPrefix + "Public/images/Latest/Error/PhotoErrorTip_1x.png";
                        errorInfo.ImgErrTipSrcset = SiteGlobal.StaticResourceUrlPrefix + "Public/images/Latest/Error/PhotoErrorTip_1x.png" + " 445w";
                    }
                    else
                    {
                        errorInfo.ImgErrStatusSrc = Url.Content("~/Public/images/Latest/Error/PhotoErrorTitle_1x.png");
                        errorInfo.ImgErrStatusSrcset = Url.Content("~/Public/images/Latest/Error/PhotoErrorTitle_1x.png") + " 88w, " +
                            Url.Content("~/Public/images/Latest/Error/PhotoErrorTitle_2x.png") + " 175w";

                        errorInfo.ImgErrTipSrc = Url.Content("~/Public/images/Latest/Error/PhotoErrorTip_1x.png");
                        errorInfo.ImgErrTipSrcset = Url.Content("~/Public/images/Latest/Error/PhotoErrorTip_1x.png") + " 445w";
                    }
<<<<<<< HEAD
                    
=======

>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
                    break;
                case 1:
                case 2:
                case -1:  // Blender rendering fails
                default:
                    errorInfo = new ErrorInfoViewModel();
                    errorInfo.PageTitle = "上載失敗";
                    errorInfo.ImgErrTipElementId = "imgUploadErrTip";

                    if (SiteGlobal.IsLoadStaticResourceFromAwsS3)
                    {
                        errorInfo.ImgErrStatusSrc = SiteGlobal.StaticResourceUrlPrefix + "Public/images/Latest/Error/UploadErrorTitle_1x.png";
<<<<<<< HEAD
                        errorInfo.ImgErrStatusSrcset = SiteGlobal.StaticResourceUrlPrefix +  "Public/images/Latest/Error/UploadErrorTitle_1x.png" + " 74w, " +
                            SiteGlobal.StaticResourceUrlPrefix + "Public/images/Latest/Error/UploadErrorTitle_2x.png" + " 148w";
                        
=======
                        errorInfo.ImgErrStatusSrcset = SiteGlobal.StaticResourceUrlPrefix + "Public/images/Latest/Error/UploadErrorTitle_1x.png" + " 74w, " +
                            SiteGlobal.StaticResourceUrlPrefix + "Public/images/Latest/Error/UploadErrorTitle_2x.png" + " 148w";

>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
                        errorInfo.ImgErrTipSrc = SiteGlobal.StaticResourceUrlPrefix + "Public/images/Latest/Error/UploadErrorTip_1x.png";
                        errorInfo.ImgErrTipSrcset = SiteGlobal.StaticResourceUrlPrefix + "Public/images/Latest/Error/UploadErrorTip_1x.png" + " 335w";
                    }
                    else
                    {
                        errorInfo.ImgErrStatusSrc = Url.Content("~/Public/images/Latest/Error/UploadErrorTitle_1x.png");
                        errorInfo.ImgErrStatusSrcset = Url.Content("~/Public/images/Latest/Error/UploadErrorTitle_1x.png") + " 74w, " +
                            Url.Content("~/Public/images/Latest/Error/UploadErrorTitle_2x.png") + " 148w";
<<<<<<< HEAD
                        
=======

>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
                        errorInfo.ImgErrTipSrc = Url.Content("~/Public/images/Latest/Error/UploadErrorTip_1x.png");
                        errorInfo.ImgErrTipSrcset = Url.Content("~/Public/images/Latest/Error/UploadErrorTip_1x.png") + " 335w";
                    }

                    break;
            }

            return View(errorInfo);
        }

        private static void ProduceError()
        {
            int x = 0;
            int y = 1 / x;
        }
    }
}