using FMHJJService.Model.FMHJJ;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.Model.CustomModel.FMHJJ
{
    public class ProductInfoDetailModel : Base_ProductInfo_Detail
    {
        [ForeignKey("ProductType")]
        public ProductInfoModel ProductInfo { get; set; }
    }

    public class ProductInfoModel : Base_ProductInfo
    {
        public List<ProductInfoDetailModel> ProductInfoDetails { get; set; }
    }
}
