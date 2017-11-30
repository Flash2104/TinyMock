using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : BaseWebController
    {
        public ActionResult SinglePage()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "index.html");
            return new FilePathResult(path, "text/html");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}