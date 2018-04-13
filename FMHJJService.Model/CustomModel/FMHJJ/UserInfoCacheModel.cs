using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.Model.FMHJJ
{
    public partial class Base_UserInfo
    {
        public bool REMEMBER_ME { get; set; }
    }
}

namespace FMHJJService.Model.CustomModel.FMHJJ
{
    public class UserInfoCacheModel
    {
        public int ID { set; get; }

        public Nullable<int> UserType { set; get; }

        public Nullable<int> UserLvl { set; get; }

        public Nullable<int> ProductType { set; get; }

        public string UserName { set; get; }        

        public string Password { set; get; }

        public string CompanyName { set; get; }
    }
}
