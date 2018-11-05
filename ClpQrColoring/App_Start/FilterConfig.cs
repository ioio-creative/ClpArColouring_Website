using System.Web.Mvc;

namespace ClpQrColoring
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // https://stackoverflow.com/questions/6508415/application-error-not-firing-when-customerrors-on
            //filters.Add(new HandleErrorAttribute());
        }
    }
}
