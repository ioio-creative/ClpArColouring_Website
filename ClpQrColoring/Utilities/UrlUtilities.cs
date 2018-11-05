using System.Web;
using System.Web.Mvc;

namespace ClpQrColoring.Utilities
{
    public class UrlUtilities
    {
        public static UrlHelper GetUrlHelperFromCurrentHttpContext()
        {
            return new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public static UrlHelper GetUrlHelperFromControllerContext(Controller ctrl)
        {
            return new UrlHelper(ctrl.ControllerContext.RequestContext);
        }
    }
}