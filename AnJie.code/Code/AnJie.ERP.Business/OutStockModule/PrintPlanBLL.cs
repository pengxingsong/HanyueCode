using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using AnJie.ERP.DataAccess;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// Print_Plan
    /// </summary>
    public class PrintPlanBLL : RepositoryFactory<PrintPlanEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PrintPlanEntity> GetList()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM [Print_Plan] ORDER BY CreateDate");
            return DataFactory.Database().FindListBySql<PrintPlanEntity>(strSql.ToString());
        }
    }
}