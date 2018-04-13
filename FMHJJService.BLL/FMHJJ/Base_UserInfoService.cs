using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMHJJService.BLL.Base;
using FMHJJService.BLL.IBLL.FMHJJ;
using FMHJJService.DAL.Container;
using FMHJJService.DAL.IDAL.FMHJJ;
using FMHJJService.Model.FMHJJ;

namespace FMHJJService.BLL.FMHJJ
{
    public partial class Base_UserInfoService : BaseService<Base_UserInfo>, IBase_UserInfoService
    {
        private IBase_UserInfoDAL Base_UserInfoDAL = DALContainer.Resolve<IBase_UserInfoDAL>();
        public override void SetDal()
        {
            Dal = Base_UserInfoDAL;
        }
    }
}
