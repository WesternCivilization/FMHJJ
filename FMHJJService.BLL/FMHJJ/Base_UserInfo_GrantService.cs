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
	public class Base_UserInfo_GrantService : BaseService<Base_UserInfo_Grant>, IBase_UserInfo_GrantService
	{
		private IBase_UserInfo_GrantDAL base_UserInfo_GrantDAL = DALContainer.Resolve<IBase_UserInfo_GrantDAL>();
        public override void SetDal()
        {
            Dal = base_UserInfo_GrantDAL;
        }
	} 
}