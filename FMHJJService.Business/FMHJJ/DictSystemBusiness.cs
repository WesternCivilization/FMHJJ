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
    public class DictSystemBusiness
    {
        private static IDict_SystemService dict_SystemService = BLLContainer.Resolve<IDict_SystemService>();

        private static List<DictSystemModel> listDictSystem = null;

        public static List<DictSystemModel> DictSystemList
        {
            get {
                if (listDictSystem == null) Init();
                return listDictSystem;
            }
        }

        public static void Init()
        {
            string sql = "select a.*,b.dictkey as PDictKey from Dict_System a left join Dict_System b on a.pid=b.id";
            var dbContext = DbContextFactory.CreateByModelNamespace(typeof(Dict_System).Namespace);
            listDictSystem = dbContext.Database.SqlQuery<DictSystemModel>(sql).ToList();
        }

        public static bool IsEffectiveDict(int id)
        {
            return DictSystemList.Any<DictSystemModel>(p => p.ID == id);
        }

        public static bool IsEffectiveDict(string dict_key)
        {
            return DictSystemList.Any<DictSystemModel>(p => p.DictKey.Equals(dict_key, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsEffectiveDict(string pdict_key, string dict_key)
        {
            return DictSystemList.Any<DictSystemModel>(p => p.PDictKey != null && p.PDictKey.Equals(pdict_key, StringComparison.OrdinalIgnoreCase) &&
                p.DictKey.Equals(dict_key, StringComparison.OrdinalIgnoreCase));
        }

        public static List<DictSystemModel> GetDicts(string pdict_key)
        {
            return DictSystemList.Where<DictSystemModel>(p => p.PDictKey != null && p.PDictKey.Equals(pdict_key, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static string GetDictKey(string pdict_key, string dict_value)
        {
            var dictSystemModel = DictSystemList.Where<DictSystemModel>(p => p.PDictKey != null && p.PDictKey.Equals(pdict_key, StringComparison.OrdinalIgnoreCase) && 
                p.DictValue.Equals(dict_value, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return dictSystemModel != null ? dictSystemModel.DictKey : string.Empty;
        }

        public static DictSystemModel GetDictModel(string dict_key)
        {
            return DictSystemList.Where<DictSystemModel>(p => p.DictKey.Equals(dict_key, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public static bool AddModel(string pdict_key, string dict_key, out string errMsg)
        {
            errMsg = string.Empty;

            //开启EF事务
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    if (!DictSystemBusiness.IsEffectiveDict("客户等级"))
                    {
                        errMsg = "关键字类别不在字典表中";
                        return false;
                    }

                    if (DictSystemBusiness.IsEffectiveDict("客户等级", dict_key))
                    {
                        errMsg = "关键字已存在字典表中";
                        return false;
                    }

                    var dictSystemList = DictSystemBusiness.GetDicts("客户等级");
                    var no = 0;
                    var dictValue = dictSystemList.Max(p => p.DictValue);
                    int.TryParse(dictValue, out no);
                    no++;
                    var dictSystemMode = new DictSystemModel();
                    dictSystemMode.DictKey = dict_key;
                    dictSystemMode.PID = DictSystemBusiness.GetDictModel("客户等级").ID;
                    dictSystemMode.DictValue = no.ToString();

                    var dict_System = Utils.GetCopy<Dict_System, DictSystemModel>(dictSystemMode);
                    if (!dict_SystemService.Add(dict_System))
                    {
                        errMsg = "数据库更新失败";
                        return false;
                    }

                    dictSystemMode.ID = dict_System.ID;
                    dictSystemMode.PDictKey = pdict_key;
                    DictSystemList.Add(dictSystemMode);

                    ts.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                    return false;
                }
            }
                
            return true;
        }

        public static bool DelModel(int id, out string errMsg)
        {
            errMsg = string.Empty;

            //开启EF事务
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    if (!DictSystemList.Any(p => p.ID == id))
                    {
                        errMsg = "关键字不在字典表中";
                        return false;
                    }

                    var dictSystemMode = DictSystemList.Where(p => p.ID == id).FirstOrDefault();

                    if (!dict_SystemService.Delete(Utils.GetCopy<Dict_System, DictSystemModel>(dictSystemMode)))
                    {
                        errMsg = "数据库更新失败";
                        return false;
                    }

                    if (!DictSystemList.Remove(dictSystemMode))
                    {
                        errMsg = "缓存更新失败";
                        return false;
                    }

                    ts.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                    return false;
                }
            }

            return true;
        }

        public static bool UpdateModel(int id, string dict_key, out string errMsg)
        {
            errMsg = string.Empty;

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    if (!DictSystemList.Any(p => p.ID == id))
                    {
                        errMsg = "关键字不在字典表中";
                        return false;
                    }

                    var dictSystemMode = DictSystemList.Where(p => p.ID == id).FirstOrDefault();
                    dictSystemMode.DictKey = dict_key;
                    if (!dict_SystemService.Update(Utils.GetCopy<Dict_System, DictSystemModel>(dictSystemMode)))
                    {
                        errMsg = "数据库更新失败";
                        return false;
                    }

                    ts.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                    return false;
                }
            }
                        
            return true;
        }
    }
}
