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
    /// 数据范围权限表
    /// </summary>
    public class BaseDataScopePermissionBLL : RepositoryFactory<Base_DataScopePermission>
    {
        private static BaseDataScopePermissionBLL _item;

        /// <summary>
        /// 静态化
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
        /// 新建的项目数据，默认把数据权限设置了这样就别必要要去数据权限管理里面去打钩
        /// </summary>
        /// <param name="moduleId">模块主键</param>
        /// <param name="objectId">对象主键</param>
        /// <param name="resourceId">对什么资源</param>
        /// <param name="isOpenTrans">事务</param>
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

        #region 公司管理
        /// <summary>
        /// 加载公司列表
        /// <param name="moduleId">模块主键</param>
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeCompanyList(string moduleId, string objectId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  c.CompanyId ,				--公司ID
                                    c.ParentId ,				--公司节点
                                    c.Code ,					--编码
                                    c.FullName ,				--名称
                                    c.SortCode ,				--排序吗
                                    dsp.ObjectId				--是否存在
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

        #region 部门管理
        /// <summary>
        /// 加载部门列表
        /// <param name="moduleId">模块主键</param>
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeDepartmentList(string moduleId, string objectId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                            FROM    ( SELECT    CompanyId ,				--公司ID
                                                CompanyId AS DepartmentId ,--部门ID
                                                Code ,					--编码
                                                FullName ,				--名称
                                                ParentId ,				--节点ID
                                                SortCode ,				--排序编码
                                                'Company' AS Sort		--分类
                                      FROM      Base_Company			--公司表
                                      UNION
                                      SELECT    CompanyId ,				--公司ID
                                                DepartmentId ,			--部门ID
                                                Code ,					--编码
                                                FullName ,				--名称
                                                CompanyId AS ParentId ,	--节点ID
                                                SortCode ,				--排序编码
                                                'Department' AS Sort	--分类
                                      FROM      Base_Department			--部门表
          
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

        #region 仓库管理
        /// <summary>
        /// 加载仓库列表
        /// <param name="moduleId">模块主键</param>
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeWarehouseList(string moduleId, string objectId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                           FROM    ( SELECT    CompanyId ,				--公司ID
                                                CompanyId AS WarehouseId ,--部门ID
                                                Code ,					--编码
                                                FullName ,				--名称
                                                ParentId ,				--节点ID
                                                SortCode ,				--排序编码
                                                'Company' AS Sort		--分类
                                      FROM      Base_Company			--公司表
                                      UNION
                                      SELECT    CompanyId ,				--公司ID
                                                WarehouseId ,			--仓库ID
                                                WarehouseCode AS Code ,					--编码
                                                WarehouseName AS FullName ,				--名称
                                                CompanyId AS ParentId ,	--节点ID
                                                SortCode ,				--排序编码
                                                'Warehouse' AS Sort	--分类
                                      FROM      dbo.Warehouse			--仓库表          
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

        #region 角色管理
        /// <summary>
        /// 加载角色列表
        /// <param name="moduleId">模块主键</param>
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeRoleList(string moduleId, string objectId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                            FROM    ( SELECT    CompanyId ,				--公司ID
                                                CompanyId AS RoleId ,	--角色ID
                                                Code ,					--编码
                                                FullName ,				--名称
                                                ParentId ,				--节点ID
                                                SortCode ,				--排序编码
                                                'Company' AS Sort		--分类
                                      FROM      Base_Company			--公司表
                                      UNION
                                      SELECT    CompanyId ,				--公司ID
                                                RoleId ,				--角色ID
                                                Code ,					--编码
                                                FullName ,				--名称
                                                CompanyId AS ParentId ,	--节点ID
                                                SortCode ,				--排序编码
                                                'Roles' AS Sort			--分类
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

        #region 用户管理
        /// <summary>
        /// 加载用户列表
        /// <param name="moduleId">模块主键</param>
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeUserList(string moduleId, string objectId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                            FROM    ( SELECT    CompanyId AS Id ,		--公司ID
                                                Code ,					--编码
                                                FullName ,				--名称
                                                ParentId ,				--节点ID
                                                '' AS Gender,			--性别
                                                SortCode ,				--排序编码
                                                'Company' AS Sort		--分类
                                      FROM      Base_Company			--公司表
                                      UNION
                                      SELECT    DepartmentId AS Id,		--部门ID
                                                Code ,					--编码
                                                FullName ,				--名称
                                                CompanyId AS ParentId ,	--节点ID
                                                '' AS Gender,			--性别
                                                SortCode ,				--排序编码
                                                'Department' AS Sort	--分类
                                      FROM      Base_Department			--部门表
                                      UNION
                                      SELECT    UserId AS Id,			--用户ID
                                                Code ,					--编码
                                                RealName ,				--名称
                                                DepartmentId AS ParentId ,--节点ID
                                                Gender,					--性别
                                                SortCode ,				--排序编码
                                                'User' AS Sort			--分类
                                      FROM      Base_User			    --用户表
          
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