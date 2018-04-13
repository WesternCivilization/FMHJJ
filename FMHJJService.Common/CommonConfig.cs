using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.Common
{
    public class CommonConfig
    {
        /// <summary>
        /// 隐身人名称
        /// </summary>
        public static string InvisibleMan = System.Configuration.ConfigurationManager.AppSettings["InvisibleMan"] ?? "";
        /// <summary>
        /// 管理员名称
        /// </summary>
        public static string SuperName = System.Configuration.ConfigurationManager.AppSettings["SuperName"] ?? "";
        /// <summary>
        /// 短信用户名
        /// </summary>
        public static string SMSName = System.Configuration.ConfigurationManager.AppSettings["SmsName"] ?? "";
        /// <summary>
        /// 短信用户密码
        /// </summary>
        public static string SMSPassWord = System.Configuration.ConfigurationManager.AppSettings["SmsPassWord"] ?? "";
        /// <summary>
        /// 文本日志路径
        /// </summary>
        public static string TxtLogPath = System.Configuration.ConfigurationManager.AppSettings["TxtLogPath"] ?? "";
        /// <summary>
        /// 系统初始化日志
        /// </summary>
        public static string SystemInitLog = "Init/";

        public static string DesKey = System.Configuration.ConfigurationManager.AppSettings["DesKey"] ?? "";

    }
}
