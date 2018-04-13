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
	public class Flw_BiddingNoticeService : BaseService<Flw_BiddingNotice>, IFlw_BiddingNoticeService
	{
		private IFlw_BiddingNoticeDAL flw_BiddingNoticeDAL = DALContainer.Resolve<IFlw_BiddingNoticeDAL>();
        public override void SetDal()
        {
            Dal = flw_BiddingNoticeDAL;
        }
	} 
}