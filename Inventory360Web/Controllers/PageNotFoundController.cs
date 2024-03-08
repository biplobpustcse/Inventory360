using System.Web.Mvc;

namespace Inventory360Web.Controllers
{
    public class PageNotFoundController : Controller
    {
        // GET: PageNotFound
        public ActionResult Index()
        {
            return View();
        }
    }
}