using FMHJJService.BLL.Container;
using FMHJJService.BLL.IBLL.FMHJJ;
using FMHJJService.Business.FMHJJ;
using FMHJJService.Common;
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
    public class NoticeManageController : Controller
    {
        private void GetNoticeTypes(string noteType = "0")
        {
            ////从数据库中读取
            //string sql = "select a.* from Dict_System a inner join Dict_System b on a.PID=b.ID and b.DictKey='公告类型'";
            //var dbContext = DbContextFactory.CreateByModelNamespace(typeof(Flw_BiddingNotice).Namespace);
            //var noticeTypeList = dbContext.Database.SqlQuery<Dict_System>(sql).ToList();
            var noticeTypeList = DictSystemBusiness.GetDicts("公告类型");
            var selectItemList = new List<SelectListItem>() {
                new SelectListItem(){ Value="0", Text="请选择" }
            };

            bool hasSelected = false;
            if (noticeTypeList != null && noticeTypeList.Count > 0)
            {
                foreach (var item in noticeTypeList)
                {
                    if (item.DictValue.Equals(noteType, StringComparison.OrdinalIgnoreCase))
                    {
                        hasSelected = true;
                        selectItemList.Add(new SelectListItem { Value = item.DictValue, Text = item.DictKey, Selected = true });
                    }
                    else
                    {
                        selectItemList.Add(new SelectListItem { Value = item.DictValue, Text = item.DictKey });
                    }
                }
            }

            if (!hasSelected)
            {
                selectItemList[0].Selected = true;
            }

            //var selectList = new SelectList(noticeTypeList, "DictValue", "DictKey");
            //selectItemList.AddRange(selectList);
            ViewBag.database = selectItemList;
        }

        // GET: NoticeManage
        public ActionResult Index(string begin_time, string end_time, string noticetype = "0")
        {
            if (string.IsNullOrEmpty(begin_time))
            {
                begin_time = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(end_time))
            {
                end_time = DateTime.Now.ToString("yyyy-MM-dd");
            }

            GetNoticeTypes(noticetype);
            ViewData["begin_time"] = begin_time;
            ViewData["end_time"] = end_time;
            begin_time = begin_time + " 00:00:00";
            end_time = end_time + " 23:59:59";

            SqlParameter[] parameters = new SqlParameter[2];            
            parameters[0] = new SqlParameter("@begin_time", begin_time);
            parameters[1] = new SqlParameter("@end_time", end_time);
            string sql = "select * from Flw_BiddingNotice where PublishTime > convert(datetime, @begin_time, 120) and PublishTime < convert(datetime, @end_time, 120)";
            if (noticetype != "0")
            {
                sql = sql + " and NoticeType=@noticetype";
                Array.Resize(ref parameters, parameters.Length + 1);
                parameters[parameters.Length - 1] = new SqlParameter("@noticetype", noticetype);
            }
            
            var dbContext = DbContextFactory.CreateByModelNamespace(typeof(Flw_BiddingNotice).Namespace);
            var biddingNoticeList = dbContext.Database.SqlQuery<Flw_BiddingNotice>(sql, parameters).ToList();
            return View(biddingNoticeList);
        }

        public ActionResult AddNotice()
        {
            GetNoticeTypes();
            return View();
        }

        public ActionResult ViewNotice(int id)
        {
            IFlw_BiddingNoticeService iFlw_BiddingNoticeService = BLLContainer.Resolve<IFlw_BiddingNoticeService>();
            var iFlw_BiddingNotice = iFlw_BiddingNoticeService.GetModels(p => p.ID == id).FirstOrDefault();
            GetNoticeTypes(iFlw_BiddingNotice != null ? iFlw_BiddingNotice.NoticeType.ToString() : null);
            return View(iFlw_BiddingNotice);
        }

        [HttpPost]
        public ActionResult AddNotice(Flw_BiddingNotice flw_BiddingNotice)
        {
            var json = new JsonHelper() { Msg = "录入成功", Status = "n" };
            try
            {
                #region 验证参数
                if (string.IsNullOrWhiteSpace(flw_BiddingNotice.Title))
                {
                    json.Msg = "公告标题不能为空";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                if (flw_BiddingNotice.NoticeType == 0)
                {
                    json.Msg = "请选择公告类型";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                #endregion
                flw_BiddingNotice.PublishContent = Microsoft.JScript.GlobalObject.decodeURIComponent(flw_BiddingNotice.PublishContent);
                flw_BiddingNotice.Publisher = ((UserInfoCacheModel)Session["loginUser"]).UserName;
                flw_BiddingNotice.PublishCompany = ((UserInfoCacheModel)Session["loginUser"]).CompanyName;
                flw_BiddingNotice.PublishTime = DateTime.Now;
                if (BLLContainer.Resolve<IFlw_BiddingNoticeService>().Add(flw_BiddingNotice))
                {
                    json.Status = "y";                    
                }
                else
                {
                    json.Msg = "更新数据库失败";
                }
            }
            catch (Exception e)
            {
                json.Msg = e.Message;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditNotice(Flw_BiddingNotice flw_BiddingNotice)
        {
            var json = new JsonHelper() { Msg = "录入成功", Status = "n" };
            try
            {
                #region 验证参数
                if (string.IsNullOrWhiteSpace(flw_BiddingNotice.Title))
                {
                    json.Msg = "公告标题不能为空";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                if (flw_BiddingNotice.NoticeType == 0)
                {
                    json.Msg = "请选择公告类型";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                #endregion

                int id = flw_BiddingNotice.ID;
                var bll = BLLContainer.Resolve<IFlw_BiddingNoticeService>();
                var model = bll.GetModels(p => p.ID == id).FirstOrDefault();
                model.PublishContent = Microsoft.JScript.GlobalObject.decodeURIComponent(flw_BiddingNotice.PublishContent);
                model.Publisher = ((UserInfoCacheModel)Session["loginUser"]).UserName;
                model.PublishCompany = ((UserInfoCacheModel)Session["loginUser"]).CompanyName;
                model.PublishTime = DateTime.Now;
                if (bll.Update(model))
                {
                    json.Status = "y";
                }
                else
                {
                    json.Msg = "更新数据库失败";
                }
            }
            catch (Exception e)
            {
                json.Msg = e.Message;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteNotice(int id)
        {
            string errMsg = "删除成功";

            IFlw_BiddingNoticeService iFlw_BiddingNoticeService = BLLContainer.Resolve<IFlw_BiddingNoticeService>();
            var flw_BiddingNotice = iFlw_BiddingNoticeService.GetModels(p => p.ID == id).FirstOrDefault();
            if (flw_BiddingNotice == null)
            {
                errMsg = "该公告信息不存在";
                return RedirectToAction("Index", "NoticeManage", new { errMsg = errMsg });
            }

            if (!iFlw_BiddingNoticeService.Delete(flw_BiddingNotice))
            {
                errMsg = "数据库更新失败";
                return RedirectToAction("Index", "NoticeManage", new { errMsg = errMsg });
            }

            return RedirectToAction("Index", "NoticeManage", new { errMsg = errMsg });
        }

        [AllowAnonymous]
        public ActionResult NoticePublish()
        {
            IFlw_BiddingNoticeService iFlw_BiddingNoticeService = BLLContainer.Resolve<IFlw_BiddingNoticeService>();
            var pv = DataParameterBusiness.GetParameterValue("公告期数") ?? string.Empty;
            return View(iFlw_BiddingNoticeService.GetModels(p => p.NoticeType == (int)NoticeType.Notice).OrderByDescending(o => o.PublishTime)
                .Take((!string.IsNullOrEmpty(pv) && Utils.IsNumeric(pv)) ? Convert.ToInt32(pv) : int.MaxValue).ToList());
        }

        [AllowAnonymous]
        public ActionResult NoticeRule()
        {
            IFlw_BiddingNoticeService iFlw_BiddingNoticeService = BLLContainer.Resolve<IFlw_BiddingNoticeService>();
            return View(iFlw_BiddingNoticeService.GetModels(p=>p.NoticeType == (int)NoticeType.Rule).OrderByDescending(p=>p.PublishTime).FirstOrDefault());
        }
    }
}