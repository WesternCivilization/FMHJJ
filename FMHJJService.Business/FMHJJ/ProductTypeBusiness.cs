using FMHJJService.BLL.Container;
using FMHJJService.BLL.IBLL.FMHJJ;
using FMHJJService.DAL;
using FMHJJService.Model.CustomModel.FMHJJ;
using FMHJJService.Model.FMHJJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.Business.FMHJJ
{
    public class ProductTypeBusiness
    {
        private static IBase_ProductInfoService base_ProductInfoService = BLLContainer.Resolve<IBase_ProductInfoService>();

        private static List<Base_ProductInfo> listProductInfo = null;

        public static List<Base_ProductInfo> ProductInfoList
        {
            get {
                if (listProductInfo == null) Init();
                return listProductInfo;
            }
        }

        public static void Init()
        {
            string sql = "select ID, ProductName from Base_ProductInfo";
            var dbContext = DbContextFactory.CreateByModelNamespace(typeof(Base_ProductInfo).Namespace);
            listProductInfo = dbContext.Database.SqlQuery<Base_ProductInfo>(sql).ToList();
        }

        public static bool IsEffective(int id, out Base_ProductInfo base_ProductInfo)
        {
            base_ProductInfo = null;

            if (ProductInfoList.Any(p => p.ID == id))
            {
                base_ProductInfo = ProductInfoList.Where(p => p.ID == id).FirstOrDefault();
                return true;
            }

            return false;
        }

        public static bool IsEffective(string productName, out Base_ProductInfo base_ProductInfo)
        {
            base_ProductInfo = null;

            if (ProductInfoList.Any(p => p.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase)))
            {
                base_ProductInfo = ProductInfoList.Where(p => p.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                return true;
            }

            return false;            
        }

        public static string GetProductName(Nullable<int> id)
        {
            return ProductInfoList.Any(p => p.ID == id) ? ProductInfoList.Where(p => p.ID == id).FirstOrDefault().ProductName : string.Empty;
        }
    }
}
