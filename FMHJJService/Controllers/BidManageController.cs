using FMHJJService.BLL.Container;
using FMHJJService.BLL.IBLL.FMHJJ;
using FMHJJService.Business.FMHJJ;
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
    public class BidManageController : Controller
    {
        private void GetProductTypes(string productType = "0")
        {
            var productInfoList = ProductTypeBusiness.ProductInfoList;
            var selectItemList = new List<SelectListItem>() {
                new SelectListItem(){ Value = "0", Text = "请选择" }
            };

            bool hasSelected = false;
            if (productInfoList != null && productInfoList.Count > 0)
            {
                foreach (var item in productInfoList)
                {
                    if (item.ID.ToString().Equals(productType, StringComparison.OrdinalIgnoreCase))
                    {
                        hasSelected = true;
                        selectItemList.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.ProductName, Selected = true });
                    }
                    else
                    {
                        selectItemList.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.ProductName });
                    }
                }
            }

            if (!hasSelected)
            {
                selectItemList[0].Selected = true;
            }

            ViewBag.database_producttypes = selectItemList;
        }

        // GET: BidManage
        public ActionResult Index(string begin_time, string end_time, string producttype = "0")
        {            
            if (string.IsNullOrEmpty(begin_time))
            {
                begin_time = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(end_time))
            {
                end_time = DateTime.Now.ToString("yyyy-MM-dd");
            }

            ViewData["SelectedProductType"] = producttype;
            GetProductTypes(producttype);
            ViewData["begin_time"] = begin_time;
            ViewData["end_time"] = end_time;
            begin_time = begin_time + " 00:00:00";
            end_time = end_time + " 23:59:59";

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@begin_time", begin_time);
            parameters[1] = new SqlParameter("@end_time", end_time);
            string sql = "select * from Data_BidManage where BidDate > convert(datetime, @begin_time, 120) and BidDate < convert(datetime, @end_time, 120)";
            if (producttype != "0")
            {
                sql = sql + " and ProductType=@producttype";
                Array.Resize(ref parameters, parameters.Length + 1);
                parameters[parameters.Length - 1] = new SqlParameter("@producttype", producttype);
            }

            var dbContext = DbContextFactory.CreateByModelNamespace(typeof(Data_BidManage).Namespace);
            var bidManageList = dbContext.Database.SqlQuery<Data_BidManage>(sql, parameters).ToList();
            return View(bidManageList);
        }

        public ActionResult CreateBid(int productType, string bidDate, string bidStartTime, string bidEndTime, Nullable<decimal> estimateAmount, Nullable<decimal> currentPriceLower, Nullable<decimal> currentPriceUpper, Nullable<int> bidCount, Nullable<decimal> amountMin, Nullable<int> bidManner)
        {
            string errMsg = string.Empty;

            #region 验证参数
            if (productType == 0)
            {
                errMsg = "产品类型不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (string.IsNullOrEmpty(bidDate))
            {
                errMsg = "竞价日期不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (string.IsNullOrEmpty(bidStartTime))
            {
                errMsg = "开始时间不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (string.IsNullOrEmpty(bidEndTime))
            {
                errMsg = "结束时间不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (estimateAmount == null)
            {
                errMsg = "预估量不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (estimateAmount <= 0)
            {
                errMsg = "预估量须大于0";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (currentPriceLower == null)
            {
                errMsg = "报价下限不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (currentPriceLower < 0)
            {
                errMsg = "报价下限须大于0";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (currentPriceUpper == null)
            {
                errMsg = "报价上限不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (currentPriceUpper < 0)
            {
                errMsg = "报价上限须大于0";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (currentPriceUpper < currentPriceLower)
            {
                errMsg = "报价上限须大于报价下限";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            #endregion

            IData_BidManageService data_BidManageService = BLLContainer.Resolve<IData_BidManageService>();
            var data_BidManage = new Data_BidManage();
            data_BidManage.ProductType = productType;
            data_BidManage.BidDate = Convert.ToDateTime(bidDate);
            data_BidManage.BidStartTime = Convert.ToDateTime(bidDate + " " + bidStartTime);
            data_BidManage.BidEndTime = Convert.ToDateTime(bidDate + " " + bidEndTime);
            data_BidManage.EstimateAmount = estimateAmount;
            data_BidManage.CurrentPriceLower = currentPriceLower;
            data_BidManage.CurrentPriceUpper = currentPriceUpper;
            data_BidManage.BidCount = bidCount;
            data_BidManage.AmountMin = amountMin;
            data_BidManage.BidManner = bidManner;
            data_BidManage.Publisher = (Session["loginUser"] as UserInfoCacheModel).UserName;
            data_BidManage.PublishTime = DateTime.Now;

            if (!data_BidManageService.Add(data_BidManage))
            {
                return RedirectToAction("Index", "BidManage", new { errMsg = "更新数据库失败" });
            }

            return RedirectToAction("Index", "BidManage", new { errMsg = "添加成功" });
        }

        public ActionResult UpdateBid(int id, int productType, string bidDate, string bidStartTime, string bidEndTime, Nullable<decimal> estimateAmount, Nullable<decimal> currentPriceLower, Nullable<decimal> currentPriceUpper, Nullable<int> bidCount, Nullable<decimal> amountMin, Nullable<int> bidManner)
        {
            string errMsg = string.Empty;

            #region 验证参数
            if (id == 0)
            {
                errMsg = "竞价标的Id不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (productType == 0)
            {
                errMsg = "产品类型不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (string.IsNullOrEmpty(bidDate))
            {
                errMsg = "竞价日期不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (string.IsNullOrEmpty(bidStartTime))
            {
                errMsg = "开始时间不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            if (string.IsNullOrEmpty(bidEndTime))
            {
                errMsg = "结束时间不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }
            #endregion

            IData_BidManageService data_BidManageService = BLLContainer.Resolve<IData_BidManageService>();
            if (data_BidManageService.GetCount(p => p.ID == id) == 0)
            {
                errMsg = "该竞价标的不存在";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            var data_BidManage = data_BidManageService.GetModels(p => p.ID == id).FirstOrDefault();
            data_BidManage.ProductType = productType;
            data_BidManage.BidDate = Convert.ToDateTime(bidDate);
            data_BidManage.BidStartTime = Convert.ToDateTime(bidDate + " " + bidStartTime);
            data_BidManage.BidEndTime = Convert.ToDateTime(bidDate + " " + bidEndTime);
            data_BidManage.EstimateAmount = estimateAmount;
            data_BidManage.CurrentPriceLower = currentPriceLower;
            data_BidManage.CurrentPriceUpper = currentPriceUpper;
            data_BidManage.BidCount = bidCount;
            data_BidManage.AmountMin = amountMin;
            data_BidManage.BidManner = bidManner;

            if (!data_BidManageService.Update(data_BidManage))
            {
                return RedirectToAction("Index", "BidManage", new { errMsg = "更新数据库失败" });
            }

            return RedirectToAction("Index", "BidManage", new { errMsg = "更新成功" });
        }

        public ActionResult DeleteBid(int id)
        {
            string errMsg = string.Empty;

            #region 验证参数
            if (id == 0)
            {
                errMsg = "竞价标的Id不能为空";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }            
            #endregion

            IData_BidManageService data_BidManageService = BLLContainer.Resolve<IData_BidManageService>();
            if (data_BidManageService.GetCount(p => p.ID == id) == 0)
            {
                errMsg = "该竞价标的不存在";
                return RedirectToAction("Index", "BidManage", new { errMsg = errMsg });
            }

            var data_BidManage = data_BidManageService.GetModels(p => p.ID == id).FirstOrDefault();

            if (!data_BidManageService.Delete(data_BidManage))
            {
                return RedirectToAction("Index", "BidManage", new { errMsg = "更新数据库失败" });
            }

            return RedirectToAction("Index", "BidManage", new { errMsg = "删除成功" });
        }
    }
}