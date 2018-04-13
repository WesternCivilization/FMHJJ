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
	public class Dict_SystemService : BaseService<Dict_System>, IDict_SystemService
	{
		private IDict_SystemDAL dict_SystemDAL = DALContainer.Resolve<IDict_SystemDAL>();
        public override void SetDal()
        {
            Dal = dict_SystemDAL;
        }
	} 
}