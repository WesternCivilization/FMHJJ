using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.Model.CustomModel.FMHJJ
{
    public class HistoryBiddingInfoModel
    {
        public int BidID { get; set; }
        public List<Nullable<int>> UserIDs { get; set; }
        public Nullable<DateTime> BidDate { get; set; }
        public Nullable<int> ProductType { get; set; }
        public Nullable<decimal> EstimateAmount { get; set; }
        public Nullable<decimal> BidPriceMin { get; set; }
        public Nullable<decimal> DealTotalAmount { get; set; }
        public Nullable<decimal> DealTotalPrice { get; set; }
        public Nullable<decimal> DealAvgPrice { get { return (DealTotalAmount != 0) ? Math.Round((decimal)(DealTotalPrice / DealTotalAmount), 2, MidpointRounding.AwayFromZero) : 0M; } }
    }
}
