using System.Net;
using System.Web.Mvc;

namespace ClpQrColoring.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error   
        public ActionResult Index()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View();
        }

        // GET: Error/NotFound
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }

        // GET: Error/BadRequest
        public ActionResult BadRequest()
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return View();
        }
    }
}