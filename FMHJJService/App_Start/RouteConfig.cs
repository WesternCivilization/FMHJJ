using System.Web.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMHJJService
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
           "Default", // 1.路由名称
           "{controller}/{action}/{id}", // 2.带有参数的URL
            new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            }
         );
        }
    }
}
