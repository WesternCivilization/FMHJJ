using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMHJJService.DAL.CommonDAL;

namespace FMHJJService.BLL.CommonBLL
{
    public class FMHJJBLL
    {
        public static DataSet ExecuteQuerySentence(string strSQL, SqlParameter[] paramArr)
        {
            return new DataAccess(DataAccessDB.FMHJJDB).ExecuteQuerySentence(strSQL, paramArr);
        }

        public static string ExecuteScalar(string strSQL)
        {
            return new DataAccess(DataAccessDB.FMHJJDB).ExecuteScalar(strSQL);
        }

        public static void ExecuteStoredProcedureSentence(string storedProcedureName, SqlParameter[] paramArr)
        {
            new DataAccess(DataAccessDB.FMHJJDB).ExecuteStoredProcedureSentence(storedProcedureName, paramArr);
        }
    }
}
