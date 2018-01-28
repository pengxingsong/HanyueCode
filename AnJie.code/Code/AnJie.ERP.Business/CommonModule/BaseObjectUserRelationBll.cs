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
    /// 对象用户关系表
    /// </summary>
    public class BaseObjectUserRelationBll : RepositoryFactory<Base_ObjectUserRelation>
    {
        /// <summary>
        /// 成员列表
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="departmentId">部门ID</param>
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// <returns></returns>
        public DataTable GetList(string companyId, string departmentId, string objectId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    u.UserId, u.Account, u.RealName, u.Code, u.Gender,
                                                u.CompanyId, u.DepartmentId, u.SortCode, ou.ObjectId
                                      FROM      Base_User u
                                                LEFT JOIN Base_ObjectUserRelation ou ON ou.UserId = u.UserId
                                                                                        AND ou.ObjectId = @ObjectId
                                                                                        AND ou.Category = @Category
                                    ) T
                            WHERE   1 = 1");
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
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", objectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", category));
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( UserId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" ORDER BY ObjectId DESC,SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 批量添加成员
        /// </summary>
        /// <param name="arrayUserId">选中用户ID: 1,2,3,4,5,6</param>
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// <returns></returns>
        public int BatchAddMember(string[] arrayUserId, string objectId, string category)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                StringBuilder sbDelete =
                    new StringBuilder(
                        "DELETE FROM Base_ObjectUserRelation WHERE ObjectId = @ObjectId AND Category=@Category");
                List<DbParameter> parameter = new List<DbParameter>
                {
                    DbFactory.CreateDbParameter("@ObjectId", objectId),
                    DbFactory.CreateDbParameter("@Category", category)
                };
                database.ExecuteBySql(sbDelete, parameter.ToArray(), isOpenTrans);
                int index = 1;
                foreach (string item in arrayUserId)
                {
                    if (item.Length > 0)
                    {
                        var entity = new Base_ObjectUserRelation
                        {
                            ObjectUserRelationId = CommonHelper.GetGuid,
                            ObjectId = objectId,
                            UserId = item,
                            Category = category,
                            SortCode = index
                        };
                        entity.Create();
                        index++;
                        database.Insert(entity, isOpenTrans);
                    }
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

        /// <summary>
        /// 批量添加对象用户关系
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="arrayObjectId">对象ID</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// <returns></returns>
        public int BatchAddObject(string userId, string[] arrayObjectId, string category)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                StringBuilder sbDelete =
                    new StringBuilder(
                        "DELETE FROM Base_ObjectUserRelation WHERE UserId = @UserId AND Category=@Category");
                List<DbParameter> parameter = new List<DbParameter>
                {
                    DbFactory.CreateDbParameter("@UserId", userId),
                    DbFactory.CreateDbParameter("@Category", category)
                };
                database.ExecuteBySql(sbDelete, parameter.ToArray(), isOpenTrans);
                int index = 1;
                foreach (string item in arrayObjectId)
                {
                    if (item.Length > 0)
                    {
                        var entity = new Base_ObjectUserRelation
                        {
                            ObjectUserRelationId = CommonHelper.GetGuid,
                            UserId = userId,
                            ObjectId = item,
                            Category = category,
                            SortCode = index
                        };
                        entity.Create();
                        index++;
                        database.Insert(entity, isOpenTrans);
                    }
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

        /// <summary>
        /// 根据用户ID查询对象列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<Base_ObjectUserRelation> GetList(string userId)
        {
            return Repository().FindList("UserId", userId);
        }

        /// <summary>
        /// 根据用户ID查询对象列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public string GetObjectId(string userId)
        {
            StringBuilder sbObjectId = new StringBuilder();
            List<Base_ObjectUserRelation> list = GetList(userId);
            if (list.Count > 0)
            {
                foreach (Base_ObjectUserRelation item in list)
                {
                    sbObjectId.Append(item.ObjectId + ",");
                }
                sbObjectId.Append(userId);
            }
            return sbObjectId.ToString();
        }

        /// <summary>
        /// 根据对象Id 获取用户列表
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public List<Base_User> GetUserList(string objectId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  u.UserId, u.Account, u.RealName, u.Code, u.Gender, u.CompanyId,
                                    u.DepartmentId, u.SortCode
                            FROM    Base_User u
                                    INNER JOIN Base_ObjectUserRelation ou ON ou.UserId = u.UserId
                                                                             AND ou.ObjectId = @ObjectId");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", objectId));
            return DataFactory.Database().FindListBySql<Base_User>(strSql.ToString(), parameter.ToArray());
        }
    }
}