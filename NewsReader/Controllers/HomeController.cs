using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsReader.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "News Reader.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Developer:";

            return View();
        }
    }
}