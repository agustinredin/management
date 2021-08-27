using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gym.site;
using gym.model;
using gym.site.Filters;

namespace gym.site.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        public JsonResult LoginVerif(string email, string password)
        {

            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Register()
        {
            return View();
        }

        [SessionFilter]
        public ActionResult PasswordRecovery()
        {
            return View();
        }
    }
}