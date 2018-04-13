using FMHJJService.BLL.Container;
using FMHJJService.BLL.IBLL.FMHJJ;
using FMHJJService.Business.FMHJJ;
using FMHJJService.DAL;
using FMHJJService.Model.CustomModel.FMHJJ;
using FMHJJService.Model.FMHJJ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace FMHJJService.Controllers
{
    [AuthorizeUser]
    public class ProductInfoManageController : Controller
    {
        // GET: ProductInfoManage
        public ActionResult Index()
        {
            var dbContext = DbContextFactory.CreateByModelNamespace(typeof(ProductInfoDetailModel).Namespace);
            var productInfoModel = dbContext.Set<ProductInfoModel>().Include(m => m.ProductInfoDetails).ToList();
            //string sql = "select * from Base_ProductInfo a left join Base_ProductInfo_Detail b on a.ID=b.ProductType";
            //var productInfoModel = dbContext.Database.SqlQuery<ProductInfoModel>(sql).ToList();
            return View(productInfoModel ?? new List<ProductInfoModel>());
        }

        public ActionResult CreateProduct(string productName)
        {
            string errMsg = "添加成功";

            if (string.IsNullOrEmpty(productName))
            {
                errMsg = "产品名称不能为空";
                return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
            }

            //开启EF事务
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    IBase_ProductInfoService base_ProductInfoService = BLLContainer.Resolve<IBase_ProductInfoService>();
                    if (base_ProductInfoService.GetCount(p => p.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase)) > 0)
                    {
                        errMsg = "该产品名称已存在";
                        return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
                    }

                    var base_ProductInfo = new Base_ProductInfo();
                    base_ProductInfo.ProductName = productName;
                    if (!base_ProductInfoService.Add(base_ProductInfo))
                    {
                        errMsg = "数据库更新失败";
                        return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
                    }

                    var dbContext = DbContextFactory.CreateByModelNamespace(typeof(Base_ProductInfo).Namespace);
                    var userLvls = DictSystemBusiness.GetDicts("客户等级");
                    foreach (var model in userLvls)
                    {
                        var base_ProductInfo_Detail = new Base_ProductInfo_Detail();
                        base_ProductInfo_Detail.UserLvl = Convert.ToInt32(model.DictValue);
                        base_ProductInfo_Detail.ProductType = base_ProductInfo.ID;
                        dbContext.Entry(base_ProductInfo_Detail).State = EntityState.Added;
                    }

                    if (dbContext.SaveChanges() < 0)
                    {
                        errMsg = "数据库更新失败";
                        return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
                    }

                    ts.Complete();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "ProductInfoManage", new { errMsg = ex.Message });
                }
            }

            //更新缓存
            ProductTypeBusiness.Init();
            
            return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
        }

        public ActionResult UpdateProduct(int id, string productName)
        {
            string errMsg = "更新成功";

            if (string.IsNullOrEmpty(productName))
            {
                errMsg = "产品名称不能为空";
                return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
            }

            IBase_ProductInfoService base_ProductInfoService = BLLContainer.Resolve<IBase_ProductInfoService>();
            if (base_ProductInfoService.GetCount(p => p.ID != id && p.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase)) > 0)
            {
                errMsg = "该产品名称已存在";
                return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
            }

            var base_ProductInfo = base_ProductInfoService.GetModels(p => p.ID == id).FirstOrDefault();
            base_ProductInfo.ProductName = productName;
            if (!base_ProductInfoService.Update(base_ProductInfo))
            {
                errMsg = "数据库更新失败";
                return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
            }

            //更新缓存
            ProductTypeBusiness.Init();

            return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
        }

        public ActionResult DeleteProduct(int id)
        {
            string errMsg = "删除成功";            

            //开启EF事务
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    IBase_ProductInfoService base_ProductInfoService = BLLContainer.Resolve<IBase_ProductInfoService>();
                    if (base_ProductInfoService.GetCount(p => p.ID == id) == 0)
                    {
                        errMsg = "该产品信息不存在";
                        return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
                    }

                    var base_ProductInfo = base_ProductInfoService.GetModels(p => p.ID == id).FirstOrDefault();
                    if (!base_ProductInfoService.Delete(base_ProductInfo))
                    {
                        errMsg = "数据库更新失败";
                        return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
                    }

                    IBase_ProductInfo_DetailService base_ProductInfo_DetailService = BLLContainer.Resolve<IBase_ProductInfo_DetailService>();
                    var dbContext = DbContextFactory.CreateByModelNamespace(typeof(Base_ProductInfo_Detail).Namespace);
                    var base_ProductInfo_Details = base_ProductInfo_DetailService.GetModels(p => p.ProductType == base_ProductInfo.ID).ToList();
                    foreach (var item in base_ProductInfo_Details)
                    {
                        dbContext.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }

                    if (dbContext.SaveChanges() < 0)
                    {
                        errMsg = "数据库更新失败";
                        return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
                    }

                    ts.Complete();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "ProductInfoManage", new { errMsg = ex.Message });
                }
            }

            //更新缓存
            ProductTypeBusiness.Init();

            return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
        }

        [HttpPost]
        public ActionResult UpdateLevel(List<ProductInfoDetailModel> productInfoDetails)
        {
            string errMsg = "更新等级成功";

            if (productInfoDetails == null)
            {
                errMsg = "上传数据不能为空";
                return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
            }

            try
            {
                var dbContext = DbContextFactory.CreateByModelNamespace(typeof(ProductInfoDetailModel).Namespace);
                foreach (var item in productInfoDetails)
                {
                    if (!item.CustomerUpper.HasValue)
                    {
                        errMsg = "客户上限不能为空";
                        return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
                    }

                    //if (!item.AreaUpper.HasValue)
                    //{
                    //    errMsg = "区域上限不能为空";
                    //    return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
                    //}

                    if (item.ID <= 0)
                    {
                        errMsg = "产品信息ID不能为空";
                        return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
                    }

                    dbContext.Entry(item).State = EntityState.Modified;
                }

                if (dbContext.SaveChanges() < 0)
                {
                    errMsg = "数据库更新失败";
                    return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "ProductInfoManage", new { errMsg = ex.Message });
            }

            return RedirectToAction("Index", "ProductInfoManage", new { errMsg = errMsg });
        }
    }
}