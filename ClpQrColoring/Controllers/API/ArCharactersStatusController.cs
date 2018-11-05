using ClpQrColoring.Globals;
using FileHelperDLL;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace ClpQrColoring.Controllers.API
{
    public class ArCharactersStatusController : ApiController
    {
        private const string AuthCode = "ZpQrkvmSH5jgfpqPM3F9MCTc";


        private bool IsAuthorised(string authCode)
        {
            return AuthCode == authCode;
        }


        [HttpPost]
        // /api/ArCharactersStatus/UsersInQueueCount
        // return -1 for unauthorised callers
        public int UsersInQueueCount([FromBody] string authCode)
        {
            if (IsAuthorised(authCode))
            {
                IEnumerable<FileInfo> files =
                    FileHelper.GetFilesInDirectoryByExtensions(
                        SiteGlobal.WarpedImageDirectoryPath,
                        SiteGlobal.AllowedUploadFileExtensions);
                return files.Count();
            }            
            return -1;
        }

        [HttpPost]
        // /api/ArCharactersStatus/StopListeningToCreateRequests
        // do nothing for unauthorised callers
        public void StopListeningToCreateRequests([FromBody] string authCode)
        {
            if (IsAuthorised(authCode))
            {
                SiteGlobal.DisableListeningToCreateReqeust();
            }
        }

        [HttpPost]
        // /api/ArCharactersStatus/StopServerByUsersInQueueCount
        // return -1 for unauthorised callers
        public int StopServerByUsersInQueueCount([FromBody] string authCode)
        {
            int usersInQueueCount = UsersInQueueCount(authCode);
            if (usersInQueueCount == 0)
            {
                StopListeningToCreateRequests(authCode);
            }
            return usersInQueueCount;
        }
    }
}