using System.Web;
using System.Web.Mvc;

namespace gym.site.Filters
{
    public class SessionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if(ctx.Session["accountId"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }
}