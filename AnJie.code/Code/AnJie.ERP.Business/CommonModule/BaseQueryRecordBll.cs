using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// ��ѯ������¼
    /// </summary>
    public class BaseQueryRecordBll : RepositoryFactory<Base_QueryRecord>
    {
        /// <summary>
        /// ����������ȡ�����б�
        /// </summary>
        /// <param name="moduleId">ģ��ID</param>
        /// <param name="createUserId">�û�ID</param>
        /// <returns></returns>
        public List<Base_QueryRecord> GetList(string moduleId, string createUserId)
        {
            StringBuilder whereSql = new StringBuilder();
            whereSql.Append(" AND CreateUserId = @CreateUserId ");
            whereSql.Append(" AND ModuleId = @ModuleId Order By CreateDate Desc");
            List<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@CreateUserId", createUserId),
                DbFactory.CreateDbParameter("@ModuleId", moduleId)
            };
            return DataFactory.Database().FindList<Base_QueryRecord>(whereSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// ���ó�ʼ��Ĭ�Ϸ���
        /// </summary>
        /// <param name="moduleId">ģ��ID</param>
        /// <param name="queryRecordId">����</param>
        /// <returns></returns>
        public int DefaultProject(string moduleId, string queryRecordId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(string.Format("UPDATE Base_QueryRecord SET NextDefault = 0 WHERE ModuleId = '{0}'",
                    moduleId));
                database.ExecuteBySql(strSql, isOpenTrans);
                Base_QueryRecord entity = new Base_QueryRecord();
                if (!string.IsNullOrEmpty(queryRecordId))
                {
                    entity.QueryRecordId = queryRecordId;
                    entity.NextDefault = 1;
                    database.Update(entity, isOpenTrans);
                }
                database.Commit();
                return 1;
            }
            catch
            {
                database.Rollback();
                return -1;
            }
        }
    }
}