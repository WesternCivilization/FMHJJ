using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMHJJService.DAL.Container;
using FMHJJService.DAL.IDAL.FMHJJ;
using FMHJJService.BLL.Base;
using FMHJJService.BLL.IBLL.FMHJJ;
using FMHJJService.Model.FMHJJ;
namespace FMHJJService.BLL.FMHJJ
{
	public class Base_ProductInfo_DetailService : BaseService<Base_ProductInfo_Detail>, IBase_ProductInfo_DetailService
	{
		private IBase_ProductInfo_DetailDAL base_ProductInfo_DetailDAL = DALContainer.Resolve<IBase_ProductInfo_DetailDAL>();
        public override void SetDal()
        {
            Dal = base_ProductInfo_DetailDAL;
        }
	} 
}