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
	public class Flw_BiddingInfoService : BaseService<Flw_BiddingInfo>, IFlw_BiddingInfoService
	{
		private IFlw_BiddingInfoDAL flw_BiddingInfoDAL = DALContainer.Resolve<IFlw_BiddingInfoDAL>();
        public override void SetDal()
        {
            Dal = flw_BiddingInfoDAL;
        }
	} 
}