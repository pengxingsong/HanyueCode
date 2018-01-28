using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// ���ݷ�ΧȨ�ޱ�
    /// </summary>
    public class BaseDataScopePermissionBLL : RepositoryFactory<Base_DataScopePermission>
    {
        private static BaseDataScopePermissionBLL _item;

        /// <summary>
        /// ��̬��
        /// </summary>
        public static BaseDataScopePermissionBLL Instance
        {
            get
            {
                if (_item == null)
                {
                    _item = new BaseDataScopePermissionBLL();
                }
                return _item;
            }
        }

        /// <summary>
        /// �½�����Ŀ���ݣ�Ĭ�ϰ�����Ȩ�������������ͱ��ҪҪȥ����Ȩ�޹�������ȥ��
        /// </summary>
        /// <param name="moduleId">ģ������</param>
        /// <param name="objectId">��������</param>
        /// <param name="resourceId">��ʲô��Դ</param>
        /// <param name="isOpenTrans">����</param>
        public void AddScopeDefault(string moduleId, string objectId, string resourceId, DbTransaction isOpenTrans = null)
        {
            Base_DataScopePermission entity = new Base_DataScopePermission();
            entity.Create();
            entity.ModuleId = moduleId;
            entity.ObjectId = objectId;
            entity.Category = "5";
            entity.ResourceId = resourceId;
            if (isOpenTrans != null)
            {
                Repository().Insert(entity, isOpenTrans);
            }
            else
            {
                Repository().Insert(entity);
            }
        }

        #region ��˾����
        /// <summary>
        /// ���ع�˾�б�
        /// <param name="moduleId">ģ������</param>
        /// <param name="objectId">��������</param>
        /// <param name="category">�������:1-����2-��ɫ</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeCompanyList(string moduleId, string objectId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  c.CompanyId ,				--��˾ID
                                    c.ParentId ,				--��˾�ڵ�
                                    c.Code ,					--����
                                    c.FullName ,				--����
                                    c.SortCode ,				--������
                                    dsp.ObjectId				--�Ƿ����
                            FROM    Base_Company c
                                    LEFT JOIN Base_DataScopePermission dsp ON c.CompanyId = dsp.ResourceId
												                                AND dsp.ObjectId = @ObjectId
                                                                                AND dsp.Category = @Category
                                                                                AND dsp.ModuleId = @ModuleId");
            strSql.Append(" WHERE   1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( CompanyId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" order by c.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", objectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", category));
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", moduleId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        #endregion

        #region ���Ź���
        /// <summary>
        /// ���ز����б�
        /// <param name="moduleId">ģ������</param>
        /// <param name="objectId">��������</param>
        /// <param name="category">�������:1-����2-��ɫ</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeDepartmentList(string moduleId, string objectId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                            FROM    ( SELECT    CompanyId ,				--��˾ID
                                                CompanyId AS DepartmentId ,--����ID
                                                Code ,					--����
                                                FullName ,				--����
                                                ParentId ,				--�ڵ�ID
                                                SortCode ,				--�������
                                                'Company' AS Sort		--����
                                      FROM      Base_Company			--��˾��
                                      UNION
                                      SELECT    CompanyId ,				--��˾ID
                                                DepartmentId ,			--����ID
                                                Code ,					--����
                                                FullName ,				--����
                                                CompanyId AS ParentId ,	--�ڵ�ID
                                                SortCode ,				--�������
                                                'Department' AS Sort	--����
                                      FROM      Base_Department			--���ű�
          
                                    ) T
                                    LEFT JOIN Base_DataScopePermission dsp ON T.DepartmentId = dsp.ResourceId
                                                                              AND dsp.ObjectId = @ObjectId
                                                                              AND dsp.Category = @Category
                                                                              AND dsp.ModuleId = @ModuleId");
            strSql.Append(" WHERE 1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( DepartmentId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" order by T.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", objectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", category));
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", moduleId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        #endregion

        #region �ֿ����
        /// <summary>
        /// ���زֿ��б�
        /// <param name="moduleId">ģ������</param>
        /// <param name="objectId">��������</param>
        /// <param name="category">�������:1-����2-��ɫ</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeWarehouseList(string moduleId, string objectId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                           FROM    ( SELECT    CompanyId ,				--��˾ID
                                                CompanyId AS WarehouseId ,--����ID
                                                Code ,					--����
                                                FullName ,				--����
                                                ParentId ,				--�ڵ�ID
                                                SortCode ,				--�������
                                                'Company' AS Sort		--����
                                      FROM      Base_Company			--��˾��
                                      UNION
                                      SELECT    CompanyId ,				--��˾ID
                                                WarehouseId ,			--�ֿ�ID
                                                WarehouseCode AS Code ,					--����
                                                WarehouseName AS FullName ,				--����
                                                CompanyId AS ParentId ,	--�ڵ�ID
                                                SortCode ,				--�������
                                                'Warehouse' AS Sort	--����
                                      FROM      dbo.Warehouse			--�ֿ��          
                                    ) T
                                    LEFT JOIN Base_DataScopePermission dsp ON T.WarehouseId = dsp.ResourceId
                                                                              AND dsp.ObjectId = @ObjectId
                                                                              AND dsp.Category = @Category
                                                                              AND dsp.ModuleId = @ModuleId");
            strSql.Append(" WHERE 1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( WarehouseId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" order by T.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", objectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", category));
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", moduleId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        #endregion

        #region ��ɫ����
        /// <summary>
        /// ���ؽ�ɫ�б�
        /// <param name="moduleId">ģ������</param>
        /// <param name="objectId">��������</param>
        /// <param name="category">�������:1-����2-��ɫ</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeRoleList(string moduleId, string objectId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                            FROM    ( SELECT    CompanyId ,				--��˾ID
                                                CompanyId AS RoleId ,	--��ɫID
                                                Code ,					--����
                                                FullName ,				--����
                                                ParentId ,				--�ڵ�ID
                                                SortCode ,				--�������
                                                'Company' AS Sort		--����
                                      FROM      Base_Company			--��˾��
                                      UNION
                                      SELECT    CompanyId ,				--��˾ID
                                                RoleId ,				--��ɫID
                                                Code ,					--����
                                                FullName ,				--����
                                                CompanyId AS ParentId ,	--�ڵ�ID
                                                SortCode ,				--�������
                                                'Roles' AS Sort			--����
                                      FROM      Base_Roles
                                    ) T
                                    LEFT JOIN Base_DataScopePermission dsp ON T.RoleId = dsp.ResourceId
                                                                              AND dsp.ObjectId = @ObjectId
                                                                              AND dsp.Category = @Category
                                                                              AND dsp.ModuleId = @ModuleId");
            strSql.Append(" WHERE 1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( RoleId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" order by T.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", objectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", category));
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", moduleId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        #endregion

        #region �û�����
        /// <summary>
        /// �����û��б�
        /// <param name="moduleId">ģ������</param>
        /// <param name="objectId">��������</param>
        /// <param name="category">�������:1-����2-��ɫ</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeUserList(string moduleId, string objectId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                            FROM    ( SELECT    CompanyId AS Id ,		--��˾ID
                                                Code ,					--����
                                                FullName ,				--����
                                                ParentId ,				--�ڵ�ID
                                                '' AS Gender,			--�Ա�
                                                SortCode ,				--�������
                                                'Company' AS Sort		--����
                                      FROM      Base_Company			--��˾��
                                      UNION
                                      SELECT    DepartmentId AS Id,		--����ID
                                                Code ,					--����
                                                FullName ,				--����
                                                CompanyId AS ParentId ,	--�ڵ�ID
                                                '' AS Gender,			--�Ա�
                                                SortCode ,				--�������
                                                'Department' AS Sort	--����
                                      FROM      Base_Department			--���ű�
                                      UNION
                                      SELECT    UserId AS Id,			--�û�ID
                                                Code ,					--����
                                                RealName ,				--����
                                                DepartmentId AS ParentId ,--�ڵ�ID
                                                Gender,					--�Ա�
                                                SortCode ,				--�������
                                                'User' AS Sort			--����
                                      FROM      Base_User			    --�û���
          
                                    ) T
                                    LEFT JOIN Base_DataScopePermission dsp ON T.Id = dsp.ResourceId
                                                                              AND dsp.ObjectId = @ObjectId
                                                                              AND dsp.Category = @Category
                                                                              AND dsp.ModuleId = @ModuleId");
            strSql.Append(" WHERE 1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( Id IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" order by T.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", objectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", category));
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", moduleId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        #endregion
    }
}