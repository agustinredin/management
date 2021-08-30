using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gym.site;
using gym.model;
using gym.site.Filters;
using AegisImplicitMail;
using System.Configuration;
using System.Text;

namespace gym.site.Controllers
{
    public class AccountController : Controller
    {
        private readonly IntegratekGYMEntities ctxgym = new IntegratekGYMEntities();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        public JsonResult LoginVerif(string email, string password)
        {
            if (ctxgym.Account.Where(p => p.User == email && p.Password == password).Count() == 1)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult PasswordRecovery()
        {
            return View();
        }

        public JsonResult PasswordRecoveryMail(string email)
        {
            try
            {
                string from = "gestionaloargentina@gmail.com";

                var pass = ConfigurationManager.AppSettings["mailPass"];

                MimeMailAddress fromaddress = new MimeMailAddress(from);

                //Generate Message 
                var mymessage = new MimeMailMessage();
                mymessage.From = fromaddress;

                mymessage.To.Add(email);
                mymessage.Subject = "Gestionalo - Recuperar contraseña";
                mymessage.SubjectEncoding = Encoding.UTF8;

                #region mailbody
                var body = new StringBuilder();
                body.AppendLine("<html><body><h4></h4>");
                #endregion

                //mymessage.Body = body;
                mymessage.IsBodyHtml = true;
                mymessage.BodyEncoding = Encoding.UTF8;
                //Create Smtp Client
                var mailer = new MimeMailer("smtp.gmail.com", 465);
                mailer.User = from;
                mailer.Password = pass;
                mailer.SslType = SslMode.Ssl;
                mailer.AuthenticationMode = AuthenticationType.Base64;

                //Set a delegate function for call back                
                mailer.SendMail(mymessage);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}