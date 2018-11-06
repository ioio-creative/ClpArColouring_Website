using ClpQrColoring.Extensions;
using ClpQrColoring.Globals;
using System;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ClpQrColoring.Utilities
{
    // https://www.mikesdotnetting.com/article/268/how-to-send-email-in-asp-net-mvc
    // https://stackoverflow.com/questions/32260/sending-email-in-net-through-gmail?rq=1    
    public class EmailUtilities
    {
        // https://stackoverflow.com/questions/4363038/setting-multiple-smtp-settings-in-web-config
        private static SmtpSection smtp_clp_noreply = WebConfigurationManager
            .GetSection("myMailSettings/smtp_clp_noreply") as SmtpSection;
        private static SmtpSection smtp_ioio_error = WebConfigurationManager
            .GetSection("myMailSettings/smtp_ioio_error") as SmtpSection;


        private static SmtpClient GetNewSmtpClientFromSmtpSectionConfig(SmtpSection smtpSec)
        {
            NetworkCredential credential = new NetworkCredential()
<<<<<<< HEAD
            {                
=======
            {
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
                UserName = smtpSec.Network.UserName,
                Password = smtpSec.Network.Password
            };

            return new SmtpClient()
<<<<<<< HEAD
            {                
=======
            {
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
                Credentials = credential,
                Host = smtpSec.Network.Host,
                Port = smtpSec.Network.Port,
                EnableSsl = smtpSec.Network.EnableSsl

                // The following statement does not get credentials
                // from web.config
                // Hence, it's completely wrong
                //smtp.UseDefaultCredentials = true;
            };
        }

        private static void AddMultipleRecipientsToMailMessage(MailMessage mailMessage, string receiverAddrs)
        {
            string[] receivers = receiverAddrs.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string receiver in receivers)
            {
                mailMessage.To.Add(receiver);
            }
        }

        // receiverAddrs can be comma separated list
        public async static Task SendVideoCreatedNotificationAsync(string receiverAddrs, string newUserId)
        {
            string inquiryEmailAddr = @"powerkid@clp.com.hk";
            string mailBodyFormat = @"<p>動畫已經準備好！立即點擊以下連結，觀看屬於你的超人中中3D動畫！記得同朋友分享，大家一齊慳電啦！</p>" +
                @"<p><a href='{0}'>{0}</p>" +
                @"<p><img alt='超人中中3D動畫填色遊戲' width='600px' src='{1}' /></p>" +
                @"<p>查詢電郵：<a href='mailto:" + inquiryEmailAddr + "'>" + inquiryEmailAddr + "</a></p>";

            try
            {
                // https://stackoverflow.com/questions/2031995/call-urlhelper-in-models-in-asp-net-mvc
                UrlHelper urlHelper = UrlUtilities.GetUrlHelperFromCurrentHttpContext();

                string imgInEmailSrc = "";
                if (SiteGlobal.IsLoadStaticResourceFromAwsS3)
                {
                    imgInEmailSrc = SiteGlobal.AWSS3StaticResourceBucketDomain + "Public/images/Latest/Sharing/PowerKidLong_compressed.jpg";
                }
                else
                {
                    imgInEmailSrc = urlHelper.Content(true, "~/Public/images/Latest/Sharing/PowerKidLong_compressed.jpg");
                }

                using (MailMessage message = new MailMessage())
                {
<<<<<<< HEAD
                    message.From = new MailAddress(smtp_clp_noreply.From);                                       
                    AddMultipleRecipientsToMailMessage(message, receiverAddrs);
                    message.Subject = "超人中中3D動畫填色遊戲";
                    message.Body = String.Format(mailBodyFormat, 
=======
                    message.From = new MailAddress(smtp_clp_noreply.From);
                    AddMultipleRecipientsToMailMessage(message, receiverAddrs);
                    message.Subject = "超人中中3D動畫填色遊戲";
                    message.Body = String.Format(mailBodyFormat,
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
                        urlHelper.Action(true, "Details", new { id = newUserId }),
                        imgInEmailSrc);
                    message.IsBodyHtml = true;

                    using (SmtpClient smtp = GetNewSmtpClientFromSmtpSectionConfig(smtp_clp_noreply))
                    {
                        await smtp.SendMailAsync(message);
                    }
<<<<<<< HEAD
                }                
=======
                }
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
            }
            catch (Exception ex)
            {
                // TODO: Silence exception when sending email?
                //Console.WriteLine("Could not send video created notification e-mail." + Environment.NewLine + 
                //    "Exception caught: " + ex);
                await ExceptionUtilities.NotifySystemOps(ex, HttpContext.Current.Request);
            }
        }

        public async static Task SendInternalErrorNotificationAsysnc(string receiverAddrs, Exception exc)
        {
            StringBuilder MessageBodyBuilder = new StringBuilder();
            MessageBodyBuilder.AppendFormat("********** {0} **********", DateTime.Now);
<<<<<<< HEAD
            MessageBodyBuilder.AppendLine("");                       
=======
            MessageBodyBuilder.AppendLine("");
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
            if (exc.InnerException != null)
            {
                MessageBodyBuilder.Append("Inner Exception Type: ");
                MessageBodyBuilder.AppendLine(exc.InnerException.GetType().ToString());
                MessageBodyBuilder.Append("Inner Exception: ");
                MessageBodyBuilder.AppendLine(exc.InnerException.Message);
                MessageBodyBuilder.Append("Inner Source: ");
                MessageBodyBuilder.AppendLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    MessageBodyBuilder.AppendLine("Inner Stack Trace: ");
                    MessageBodyBuilder.AppendLine(exc.InnerException.StackTrace);
                }
            }
            MessageBodyBuilder.Append("Exception Type: ");
            MessageBodyBuilder.AppendLine(exc.GetType().ToString());
            MessageBodyBuilder.AppendLine("Exception: " + exc.Message);
            MessageBodyBuilder.AppendLine("Source: " + exc.Source);
            MessageBodyBuilder.AppendLine("Stack Trace: ");
            if (exc.StackTrace != null)
            {
                MessageBodyBuilder.AppendLine(exc.StackTrace);
                MessageBodyBuilder.AppendLine();
            }
            string messageBody = MessageBodyBuilder.ToString();

            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(smtp_ioio_error.From);
                    AddMultipleRecipientsToMailMessage(message, receiverAddrs);
                    message.Subject = "超人中中3D動畫填色遊戲 Exception";
                    message.Body = messageBody;
                    message.IsBodyHtml = false;

                    using (SmtpClient smtp = GetNewSmtpClientFromSmtpSectionConfig(smtp_ioio_error))
                    {
                        await smtp.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Silence exception when sending email?
<<<<<<< HEAD
                Console.WriteLine("Could not send internal error notification e-mail." + Environment.NewLine + 
=======
                Console.WriteLine("Could not send internal error notification e-mail." + Environment.NewLine +
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
                    "Exception caught: " + ex);
            }
        }

<<<<<<< HEAD
        public async static Task SendInternalEventNotificationAsync(string receiverAddrs, 
=======
        public async static Task SendInternalEventNotificationAsync(string receiverAddrs,
>>>>>>> ed95342c00fa102189769cd333b3b349eb2c8a6a
            string eventName, string actionName, string newUserId)
        {
            StringBuilder MessageBodyBuilder = new StringBuilder();
            MessageBodyBuilder.AppendFormat("********** {0} **********", DateTime.Now);
            MessageBodyBuilder.AppendLine("");
            MessageBodyBuilder.Append("Event: ");
            MessageBodyBuilder.AppendLine(eventName);
            MessageBodyBuilder.Append("Action: ");
            MessageBodyBuilder.AppendLine(actionName);
            MessageBodyBuilder.Append("User ID: ");
            MessageBodyBuilder.AppendLine(newUserId);

            string messageBody = MessageBodyBuilder.ToString();

            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(smtp_ioio_error.From);
                    AddMultipleRecipientsToMailMessage(message, receiverAddrs);
                    message.Subject = "超人中中3D動畫填色遊戲 Event";
                    message.Body = messageBody;
                    message.IsBodyHtml = false;

                    using (SmtpClient smtp = GetNewSmtpClientFromSmtpSectionConfig(smtp_ioio_error))
                    {
                        await smtp.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Silence exception when sending email?
                Console.WriteLine("Could not send internal event notification e-mail." + Environment.NewLine +
                    "Exception caught: " + ex);
            }
        }
    }
}