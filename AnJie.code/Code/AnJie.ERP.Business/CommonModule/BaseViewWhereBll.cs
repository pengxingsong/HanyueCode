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
    /// ��ͼ��ѯ������
    /// </summary>
    public class BaseViewWhereBll : RepositoryFactory<Base_ViewWhere>
    {
        /// <summary>
        /// ����ģ��Id��ȡ��ͼ��ѯ�����б�
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