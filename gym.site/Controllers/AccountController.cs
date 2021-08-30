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
                /*if(ctxgym.Account.Where(x => x.User == email).Count() > 0) consultas desactivadas
                {*/
                string from = ConfigurationManager.AppSettings["managementEmail"];

                var pass = ConfigurationManager.AppSettings["mailPass"];

                MimeMailAddress fromaddress = new MimeMailAddress(from);

                //Generate Message 
                var mymessage = new MimeMailMessage();
                mymessage.From = fromaddress;

                mymessage.To.Add(email);
                mymessage.Subject = "Gestionalo - Recuperación de Contraseña";
                mymessage.SubjectEncoding = Encoding.UTF8;


                #region mailbody
                var body = new StringBuilder();
                string url = String.Format("http://gestionalo.ar{0}?email={1}", Url.Action("PasswordRecoveryConfirmation", "Account"), email);
                body.AppendLine("<html>");
                body.AppendLine("<head>");
                body.AppendLine(@"<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />");
                body.AppendLine(@"<meta charset=""utf-8"" />");
                body.AppendLine("</head>");
                body.AppendLine(@"<body class=""login-page"" stlye=""min-height: 466px;"">");
                body.AppendLine(@"<div class=""text-center"">");
                body.AppendLine(@"<div class=""login-box""");
                body.AppendLine(@"<h4 style=""font-family:Courier New""> Para restablecer su contraseña, haga click <a href=" + url + ">acá</a>.</h4>");
                body.AppendLine(@"<br /> <p style=""opacity:0.5;font-family:Courier New""> Gesionalo S.A ©</p>");
                body.AppendLine(@"</div>");
                body.AppendLine(@"</div>");
                body.AppendLine(@"</body>");
                body.AppendLine(@"</html>");
                #endregion


                mymessage.Body = body.ToString();
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
                //}

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Ex = ex;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PasswordRecoveryConfirmation(string email)
        {
            return RedirectToAction("Login");
        }
    }
}