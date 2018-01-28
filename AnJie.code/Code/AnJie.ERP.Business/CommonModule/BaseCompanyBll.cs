using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 公司管理
    /// </summary>
    public class BaseCompanyBll : RepositoryFactory<Base_Company>
    {
        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns></returns>
        public List<Base_Company> GetList()
        {
            StringBuilder whereSql = new StringBuilder();
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                whereSql.Append(" AND ( CompanyId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                whereSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                whereSql.Append(" ) )");
            }
            whereSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindList(whereSql.ToString());
        }
    }
}