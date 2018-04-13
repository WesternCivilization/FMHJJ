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
	public class Dict_FunctionMenuService : BaseService<Dict_FunctionMenu>, IDict_FunctionMenuService
	{
		private IDict_FunctionMenuDAL dict_FunctionMenuDAL = DALContainer.Resolve<IDict_FunctionMenuDAL>();
        public override void SetDal()
        {
            Dal = dict_FunctionMenuDAL;
        }
	} 
}