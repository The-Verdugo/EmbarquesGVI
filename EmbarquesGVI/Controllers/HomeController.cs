using EmbarquesGVI.Models.BD_A;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmbarquesGVI.Controllers
{
    public class HomeController : Controller
    {
        private readonly pruebasEmbarquesEntities db = new pruebasEmbarquesEntities();

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
        public ActionResult login()
        {
            return View();
        }

    }
}