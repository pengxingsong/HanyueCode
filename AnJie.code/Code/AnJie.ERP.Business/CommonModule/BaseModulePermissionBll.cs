using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using AnJie.ERP.Cache;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// ģ��Ȩ�ޱ�
    /// </summary>
    public class BaseModulePermissionBll : RepositoryFactory<Base_ModulePermission>
    {
        private static BaseModulePermissionBll _item;
        public static BaseModulePermissionBll Instance
        {
            get
            {
                if (_item == null)
                {
                    _item = new BaseModulePermissionBll();
                }
                return _item;
            }
        }

        /// <summary>
        /// ģ��Ȩ���б�
        /// </summary>
        /// <param name="objectId">��������</param>
        /// <param name="category">�������:1-����2-��ɫ</param>
        /// <returns></returns>
        public DataTable GetList(string objectId, string category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(@"SELECT  m.ModuleId ,			
                                        m.ParentId ,			
                                        m.Code ,				
                                        m.FullName ,			
                                        m.Icon ,				
                                        m.SortCode ,			
                                        cp.ModuleId AS ObjectId  
                                FROM    Base_Module M INNER JOIN ( SELECT DISTINCT ModuleId FROM Base_ModulePermission");
                strSql.Append(" WHERE  ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "')) MP ON M.ModuleId = mp.ModuleId");
                strSql.Append(" LEFT JOIN ( SELECT DISTINCT ModuleId  FROM  Base_ModulePermission");
                strSql.Append(" WHERE  ObjectId = @ObjectId ) CP ON cp.ModuleId = M.ModuleId");
            }
            else
            {
                strSql.Append(@"SELECT  m.ModuleId, m.ParentId, m.Code, m.FullName, m.Icon, m.SortCode,
                                        mp.ObjectId
                                FROM    Base_Module m
                                        LEFT JOIN Base_ModulePermission mp ON mp.ModuleId = m.ModuleId
                                                                              AND mp.ObjectId = @ObjectId");
            }
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", objectId));
            strSql.Append(" ORDER BY  m.SortCode ASC ");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// ����Ȩ��ģ��
        /// </summary>
        /// <param name="objectId">��������</param>
        /// <returns></returns>
        public List<Base_Module> GetModuleList(string objectId)
        {
            StringBuilder strSql = new StringBuilder();
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(@"SELECT DISTINCT M.* FROM Base_Module M");
                strSql.AppendFormat(" INNER JOIN Base_ModulePermission MP ON M.ModuleId = MP.ModuleId WHERE MP.ObjectId = '{0}'",
                    ManageProvider.Provider.Current().UserId);
                strSql.Append(@" OR ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "')");
            }
            else
            {
                strSql.Append(@"SELECT * FROM Base_Module M");
            }
            strSql.Append(" ORDER BY  M.SortCode ASC ");
            return DataFactory.Database().FindListBySql<Base_Module>(strSql.ToString());
        }

        /// <summary>
        /// ���ݶ���Id��ȡģ��Ȩ���б�
        /// </summary>
        /// <param name="objectId">����ID</param>
        /// <returns></returns>
        public DataTable GetModulePermission(string objectId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  m.ModuleId ,
                                    m.ParentId ,
                                    m.FullName ,
                                    m.Icon
                            FROM    Base_Module m
                                    LEFT JOIN Base_ModulePermission mp ON mp.ModuleId = m.ModuleId");
            strSql.Append(" WHERE   mp.ObjectId IN ('" + objectId.Replace(",", "','") + "')");
            strSql.Append(" ORDER BY  m.SortCode ASC ");
            return Repository().FindTableBySql(strSql.ToString());
        }

        /// <summary>
        /// Actionִ��Ȩ����֤
        /// </summary>
        /// <param name="action">��ͼAction</param>
        /// <param name="objectId">��������</param>
        /// <param name="moduleId">ģ��Id</param>
        /// <param name="userId">�û�Id</param>
        /// <returns></returns>
        public bool ActionAuthorize(string action, string objectId, string moduleId, string userId)
        {
            List<Base_Module> listData;
            object actionAuthorizeList = DataCache.Get("ActionAuthorizeList_" + userId);
            if (actionAuthorizeList == null)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT  b.ModuleId ,
                                        b.FullName ,
                                        ISNULL(b.ActionEvent, '') AS Location
                                FROM    Base_Button b
                                        INNER JOIN Base_ButtonPermission bp ON bp.ModuleButtonId = b.ButtonId
                                AND (bp.ObjectId = '" + userId + "' OR bp.ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "'))");
                strSql.Append(" UNION ");
                strSql.Append(@"SELECT  m.ModuleId ,
                                        m.FullName ,
                                        ISNULL(m.Location, '') AS Location
                                FROM    Base_Module m
                                        INNER JOIN Base_ModulePermission mp ON mp.ModuleId = m.ModuleId  
                                AND (mp.ObjectId = '" + userId + "' OR mp.ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "'))");
                listData = DataFactory.Database().FindListBySql<Base_Module>(strSql.ToString());
                DataCache.Insert("ActionAuthorizeList_" + userId, listData);
            }
            else
            {
                listData = (List<Base_Module>)actionAuthorizeList;
            }
            listData = (from entity in listData
                        where (entity.Location.ToLower() == action && entity.ModuleId == moduleId)
                        select entity).ToList();
            int count = listData.Count;
            if (count > 0)
            {
                return true;
            }
            return false;
        }
    }
}