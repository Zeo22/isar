using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISAR.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: Index
        public ActionResult Index(string code)
        {
            ViewBag.Code = code;
            return View();
        }
    }
}