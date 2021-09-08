using System;
using System.Web.Mvc;
using System.Web.Routing;
using gym.model;

namespace gym.site.Filters
{
    public class ExceptionFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;

            ExLog log = new ExLog();

            string accountId = string.Empty;
            try
            {
                accountId = filterContext.HttpContext.Session["accountId"] != null
                ? filterContext.HttpContext.Session["accountId"].ToString() : "";
            }
            catch { }

            using (GYMEntities db = new GYMEntities())
            {
                //Seteamos atributos de ENTIDAD
                log.Date = DateTime.UtcNow;
                log.AccountId = accountId == "" ? null : (Guid?)Guid.Parse(accountId);
                log.Module = filterContext.RouteData.Values["controller"].ToString();
                log.Action = filterContext.RouteData.Values["action"].ToString();
                log.ExMessage = ex.Message;
                log.ExStackTrace = ex.StackTrace.Remove(400);
                log.ExSource = ex.Source;
                if (ex.InnerException != null)
                {
                    log.InnerException = true;
                    log.InnerMessage = ex.InnerException.Message;
                    log.InnerStackTrace = ex.InnerException.StackTrace;
                    log.InnerSource = ex.InnerException.Source;
                }
                log.Id = Guid.NewGuid();

                db.ExLog.Add(log);
                db.SaveChanges();
            }

            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Error" }, { "queryname", "ex" }, { "queryvalues", log } });
            filterContext.Result.ExecuteResult(filterContext);
        }
    }
}