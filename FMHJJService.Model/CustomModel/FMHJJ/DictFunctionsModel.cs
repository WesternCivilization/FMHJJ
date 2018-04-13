using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.Model.CustomModel.FMHJJ
{
    public class DictFunctionsModel
    {
        public int ID { get; set; }

        public Nullable<int> PID { get; set; }

        public string FunctionName { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }
    }

    public class UserFunctionsModel
    {
        public int ID { get; set; }

        public Nullable<int> PID { get; set; }

        public string FunctionName { get; set; }

        public Nullable<int> IsAllow { get; set; }
    }
}
