using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FMHJJService.DAL.IDAL;

namespace FMHJJService.BLL.Base
{
    public abstract partial class BaseService<T> where T : class, new()
    {
        protected string containerName;

        public BaseService()
        {
            SetDal();
        }
 
        public IBaseDAL<T> Dal{get;set;}
 
        public abstract void SetDal();

        public bool Add(T t)
        {
            return Dal.Add(t);
        }
        public bool Delete(T t)
        {
            return Dal.Delete(t);
        }
        public bool Update(T t)
        {
            return Dal.Update(t);
        }
        public int GetCount(Expression<Func<T, bool>> whereLambda)
        {
            return Dal.GetCount(whereLambda);
        }
        public IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return Dal.GetModels(whereLambda);
        }
 
        public IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc,
            Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda, out int recordCount)
        {
            return Dal.GetModelsByPage(pageSize, pageIndex, isAsc, OrderByLambda, WhereLambda,out recordCount);
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return Dal.ExecuteSqlCommand(sql,parameters);
        }

        public IQueryable<T> SqlQuery(string sql ,params object[] parameters)
        {
            return Dal.SqlQuery(sql, parameters);
        }
    }
}
