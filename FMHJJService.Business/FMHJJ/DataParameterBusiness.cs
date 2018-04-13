using FMHJJService.BLL.Container;
using FMHJJService.BLL.IBLL.FMHJJ;
using FMHJJService.Common;
using FMHJJService.DAL;
using FMHJJService.Model.CustomModel.FMHJJ;
using FMHJJService.Model.FMHJJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FMHJJService.Business.FMHJJ
{
    public class DataParameterBusiness
    {
        private static IData_ParametersService data_ParametersService = BLLContainer.Resolve<IData_ParametersService>();

        private static List<DataParameterModel> listDataParameter = null;

        public static List<DataParameterModel> DataParameterList
        {
            get {
                if (listDataParameter == null) Init();
                return listDataParameter;
            }
        }

        public static void Init()
        {
            var data_Parameter = data_ParametersService.GetModels(p => 1 == 1).ToList();
            listDataParameter = Utils.GetCopyList<DataParameterModel, Data_Parameters>(data_Parameter);
        }

        public static string GetParameterValue(string parameterName)
        {
            if (DataParameterList.Any(p => p.ParameterName.Equals(parameterName, StringComparison.OrdinalIgnoreCase)))
            {
                return DataParameterList.Where(p => p.ParameterName.Equals(parameterName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().ParameterValue;
            }

            return string.Empty;
        }
    }
}
