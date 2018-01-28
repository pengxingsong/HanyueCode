using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 模块按钮
    /// </summary>
    public class BaseButtonBll : RepositoryFactory<Base_Button>
    {
        /// <summary>
        /// 获取按钮列表
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <param name="category">分类：1-工具栏，2：右击栏</param>
        /// <returns></returns>
        public List<Base_Button> GetList(string moduleId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_Button WHERE 1=1");
            strSql.Append(" AND ModuleId = @ModuleId ");
            strSql.Append(" AND Category = @Category ");
            strSql.Append(" ORDER BY SortCode ASC");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModuleId", moduleId),
                DbFactory.CreateDbParameter("@Category", category)
            };
            return Repository().FindListBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}
