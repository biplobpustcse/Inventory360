using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory360Web.Controllers
{
    [RoutePrefix("api")]
    public class RMAController : Controller
    {
        // GET: ComplainReceive
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ComplainReceive()
        {
            return View();
        }
        public ActionResult ReplacementClaim()
        {
            return View();
        }
        public ActionResult ReplacementReceive()
        {
            return View();
        }
        public ActionResult CustomerDelivery()
        {
            return View();
        }
    }
}