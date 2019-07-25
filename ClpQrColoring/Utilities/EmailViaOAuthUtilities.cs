using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ClpQrColoring.Config.Sections;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Security;
using ClpQrColoring.Globals;
using ClpQrColoring.Extensions;

namespace ClpQrColoring.Utilities
{
    public class EmailViaOAuthUtilities
    {
        private static MailViaGraphSection clpNonReplySetting = MailViaGraphSection.Section;            


        private static Recipient GetRecipient(string emailAddr)
        {
            return new Recipient
            {
                EmailAddress = new EmailAddress
                {
                    Address = emailAddr
                }
            };
        }

        private static IEnumerable<Recipient> GetRecipients(IEnumerable<string> emailAddrs)
        {
            return emailAddrs.Select(emailAddr => GetRecipient(emailAddr));            
        }


        /* writtend by clp */

        private static async Task<string> GetTokenAsync(MailViaGraphSection mailSetting)
        {
            IPublicClientApplication clientApp = PublicClientApplicationBuilder.Create(mailSetting.ClientId).WithAuthority(mailSetting.Authority).WithRedirectUri(mailSetting.ClientAppUri).Build();

            string[] scopes = { "Mail.ReadWrite", "Mail.Send" };
            string token = null;

            var securePassword = new SecureString();
            foreach (char c in mailSetting.FromPwd)
                securePassword.AppendChar(c);

            AuthenticationResult result = null;
            result = await clientApp.AcquireTokenByUsernamePassword(scopes, mailSetting.FromUser, securePassword).ExecuteAsync();
            token = result.AccessToken;
            return token;
        }

        private static async Task<string> SendByGraph(MailViaGraphSection mailSetting, string fromAddr, IEnumerable<string> toAddrs,
            string mailSubject, string mailContent)
        {            
            GraphServiceClient graphClient = new GraphServiceClient(mailSetting.GraphUrl, new DelegateAuthenticationProvider(async (requestMessage) =>
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", await GetTokenAsync(mailSetting));
            }));

            var message = new Message
            {
                From = GetRecipient(fromAddr),
                Subject = mailSubject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = mailContent
                },
                ToRecipients = GetRecipients(toAddrs)
            };

            await graphClient.Me
                .SendMail(message)
                .Request()
                .PostAsync();

            return "ok";
        }

        /* end of written by clp */


        public async static Task SendVideoCreatedNotificationAsync(string toEmailAddr, string newUserId)
        {
            await SendVideoCreatedNotificationAsync(new string[] { toEmailAddr }, newUserId);
        }

        public async static Task SendVideoCreatedNotificationAsync(IEnumerable<string> toEmailAddrs, string newUserId)
        {
            string fromEmailAddr = @"超人中中3D動畫 &lt;3dcolouring@clp.com.hk&gt;";
            string inquiryEmailAddr = @"powerkid@clp.com.hk";
            string mailSubject = "超人中中3D動畫填色遊戲";
            string mailContentFormat = @"<p>動畫已經準備好！立即點擊以下連結，觀看屬於你的超人中中3D動畫！記得同朋友分享，大家一齊慳電啦！</p>" +
                @"<p><a href='{0}'>{0}</p>" +
                @"<p><img alt='超人中中3D動畫填色遊戲' width='600px' src='{1}' /></p>" +
                @"<p>查詢電郵：<a href='mailto:" + inquiryEmailAddr + "'>" + inquiryEmailAddr + "</a></p>";

            try
            {
                // https://stackoverflow.com/questions/2031995/call-urlhelper-in-models-in-asp-net-mvc
                System.Web.Mvc.UrlHelper urlHelper = UrlUtilities.GetUrlHelperFromCurrentHttpContext();

                string imgInEmailSrc = "";
                if (SiteGlobal.IsLoadStaticResourceFromAwsS3)
                {
                    imgInEmailSrc = SiteGlobal.AWSS3StaticResourceBucketDomain + "Public/images/Latest/Sharing/PowerKidLong_compressed.jpg";
                }
                else
                {
                    imgInEmailSrc = urlHelper.Content(true, "~/Public/images/Latest/Sharing/PowerKidLong_compressed.jpg");
                }
                
                string mailContent = String.Format(mailContentFormat,
                    urlHelper.Action(true, "Details", new { id = newUserId }),
                    imgInEmailSrc);

                await SendByGraph(clpNonReplySetting, fromEmailAddr, toEmailAddrs, mailSubject, mailContent);             
            }
            catch (Exception ex)
            {
                // TODO: Silence exception when sending email?
                //Console.WriteLine("Could not send video created notification e-mail." + Environment.NewLine + 
                //    "Exception caught: " + ex);
                await ExceptionUtilities.NotifySystemOps(ex, HttpContext.Current.Request);
            }
        }
    }
}