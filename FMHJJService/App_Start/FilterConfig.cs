using System.Web;
using System.Web.Mvc;

namespace FMHJJService
{
    //定义缓存过滤器
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //filterContext.HttpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            //filterContext.HttpContext.Response.Cache.SetNoStore();
            filterContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*"); //跨域访问CORS
        }
    }

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new NoCacheAttribute()); 
        }
    }
}
