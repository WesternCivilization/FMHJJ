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
	public class Data_BidManageService : BaseService<Data_BidManage>, IData_BidManageService
	{
		private IData_BidManageDAL data_BidManageDAL = DALContainer.Resolve<IData_BidManageDAL>();
        public override void SetDal()
        {
            Dal = data_BidManageDAL;
        }
	} 
}