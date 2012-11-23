using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HOPApiService.Models;

namespace HOPApiService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetHOPs()
        {
            var db = new DataContext();
            return Json(db.Hops.AsEnumerable(), JsonRequestBehavior.AllowGet);
        }
    }
}
