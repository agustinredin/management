using System.Web;
using System.Web.Mvc;
using gym.site.Filters;

namespace gym.site
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionFilter());
        }
    }
}
