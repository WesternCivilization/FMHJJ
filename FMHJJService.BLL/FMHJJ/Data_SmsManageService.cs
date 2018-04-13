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
	public class Data_SmsManageService : BaseService<Data_SmsManage>, IData_SmsManageService
	{
		private IData_SmsManageDAL data_SmsManageDAL = DALContainer.Resolve<IData_SmsManageDAL>();
        public override void SetDal()
        {
            Dal = data_SmsManageDAL;
        }
	} 
}