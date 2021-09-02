using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gym.site.Controllers
{
    using gym.model;
    public class HomeController : Controller
    {
        private readonly GYMEntities ctx = new GYMEntities();

        // GET: Home/Dashboard
        //La idea del dashboard es crear un panel de control completo con vínculos a las acciones más importantes del Framework.
        /*public ActionResult Dashboard()
        {
            return View();
        }*/

        public ActionResult Error(ExLog ex)
        {
            ViewBag.ex = ex;
            return View();
        }
    }
}