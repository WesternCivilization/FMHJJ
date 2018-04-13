using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FMHJJService.App_Start;
using FMHJJService.Utility;
using System.Globalization;

namespace FMHJJService
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //System.Timers.Timer RemindTimer_1;

        protected void Application_Start()
        {
            ApplicationStart.Init();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-Hans");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-Hans");

            //SQL语句定时获取定期任务（频率：1小时）
            //RemindTimer_1 = new System.Timers.Timer(1000 * 60 * 60);
            //RemindTimer_1.Elapsed += new System.Timers.ElapsedEventHandler(RemindTimer_1TimerHandler);
            //RemindTimer_1.Start();
        }

        protected void RemindTimer_1TimerHandler(object source, System.Timers.ElapsedEventArgs e)
        {
            RemindScan Scan = new RemindScan();
            Scan.SqlScanTimeWork();
        }

        protected void Application_BeginRequest()
        {
            CultureInfo cInf = new CultureInfo("en-ZA", false);
            // NOTE: change the culture name en-ZA to whatever culture suits your needs

            cInf.DateTimeFormat.DateSeparator = "/";
            cInf.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            cInf.DateTimeFormat.LongDatePattern = "dd/MM/yyyy hh:mm:ss tt";

            System.Threading.Thread.CurrentThread.CurrentCulture = cInf;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cInf;
        }
    }
}
