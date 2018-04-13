using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.DAL.IDAL
{
    public partial interface IBaseDAL<T> where T : class, new()
    {
        bool Add(T t);
        bool Delete(T t);
        bool Update(T t);
        int GetCount(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda,out int recordCount);
        /// <summary>
        /// 一个业务中有可能涉及到对多张表的操作,那么可以将操作的数据,打上相应的标记,最后调用该方法,将数据一次性提交到数据库中,避免了多次链接数据库。
        /// </summary>
        bool SaveChanges();
        int ExecuteSqlCommand(string sql,params object[] parameters);
        IQueryable<T> SqlQuery(string sql,params object[] parameters);
    }
}
