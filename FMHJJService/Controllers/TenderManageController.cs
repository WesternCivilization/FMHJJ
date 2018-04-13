using FMHJJService.BLL.CommonBLL;
using FMHJJService.BLL.Container;
using FMHJJService.BLL.IBLL.FMHJJ;
using FMHJJService.Business;
using FMHJJService.Business.FMHJJ;
using FMHJJService.Common;
using FMHJJService.Common.Enum;
using FMHJJService.DAL;
using FMHJJService.Model.CustomModel.FMHJJ;
using FMHJJService.Model.FMHJJ;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace FMHJJService.Controllers
{
    [AuthorizeUser]
    public class TenderManageController : Controller
    {

        private bool getTimeLeft(Data_BidManage data_BidManage, int bidId, out double timeleft)
        {
            timeleft = 0;
            if (data_BidManage == null) return false;
            if (data_BidManage.BidEndTime == null) return false;
            timeleft = ((DateTime)data_BidManage.BidEndTime - DateTime.Now).TotalMilliseconds;
            return true;
        }

        private bool CheckAmountTotal(int bidId, int userId, Nullable<int> productType, Nullable<decimal> estimateAmount, int userLvl, decimal bidAmount, out string errMsg)
        {
            errMsg = string.Empty;

            if (!productType.HasValue)
            {
                errMsg = "产品类别不能为空";
                return false;
            }

            if (!estimateAmount.HasValue)
            {
                errMsg = "预估量不能为空";
                return false;
            }

            IFlw_BiddingInfoService flw_BiddingInfoService = BLLContainer.Resolve<IFlw_BiddingInfoService>();
            decimal amountTotal = flw_BiddingInfoService.GetModels(p => p.BidID == bidId && p.UserID == userId).Sum(s => (decimal?)s.BidAmount).GetValueOrDefault();
            decimal maxAmount = getMaxAmount((decimal)estimateAmount, (int)productType, userLvl);

            if (maxAmount < (amountTotal + bidAmount))
            {
                errMsg = "超出申请限额";
                return false;
            }

            return true;
        }

        private bool CheckAmountMin(Nullable<decimal> amountMin, Nullable<decimal> bidAmount, out string errMsg)
        {
            errMsg = string.Empty;

            if (!bidAmount.HasValue || bidAmount <= 0)
            {
                errMsg = "申报量不能为空且须大于0";
                return false;
            }

            if (amountMin > bidAmount)
            {
                errMsg = "不能少于最小申报量" + amountMin;
                return false;
            }

            return true;
        }

        private bool CheckPrice(Nullable<decimal> currentPriceLower, Nullable<decimal> currentPriceUpper, Nullable<decimal> bidPrice, out string errMsg)
        {
            errMsg = string.Empty;

            if (!bidPrice.HasValue || bidPrice <= 0)
            {
                errMsg = "申报价格不能为空且须大于0";
                return false;
            }

            if (bidPrice < currentPriceLower || bidPrice > currentPriceUpper)
            {
                errMsg = "申报价格不在报价范围以内" + currentPriceLower + "-" + currentPriceUpper;
                return false;
            }

            return true;
        }

        private bool CheckCount(int bidId, int userId, Nullable<int> bidCount, out string errMsg)
        {
            errMsg = string.Empty;

            IFlw_BiddingInfoService flw_BiddingInfoService = BLLContainer.Resolve<IFlw_BiddingInfoService>();
            if (flw_BiddingInfoService.GetCount(p => p.BidID == bidId && p.UserID == userId) >= bidCount)
            {
                errMsg = "超出可申报单数" + bidCount;
                return false;
            }

            return true;
        }

        private List<FinalBiddingInfoModel> getFinalBiddingInfoList(int bidId)
        {
            string sql = " exec GetFinalist @BidID";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@BidID", bidId);
            System.Data.DataSet ds = FMHJJBLL.ExecuteQuerySentence(sql, parameters);
            if (ds.Tables.Count == 1)
            {
                return Utils.GetModelList<FinalBiddingInfoModel>(ds.Tables[0]);
            }

            return new List<FinalBiddingInfoModel>();
        }

        private decimal getMaxAmount(decimal estimateAmount, int productType, int userLvl)
        {
            IBase_ProductInfo_DetailService base_ProductInfo_DetailService = BLLContainer.Resolve<IBase_ProductInfo_DetailService>();
            decimal customerUpper = base_ProductInfo_DetailService.GetModels(p => p.ProductType == productType && p.UserLvl == userLvl).Max(m => (decimal?)m.CustomerUpper).GetValueOrDefault();
            return Math.Round(estimateAmount * customerUpper * 0.01M, 2, MidpointRounding.AwayFromZero);
        }        

        // GET: TenderManage
        public ActionResult BidCheck(int id)
        {
            IData_BidManageService data_BidManageService = BLLContainer.Resolve<IData_BidManageService>();
            var data_BidManage = data_BidManageService.GetModels(p => p.ID == id).FirstOrDefault();
            ViewBag.data_BidManage = data_BidManage;
            ViewBag.productName = ProductTypeBusiness.GetProductName(data_BidManage.ProductType);
            ViewBag.bidDate = string.Format("{0:yyyy-M-d}", data_BidManage.BidDate);
            ViewBag.flw_BiddingInfoList = getFinalBiddingInfoList(id);

            return View();
        }

        // GET: TenderManage
        public ActionResult BidView(int id)
        {
            IData_BidManageService data_BidManageService = BLLContainer.Resolve<IData_BidManageService>();
            var data_BidManage = data_BidManageService.GetModels(p => p.ID == id).FirstOrDefault();
            ViewBag.data_BidManage = data_BidManage;
            ViewBag.productName = ProductTypeBusiness.GetProductName(data_BidManage.ProductType);
            ViewBag.bidDate = string.Format("{0:yyyy-M-d}", data_BidManage.BidDate);
            ViewBag.flw_BiddingInfoList = getFlw_BiddingInfoList(id);

            return View();
        }

        public PartialViewResult BidDetail(int bidId)
        {
            IData_BidManageService data_BidManageService = BLLContainer.Resolve<IData_BidManageService>();
            var data_BidManage = data_BidManageService.GetModels(p => p.ID == bidId).FirstOrDefault();
            ViewBag.productName = ProductTypeBusiness.GetProductName(data_BidManage.ProductType);
            ViewBag.bidDate = string.Format("{0:yyyy-M-d}", data_BidManage.BidDate);
            ViewBag.flw_BiddingInfoList = getFinalBiddingInfoList(bidId);

            return PartialView();
        }

        // GET: TenderManage
        public ActionResult BidHistory(string begin_time, string end_time)
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
            var bt = Convert.ToDateTime(begin_time);
            var et = Convert.ToDateTime(end_time);
            IData_BidManageService data_BidManageService = BLLContainer.Resolve<IData_BidManageService>();
            IFlw_BiddingInfoService flw_BiddingInfoService = BLLContainer.Resolve<IFlw_BiddingInfoService>();
            var historyBiddingInfoList = data_BidManageService.GetModels(p => p.BidDate >= bt && p.BidDate <= et && p.State == 1).ToList().Select
                (o => new HistoryBiddingInfoModel
                {
                    BidID = o.ID,
                    ProductType = o.ProductType,
                    BidDate = o.BidDate,
                    EstimateAmount = o.EstimateAmount,
                    BidPriceMin = flw_BiddingInfoService.GetModels(p => p.BidID == o.ID).Min(m => m.BidPrice),
                    UserIDs = flw_BiddingInfoService.GetModels(p => p.BidID == o.ID).Select(s => s.UserID).ToList(),
                    DealTotalAmount = getFinalBiddingInfoList(o.ID).Sum(m => m.BidAmount),
                    DealTotalPrice = getFinalBiddingInfoList(o.ID).Sum(m => m.BidAmount * m.BidPrice)
                });

            return View(historyBiddingInfoList);
        }

        private List<Flw_BiddingInfo> getFlw_BiddingInfoList(int id)
        {
            var dbContext = DbContextFactory.CreateByModelNamespace(typeof(Data_BidManage).Namespace);
            return (from a in dbContext.Set<Flw_BiddingInfo>().Where(p => p.BidID == id)
             join b in dbContext.Set<Base_UserInfo>() on a.UserID equals b.ID
             orderby a.BidPrice descending, b.UserLvl, a.BidAmount descending, a.BidTime
             select a).ToList();
        }

        // GET: TenderManage
        public ActionResult BidGather(int id)
        {           
            var data_BidManage = BLLContainer.Resolve<IData_BidManageService>().GetModels(p => p.ID == id).FirstOrDefault();
            ViewBag.data_BidManage = data_BidManage;
            ViewBag.productName = ProductTypeBusiness.GetProductName(ViewBag.data_BidManage.ProductType);

            var userInfoCacheModel = Session["loginUser"] as UserInfoCacheModel;
            ViewBag.userId = userInfoCacheModel.ID;
            int productType = (int)userInfoCacheModel.ProductType;
            int userLvl = (int)userInfoCacheModel.UserLvl;
            ViewBag.MaxAmount = getMaxAmount((decimal)data_BidManage.EstimateAmount, productType, userLvl);

            ViewBag.flw_BiddingInfoList = getFlw_BiddingInfoList(id);

            var flw_BiddingInfo = new Flw_BiddingInfo();
            flw_BiddingInfo.BidID = id;

            double timeleft = 0;
            getTimeLeft(data_BidManage, id, out timeleft);
            ViewBag.timeleft = (timeleft <= 0) ? 0 : timeleft;
            return View(flw_BiddingInfo);
        }

        [HttpPost]        
        public ActionResult BidGather(Flw_BiddingInfo flw_BiddingInfo)
        {
            var json = new JsonHelper() { Msg = "录入成功", Status = "n" };
            try
            {
                var bidId = flw_BiddingInfo.BidID;
                if (bidId == null || bidId == 0)
                {
                    json.Msg = "竞价项目Id为空";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                
                var userInfo = Session["loginUser"] as UserInfoCacheModel;
                if (userInfo == null || !UserBusiness.IsEffectiveUser(userInfo.ID))
                {
                    json.Msg = "客户信息无效";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (userInfo.UserLvl == null || userInfo.UserLvl == 0)
                {
                    json.Msg = "未审核客户不能参与竞价";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }                

                IData_BidManageService data_BidManageService = BLLContainer.Resolve<IData_BidManageService>();
                var data_BidManage = data_BidManageService.GetModels(p => p.ID == bidId).FirstOrDefault();
                if (data_BidManage == null)
                {
                    json.Msg = "该竞价项目不存在";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                double timeleft = 0;
                if (!getTimeLeft(data_BidManage, (int)bidId, out timeleft))
                {
                    json.Msg = "获取剩余时间失败";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (timeleft <= 0)
                {
                    json.Msg = "该项目竞价已结束";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                var errMsg = string.Empty;                
                if (!CheckAmountMin(data_BidManage.AmountMin, flw_BiddingInfo.BidAmount, out errMsg))
                {
                    json.Msg = errMsg;
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (!CheckPrice(data_BidManage.CurrentPriceLower, data_BidManage.CurrentPriceUpper, flw_BiddingInfo.BidPrice, out errMsg))
                {
                    json.Msg = errMsg;
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (!CheckCount(data_BidManage.ID, userInfo.ID, data_BidManage.BidCount, out errMsg))
                {
                    json.Msg = errMsg;
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (!CheckAmountTotal(data_BidManage.ID, userInfo.ID, data_BidManage.ProductType, data_BidManage.EstimateAmount, (int)userInfo.UserLvl, (decimal)flw_BiddingInfo.BidAmount, out errMsg))
                {
                    json.Msg = errMsg;
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                var userId = userInfo.ID;
                flw_BiddingInfo.UserID = userId;
                flw_BiddingInfo.BidTime = DateTime.Now;
                IFlw_BiddingInfoService flw_BiddingInfoService = BLLContainer.Resolve<IFlw_BiddingInfoService>();
                if (!flw_BiddingInfoService.Add(flw_BiddingInfo))
                {
                    json.Msg = "更新数据库失败";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                
                json.Status = "y";
            }
            catch (Exception e)
            {
                json.Msg = e.Message;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BidVerify(int id)
        {
            IData_BidManageService data_BidManageService = BLLContainer.Resolve<IData_BidManageService>();
            var data_BidManage = data_BidManageService.GetModels(p => p.ID == id).FirstOrDefault();
            ViewBag.productName = ProductTypeBusiness.GetProductName(data_BidManage.ProductType);

            IFlw_BiddingInfoService flw_BiddingInfoService = BLLContainer.Resolve<IFlw_BiddingInfoService>();
            var flw_BiddingInfoList = flw_BiddingInfoService.GetModels(p => p.BidID == id).OrderByDescending(o => o.BidPrice).ThenByDescending(o => o.BidAmount).ThenBy(o => o.BidTime).ToList();

            return View(flw_BiddingInfoList);
        }

        [HttpPost]
        public ActionResult BidVerify(List<Flw_BiddingInfo> flw_BiddingInfos)
        {
            var json = new JsonHelper() { Msg = "录入成功", Status = "n" };
            try
            {
                if (flw_BiddingInfos == null || flw_BiddingInfos.Count == 0)
                {
                    json.Msg = "无待审核的竞价信息";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                IData_BidManageService data_BidManageService = BLLContainer.Resolve<IData_BidManageService>();
                IFlw_BiddingInfoService flw_BiddingInfoService = BLLContainer.Resolve<IFlw_BiddingInfoService>();
                int bidId = (int)flw_BiddingInfos[0].BidID;                

                //开启EF事务
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var data_BidManage = data_BidManageService.GetModels(p => p.ID == bidId).FirstOrDefault();
                        if (data_BidManage == null)
                        {
                            json.Msg = "该竞标项目已不存在";
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }

                        data_BidManage.State = 1;
                        if (!data_BidManageService.Update(data_BidManage))
                        {
                            json.Msg = "更新项目信息失败";
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }

                        foreach (var model in flw_BiddingInfos)
                        {
                            int id = model.ID;
                            var flw_BiddingInfo = flw_BiddingInfoService.GetModels(p => p.ID == id).FirstOrDefault();
                            if (flw_BiddingInfo == null)
                            {
                                json.Msg = "竞价信息id" + id + "不存在";
                                return Json(json, JsonRequestBehavior.AllowGet);
                            }

                            flw_BiddingInfo.Checked = model.Checked;
                            if (!flw_BiddingInfoService.Update(flw_BiddingInfo))
                            {
                                json.Msg = "更新竞价信息失败";
                                return Json(json, JsonRequestBehavior.AllowGet);
                            }
                        }

                        ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        json.Msg = ex.Message;
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                }                                   

                json.Status = "y";
            }
            catch (Exception e)
            {
                json.Msg = e.Message;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendSms(int id)
        {
            string errMsg = string.Empty;
            var userIDs = getFinalBiddingInfoList(id).GroupBy(g => g.UserID).Select(o => o.Key).ToList();
            IBase_UserInfoService base_UserInfoService = BLLContainer.Resolve<IBase_UserInfoService>();
            var userMobiles = base_UserInfoService.GetModels(p => userIDs.Contains(p.ID)).Select(o => o.Mobile).ToList();
            string phones = string.Join(",", userMobiles);

            IData_SmsManageService data_SmsManageService = BLLContainer.Resolve<IData_SmsManageService>();
            var data_SmsManage = new Data_SmsManage();
            data_SmsManage.CreateTime = DateTime.Now;
            data_SmsManage.Phones = phones;
            data_SmsManage.State = (int)SmsType.Send;
            if (!data_SmsManageService.Add(data_SmsManage))
            {
                return RedirectToAction("NoticePublish", "NoticeManage", new { errMsg = "发送失败" });
            }

            //发送短信
            if (!SMSBusiness.SendSMSNotice("2", data_SmsManage.Phones, "", out errMsg))
            {
                return RedirectToAction("NoticePublish", "NoticeManage", new { errMsg = errMsg });
            }

            return RedirectToAction("NoticePublish", "NoticeManage", new { errMsg = "发送成功" });
        }
    }
}