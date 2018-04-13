using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using FMHJJService.Common;
using FMHJJService.Model;
using FMHJJService.Model.FMHJJ;
using FMHJJService.Model.CustomModel.FMHJJ;

namespace FMHJJService.DAL
{
    public partial class DbContextFactory
    {
        /// <summary>
        /// 创建EF上下文对象,已存在就直接取,不存在就创建,保证线程内是唯一。
        /// </summary>
        public static DbContext CreateByModelNamespace(string modelNamespace)
        {
            string dbName = GetDBNameByModelNamespace(modelNamespace);
            DbContext dbContext = CallContext.GetData(dbName) as DbContext;
            if (dbContext == null)
            {
                dbContext = GetDbContextByModelNamespace(modelNamespace);
                CallContext.SetData(dbName, dbContext);
            }
            return dbContext;
        }
              
        private static DbContext GetDbContextByModelNamespace(string modelNamespace)
        {
            switch (modelNamespace)
            {
                case "FMHJJService.Model.FMHJJ": return new FMHJJDBEntities();
                case "FMHJJService.Model.CustomModel.FMHJJ": return new FMHJJCustomEntities();
                default: return null;
            }
        }
    
        private static string GetDBNameByModelNamespace(string modelNamespace)
        {
            switch (modelNamespace)
            {
                case "FMHJJService.Model.FMHJJ": return "FMHJJDBEntities";
                case "FMHJJService.Model.CustomModel.FMHJJ": return "FMHJJCustomEntities";
                default:return "";
            }
        }
    }
}
