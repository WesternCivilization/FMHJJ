using FMHJJService.BLL.Container;
using FMHJJService.BLL.IBLL.FMHJJ;
using FMHJJService.Business;
using FMHJJService.Common.Enum;
using FMHJJService.DAL;
using FMHJJService.Model.CustomModel.FMHJJ;
using FMHJJService.Model.FMHJJ;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMHJJService.Controllers
{
    [AuthorizeUser]
    public class SmsManageController : Controller
    {
        // GET: SmsManage
        public ActionResult Index(string begin_time, string end_time)
        {
            if (string.IsNullOrEmpty(begin_time))
            {
                begin_time = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(end_time))
            {
                end_time = DateTime.Now.ToString("yyyy-MM-dd");
            }

            ViewData["begin_time"] = begin_time;
            ViewData["end_time"] = end_time;
            begin_time = begin_time + " 00:00:00";
            end_time = end_time + " 23:59:59";

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@begin_time", begin_time);
            parameters[1] = new SqlParameter("@end_time", end_time);
            string sql = "select * from Data_SmsManage where CreateTime > convert(datetime, @begin_time, 120) and CreateTime < convert(datetime, @end_time, 120)";

            var dbContext = DbContextFactory.CreateByModelNamespace(typeof(Data_SmsManage).Namespace);
            var dataSmsMangeList = dbContext.Database.SqlQuery<Data_SmsManage>(sql, parameters).ToList();
            return View(dataSmsMangeList);
        }

        // GET: SmsManage
        public ActionResult SendSms(string phones, string message, int id = 0)
        {
            string errMsg = string.Empty;

            if (string.IsNullOrEmpty(phones))
            {
                errMsg = "手机号不能不能为空";
                return RedirectToAction("Index", "SmsManage", new { errMsg = errMsg });
            }

            if (string.IsNullOrEmpty(message))
            {
                errMsg = "短信内容不能为空";
                return RedirectToAction("Index", "SmsManage", new { errMsg = errMsg });
            }

            IData_SmsManageService data_SmsManageService = BLLContainer.Resolve<IData_SmsManageService>();
            var data_SmsManage = new Data_SmsManage();
            if (id == 0)
            {
                data_SmsManage.CreateTime = DateTime.Now;
                data_SmsManage.Message = message;
                data_SmsManage.Phones = phones.Replace("，", ",");
                data_SmsManage.State = (int)SmsType.Send;
                if (!data_SmsManageService.Add(data_SmsManage))
                {
                    return RedirectToAction("Index", "SmsManage", new { errMsg = "发送失败" });
                }
            }
            else
            {
                data_SmsManage = data_SmsManageService.GetModels(p => p.ID == id).FirstOrDefault();
                data_SmsManage.Message = message;
                data_SmsManage.Phones = phones.Replace("，", ",");
                if (!data_SmsManageService.Update(data_SmsManage))
                {
                    return RedirectToAction("Index", "SmsManage", new { errMsg = "发送失败" });
                }
            }

            //发送短信
            if (!SMSBusiness.SendSMSNotice("1", data_SmsManage.Phones, data_SmsManage.Message, out errMsg))
            {
                return RedirectToAction("Index", "SmsManage", new { errMsg = errMsg });
            }

            return RedirectToAction("Index", "SmsManage", new { errMsg = "发送成功" });
        }

        // GET: SmsManage
        public ActionResult DeleteSms(int id)
        {
            string errMsg = string.Empty;
            
            IData_SmsManageService data_SmsManageService = BLLContainer.Resolve<IData_SmsManageService>();
            var data_SmsManage = data_SmsManageService.GetModels(p => p.ID == id).FirstOrDefault();
            if (!data_SmsManageService.Delete(data_SmsManage))
            {
                return RedirectToAction("Index", "SmsManage", new { errMsg = "删除失败" });
            }

            return RedirectToAction("Index", "SmsManage", new { errMsg = "删除成功" });
        }
    }
}