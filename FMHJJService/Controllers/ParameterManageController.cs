using FMHJJService.BLL.Container;
using FMHJJService.BLL.IBLL.FMHJJ;
using FMHJJService.Business.FMHJJ;
using FMHJJService.Common;
using FMHJJService.DAL;
using FMHJJService.Model.CustomModel.FMHJJ;
using FMHJJService.Model.FMHJJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FMHJJService.Controllers
{
    [AuthorizeUser]
    public class ParameterManageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string GetJson()
        {
            var json = new JavaScriptSerializer();

            try
            {
                IData_ParametersService data_ParametersService = BLLContainer.Resolve<IData_ParametersService>();
                var data_Parameters = data_ParametersService.GetModels(p => 1 == 1).ToList();

                //将对象转换为JSON字符串      
                return json.Serialize(data_Parameters);
            }
            catch
            {
                return string.Empty;
            }
        }

        [HttpPost]
        public string SaveParameters(string json)
        {            
            //开启EF事务
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    //var dictParams = new Dictionary<string, object>((IDictionary<string, object>)Utils.DeserializeObject(json), StringComparer.OrdinalIgnoreCase);
                    var dictParams = (Dictionary<string, object>)Utils.DeserializeObject(json);
                    var dbContext = DbContextFactory.CreateByModelNamespace(typeof(Data_Parameters).Namespace);
                    foreach (var item in dictParams)
                    {
                        var data_Parameter = dbContext.Set<Data_Parameters>().Where(p => p.ControlID.Equals(item.Key, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        data_Parameter.ParameterValue = item.Value.ToString();
                        dbContext.Entry(data_Parameter).State = System.Data.Entity.EntityState.Modified;
                    }

                    if (dbContext.SaveChanges() < 0)
                    {
                        return "保存失败";
                    }

                    ts.Complete();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            //更新缓存
            DataParameterBusiness.Init();

            return "保存成功";
        }
    }
}