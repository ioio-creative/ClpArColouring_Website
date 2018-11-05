using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ClpQrColoring.Extensions
{
    public static class UrlExtensions
    {
        private static bool IsUseHttpsInFullUrlLink = bool.Parse(
            WebConfigurationManager.AppSettings["IsUseHttpsInFullUrlLink"]);

        public static string Content(this UrlHelper urlHelper, bool toAbsolute)
        {
            return Content(urlHelper, toAbsolute, urlHelper.Action());
        }

        public static string Content(this UrlHelper urlHelper, string contentPath)
        {
            return Content(urlHelper, false, urlHelper.Action());
        }

        // https://stackoverflow.com/questions/16084081/get-absolute-path-of-file-on-content
        public static string Content(this UrlHelper urlHelper, bool toAbsolute, string contentPath)
        {
            string relativeUri = urlHelper.Content(contentPath);

            if (toAbsolute)
            {
                Uri url = new Uri(HttpContext.Current.Request.Url, relativeUri);
                string absoluteUri = url.AbsoluteUri;
                if (IsUseHttpsInFullUrlLink)
                {
                    // TODO: hard-coded http and https here
                    absoluteUri = absoluteUri.Replace("http:", "https:");
                }
                return absoluteUri;
            }
            else
            {
                return relativeUri;
            }
        }

        /* bool toAbsolute is placed at the front of argument list to avoid ambiguity with object routeValues */

        public static string Action(this UrlHelper urlHelper, bool toAbsolute, string actionName, object routeValues = null)
        {
            return urlHelper.Content(toAbsolute, urlHelper.Action(actionName, routeValues));
        }

        public static string Action(this UrlHelper urlHelper, bool toAbsolute, string actionName, string controllerName, object routeValues = null)
        {
            return urlHelper.Content(toAbsolute, urlHelper.Action(actionName, controllerName, routeValues));
        }
    }
}

