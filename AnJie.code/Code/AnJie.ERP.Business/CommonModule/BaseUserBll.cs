using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using AnJie.ERP.Cache;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// �û�����
    /// </summary>
    public class BaseUserBll : RepositoryFactory<Base_User>
    {
        /// <summary>
        /// ��ȡ�û��б�
        /// </summary>
        /// <param name="keyword">ģ���ѯ</param>
        /// <param name="companyId">��˾ID</param>
        /// <param name="departmentId">����ID</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetPageList(string keyword, string companyId, string departmentId, ref JqGridParam jqgridparam)
        {
            var strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    u.UserId, u.Code, u.Account, u.RealName, u.Spell, u.Gender,
                                                u.Mobile, u.Telephone, u.Email, u.CompanyId,
                                                c.FullName AS CompanyName, u.DepartmentId,
                                                d.FullName AS DepartmentName, u.Enabled, u.LogOnCount,
                                                u.LastVisit, u.SortCode, u.CreateUserId, u.Remark
                                      FROM      Base_User u
                                                LEFT JOIN Base_Company c ON c.CompanyId = u.CompanyId
                                                LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                                    ) T
                            WHERE   1 = 1");
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@" AND (RealName LIKE @keyword
                                    OR Account LIKE @keyword
                                    OR Code LIKE @keyword
                                    OR Spell LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            if (!string.IsNullOrEmpty(companyId))
            {
                strSql.Append(" AND CompanyId = @CompanyId");
                parameter.Add(DbFactory.CreateDbParameter("@CompanyId", companyId));
            }
            if (!string.IsNullOrEmpty(departmentId))
            {
                strSql.Append(" AND DepartmentId = @DepartmentId");
                parameter.Add(DbFactory.CreateDbParameter("@DepartmentId", departmentId));
            }
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( UserId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        /// <summary>
        /// �ж��Ƿ����ӷ�����
        /// </summary>
        /// <returns></returns>
        public bool IsLinkServer()
        {
            var strSql = new StringBuilder();
            strSql.Append("SELECT  GETDATE()");
            var dt = Repository().FindTableBySql(strSql.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// ��½��֤��Ϣ
        /// </summary>
        /// <param name="account">�˻�</param>
        /// <param name="password">����</param>
        /// <param name="result">���ؽ��</param>
        /// <returns></returns>
        public Base_User UserLogin(string account, string password, out string result)
        {
            if (!this.IsLinkServer())
            {
                throw new Exception("���������Ӳ��ϣ�" + DbResultMsg.ReturnMsg);
            }
            var entity = Repository().FindEntity("Account", account);
            if (entity != null && entity.UserId != null)
            {
                if (entity.Enabled == 1)
                {
                    string dbPassword =
                        Md5Helper.Md5(DESEncrypt.Encrypt(password.ToLower(), entity.Secretkey).ToLower(), 32).ToLower();
                    if (dbPassword == entity.Password)
                    {
                        DateTime previousVisit = CommonHelper.GetDateTime(entity.LastVisit);
                        DateTime lastVisit = DateTime.Now;
                        int logOnCount = CommonHelper.GetInt(entity.LogOnCount) + 1;
                        entity.PreviousVisit = previousVisit;
                        entity.LastVisit = lastVisit;
                        entity.LogOnCount = logOnCount;
                        entity.Online = 1;
                        Repository().Update(entity);
                        result = "succeed";
                        DataCache.RemoveAllCache("ActionAuthorizeList_" + entity.UserId);
                    }
                    else
                    {
                        result = "error";
                    }
                }
                else
                {
                    result = "lock";
                }
                return entity;
            }
            result = "-1";
            return null;
        }

        /// <summary>
        /// ��ȡ�û���ɫ�б�
        /// </summary>
        /// <param name="companyId">��˾ID</param>
        /// <param name="userId">�û�Id</param>
        /// <returns></returns>
        public DataTable UserRoleList(string companyId, string userId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  r.RoleId ,				--��ɫID
                                    r.Code ,				--����
                                    r.FullName ,			--����
                                    r.SortCode ,			--������
                                    ou.ObjectId				--�Ƿ����
                            FROM    Base_Roles r
                                    LEFT JOIN Base_ObjectUserRelation ou ON ou.ObjectId = r.RoleId
                                                                            AND ou.UserId = @UserId
                                                                            AND ou.Category = 2
                            WHERE 1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( RoleId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" AND r.CompanyId = @CompanyId");
            parameter.Add(DbFactory.CreateDbParameter("@UserId", userId));
            parameter.Add(DbFactory.CreateDbParameter("@CompanyId", companyId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// ѡ���û��б�
        /// </summary>
        /// <param name="keyword">ģ���ѯ</param>
        /// <returns></returns>
        public DataTable OptionUserList(string keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@"SELECT TOP 50
                                        *
                                FROM    ( SELECT    u.UserId, u.Account, u.Code, u.RealName, u.DepartmentId,
                                                    d.FullName AS DepartmentName, u.Gender
                                          FROM      Base_User u
                                                    LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                                          WHERE     u.RealName LIKE @keyword
                                                    OR u.Account LIKE @keyword
                                                    OR u.Code LIKE @keyword
                                                    OR u.Spell LIKE @keyword
                                                    OR u.UserId IN (
                                                    SELECT  u.UserId
                                                    FROM    Base_User u
                                                            INNER JOIN Base_ObjectUserRelation oc ON u.UserId = oc.UserId
                                                            INNER JOIN dbo.Base_Company c ON c.CompanyId = oc.ObjectId
                                                    WHERE   c.FullName LIKE @keyword
                                                    UNION
                                                    SELECT  u.UserId
                                                    FROM    Base_User u
                                                            INNER JOIN Base_ObjectUserRelation od ON u.UserId = od.UserId
                                                            INNER JOIN Base_Department d ON d.DepartmentId = od.ObjectId
                                                    WHERE   d.FullName LIKE @keyword
                                                    UNION
                                                    SELECT  u.UserId
                                                    FROM    Base_User u
                                                            INNER JOIN Base_ObjectUserRelation oro ON u.UserId = oro.UserId
                                                            INNER JOIN Base_Roles r ON r.RoleId = oro.ObjectId
                                                    WHERE   r.FullName LIKE @keyword )
                                        ) a
                                WHERE   1 = 1");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            else
            {
                strSql.Append(@"SELECT TOP 50
                                        u.UserId, u.Account, u.Code, u.RealName, u.DepartmentId,
                                        d.FullName AS DepartmentName, u.Gender
                                FROM    Base_User u
                                        LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                                WHERE   1 = 1");
            }
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( UserId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}