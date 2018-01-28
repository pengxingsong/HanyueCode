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
    /// ��ͼ����Ȩ�ޱ�
    /// </summary>
    public class BaseViewPermissionBll : RepositoryFactory<Base_ViewPermission>
    {
        /// <summary>
        /// ��ͼȨ���б�
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
                strSql.Append(
                    @"SELECT  v.*,vp.ModuleId AS ObjectId FROM  Base_View v
                                INNER JOIN Base_ViewPermission P ON v.ModuleId = P.ModuleId AND v.ViewId = P.ViewId AND p.ObjectId IN ( '" +
                    ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "' )");
                strSql.Append(" LEFT JOIN ( SELECT DISTINCT *  FROM    Base_ViewPermission");
                strSql.Append(" WHERE ObjectId = @ObjectId) vp ON v.ModuleId = vP.ModuleId  AND v.ViewId = vP.ViewId");
            }
            else
            {
                strSql.Append(@"SELECT  v.ViewId ,					--ID
                                        v.ModuleId ,				--ģ��ID
                                        v.ShowName ,				--����
                                        v.SortCode ,				--������
                                        vp.ObjectId					--�Ƿ����
                                FROM    Base_View v
                                        LEFT JOIN Base_ViewPermission vp ON vp.ViewId = v.ViewId
                                                                            AND vp.ObjectId = @ObjectId");
            }
            strSql.Append(" order by v.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", objectId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// ���ر����ͼȨ��
        /// </summary>
        /// <param name="objectId">��������</param>
        /// <param name="moduleId">ģ������</param>
        /// <returns></returns>
        public List<Base_View> GetViewList(string objectId, string moduleId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            if (!ManageProvider.Provider.Current().IsSystem)
            {

                strSql.Append(@"SELECT  FieldName,Enabled
                                FROM    Base_View v
                                WHERE   v.ModuleId = @ModuleId AND FieldName NOT IN (
                                SELECT  FieldName FROM    Base_View v INNER JOIN Base_ViewPermission P ON v.ModuleId = P.ModuleId AND v.ViewId = P.ViewId");
                strSql.Append(" WHERE   p.ObjectId IN ('" +
                              ManageProvider.Provider.Current().ObjectId.Replace(",", "','") +
                              "') AND v.ModuleId = @ModuleId)");
            }
            else
            {
                strSql.Append(@"SELECT FieldName,Enabled FROM Base_View v WHERE 1=0 ");
                strSql.Append(" AND v.ModuleId = @ModuleId");
            }
            strSql.Append(" ORDER BY v.SortCode ASC ");
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", moduleId));
            return DataFactory.Database().FindListBySql<Base_View>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// ���ݶ���Id��ȡģ����ͼȨ���б�
        /// </summary>
        /// <param name="objectId">����ID</param>
        /// <returns></returns>
        public DataTable GetViewPermission(string objectId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    m.ModuleId AS ID ,
                                                m.ParentId ,
                                                m.FullName ,
                                                m.Icon ,
                                                m.SortCode
                                      FROM      Base_Module m
                                                LEFT JOIN Base_ModulePermission mp ON mp.ModuleId = m.ModuleId");
            strSql.Append(@" WHERE     mp.ObjectId IN ('" + objectId.Replace(",", "','") + "')");
            strSql.Append(@" UNION
                                      SELECT    v.ViewId AS ID ,
                                                v.ModuleId AS ParentId ,
                                                v.FullName ,
                                                '' AS Icon ,
                                                v.SortCode
                                      FROM      Base_View v
                                                LEFT JOIN Base_ViewPermission vp ON vp.ViewId = v.ViewId
                                      WHERE     vp.ObjectId IN ('" + objectId.Replace(",", "','") + "') ) A");
            strSql.Append(" ORDER BY SortCode ASC ");
            return Repository().FindTableBySql(strSql.ToString());
        }
    }
}