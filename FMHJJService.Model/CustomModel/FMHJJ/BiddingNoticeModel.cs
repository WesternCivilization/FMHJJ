using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.Model.CustomModel.FMHJJ
{
    public class BiddingNoticeModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public Nullable<int> NoticeType { get; set; }
        public string NoticeTypeStr { get; set; }
        public Nullable<System.DateTime> PublishTime { get; set; }
        public string Publisher { get; set; }
        public string PublishContent { get; set; }
    }
}
