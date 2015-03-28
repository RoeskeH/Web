using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuotationAppv1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "A quotation server programmed in MVC5";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Don't call us, we'll call you!";

            return View();
        }
    }
}