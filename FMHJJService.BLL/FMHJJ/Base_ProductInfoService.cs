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
	public class Base_ProductInfoService : BaseService<Base_ProductInfo>, IBase_ProductInfoService
	{
		private IBase_ProductInfoDAL base_ProductInfoDAL = DALContainer.Resolve<IBase_ProductInfoDAL>();
        public override void SetDal()
        {
            Dal = base_ProductInfoDAL;
        }
	} 
}