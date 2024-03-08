using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory360Web.Controllers
{
    [RoutePrefix("api")]
    public class AdvanceSearchController : Controller
    {
        // GET: AdvanceSearch
        public ActionResult AdvanceSearch()
        {
            return View();
        }      
    }
}