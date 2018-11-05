using ClpQrColoring.App_Start;
using ClpQrColoring.Utilities;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ClpQrColoring
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Default stuff
            AreaRegistration.RegisterAllAreas();

            // Manually installed WebAPI 2.2 after making an MVC project.
            // https://stackoverflow.com/questions/26067296/how-to-add-web-api-to-an-existing-asp-net-mvc-5-web-application-project
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Default stuff
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        // http://southworks.com/blog/2010/04/26/working-together-with-antiforgerytoken-and-outputcache-on-aspnet-mvc/
        //public override string GetVaryByCustomString(HttpContext context, string custom)
        //{
        //    if (custom.Equals("RequestVerificationTokenCookie", StringComparison.OrdinalIgnoreCase))
        //    {
        //        string verificationTokenCookieName =
        //            context.Request.Cookies
        //            .Cast<String>()
        //            .FirstOrDefault(cn => cn.StartsWith("__requestverificationtoken", StringComparison.InvariantCultureIgnoreCase));
        //        if (!String.IsNullOrEmpty(verificationTokenCookieName))
        //        {
        //            return context.Request.Cookies[verificationTokenCookieName].Value;
        //        }
        //    }

        //    return base.GetVaryByCustomString(context, custom);
        //}

        // https://msdn.microsoft.com/en-us/library/bb397417.aspx
        // https://stackoverflow.com/questions/20376682/application-error-handler-isnt-fired-when-use-prefix-async
        // Code that runs when an unhandled error occurs
        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the current HTTP request
            HttpRequest req = HttpContext.Current.Request;

            // Get the exceptio object
            Exception exc = Server.GetLastError();

            if (exc.GetType() == typeof(HttpException) &&
                (exc.Message.Contains("was not found or does not implement IController.") ||  // incorrect controller
                 exc.Message.Contains("was not found on controller")))  // incorrect action
            {
                // No need to take log in such cases
                return;
            }

            // Log the exception and notify system operators
            ExceptionUtilities.LogException(exc, req);            
            Task sendEmail = Task.Run(async () => await ExceptionUtilities.NotifySystemOps(exc, req));

            // Clear the error from the server
            //Server.ClearError();
            // Note: do not call Server.ClearError();
            // if called, exception will not be bubbled to <httpErrors> in web.config
        }
    }
}