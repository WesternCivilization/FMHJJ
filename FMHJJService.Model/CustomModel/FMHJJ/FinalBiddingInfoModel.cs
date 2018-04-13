using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.Model.CustomModel.FMHJJ
{
    public class FinalBiddingInfoModel
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<decimal> BidAmount { get; set; }
        public Nullable<decimal> BidPrice { get; set; }
        public Nullable<DateTime> BidTime { get; set; }
        public Nullable<decimal> DealAmount { get; set; }
        public Nullable<decimal> DealPrice { get; set; }
    }
}
