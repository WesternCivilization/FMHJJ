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
	public class Data_ParametersService : BaseService<Data_Parameters>, IData_ParametersService
	{
		private IData_ParametersDAL data_ParametersDAL = DALContainer.Resolve<IData_ParametersDAL>();
        public override void SetDal()
        {
            Dal = data_ParametersDAL;
        }
	} 
}