using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 视图查询条件表
    /// </summary>
    public class BaseViewWhereBll : RepositoryFactory<Base_ViewWhere>
    {
        /// <summary>
        /// 根据模块Id获取视图查询条件列表
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public List<Base_ViewWhere> GetViewWhereList(string moduleId)
        {
            StringBuilder whereSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            whereSql.Append(" AND ModuleId = @ModuleId");
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", moduleId));
            return Repository().FindList(whereSql.ToString(), parameter.ToArray());
        }
    }
}