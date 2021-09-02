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
using BotDetect.Web.Mvc;

namespace gym.site.Controllers
{
    public class AccountController : Controller
    {
        private readonly GYMEntities ctxgym = new GYMEntities();
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
                if(ctxgym.Account.Where(x => x.User == email).Count() > 0)
                {
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
                body.AppendLine(@"<body>");
                body.AppendLine(@"<div style=""background: #e9ecef;font-size: 16,5px; padding: 15px;margin:5% 20% 5% 20%;"">");
                body.AppendLine(@"<p style=""background: #e9ecef; text-align: center; padding: 15px; font-family: 'Roboto', sans-serif; opacity: 0.7;color:black;"">Haga click en el botón para restablecer su contraseña.</p>");
                body.AppendLine(@"<div style=""text-align:center;"">");
                body.AppendLine(@"<button style=""background: #007bff; border-radius: 5px;""><a style=""color: white;text-decoration: none;"" href=" + url + ">Solicitar nueva contraseña</a></button>");
                body.AppendLine("</div></div>");
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
                }

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
            ViewBag.email = email;
            return View();
        }

        public JsonResult PasswordRecoveryConfirmationVerif(string email, string password)
        {
            //La verificación de contraseñas que coincidan está hecha en JS
            try
            {
                var acc = ctxgym.Account.Where(x => x.User == email).First();
                //Set new password on DB
                acc.Password = password;
                ctxgym.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}