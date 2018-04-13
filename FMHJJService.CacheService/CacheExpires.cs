using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMHJJService.CacheService
{
    public class CacheExpires
    {
        public static int SMSTempTokenExpires = 10*60;//短信临时token过期时间

        public static int SMSCodeExpires = 2*60;//短信验证码

        public static int ImgCodeCache = 1*60;        

        public static int CommonPageQueryDataCacheTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CommonPageQueryDataCacheTime"].ToString());
    }
}