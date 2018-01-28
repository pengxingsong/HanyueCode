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
    /// ������ťȨ�ޱ�
    /// </summary>
    public class BaseButtonPermissionBll : RepositoryFactory<Base_ButtonPermission>
    {
        /// <summary>
        /// ��ťȨ���б�
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
                strSql.Append(@"SELECT  b.ButtonId ,				
                                        b.ModuleId ,				
                                        b.Code ,					
                                        b.FullName ,				
                                        b.Category ,				
                                        b.Icon ,					
                                        b.SortCode ,				   
                                        cp.ModuleButtonId AS ObjectId
                                FROM    Base_Button b INNER JOIN ( SELECT DISTINCT ModuleButtonId FROM Base_ButtonPermission");
                strSql.Append(" WHERE  ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") +
                              "')) bp ON B.ButtonId = bp.ModuleButtonId");
                strSql.Append(" LEFT JOIN ( SELECT DISTINCT ModuleButtonId  FROM  Base_ButtonPermission");
                strSql.Append(" WHERE  ObjectId = @ObjectId ) cp ON cp.ModuleButtonId = b.ButtonId");
            }
            else
            {
                strSql.Append(@"SELECT  b.ButtonId ,	
                                    b.ModuleId ,		
                                    b.Code ,			
                                    b.FullName ,		
                                    b.Category ,		
                                    b.Icon ,			
                                    b.SortCode ,		
                                    bp.ObjectId			                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
                            FROM    Base_Button b
                                    LEFT JOIN Base_ButtonPermission bp ON bp.ModuleButtonId = b.ButtonId
                                                                          AND bp.ObjectId = @ObjectId");
            }
            strSql.Append(" order by b.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", objectId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// ���ذ�ťȨ��
        /// </summary>
        /// <param name="objectId">��������</param>
        /// <param name="moduleId">ģ������</param>
        /// <returns></returns>
        public List<Base_Button> GetButtonList(string objectId, string moduleId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(@"SELECT DISTINCT B.* FROM Base_Button B");
                strSql.Append(" INNER JOIN Base_ButtonPermission BP ON B.ButtonId = BP.ModuleButtonId");
                strSql.AppendFormat(" WHERE (BP.ObjectId = '{0}'", ManageProvider.Provider.Current().UserId);
                strSql.Append(" OR ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") +
                              "'))");
            }
            else
            {
                strSql.Append(@"SELECT * FROM Base_Button B WHERE 1=1 ");
            }
            strSql.Append(" AND B.ModuleId = @ModuleId");
            strSql.Append(" ORDER BY B.SortCode ASC ");
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", moduleId));
            return DataFactory.Database().FindListBySql<Base_Button>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// ���ݶ���Id��ȡģ�鰴ťȨ���б�
        /// </summary>
        /// <param name="objectId">����ID</param>
        /// <returns></returns>
        public DataTable GetButtonePermission(string objectId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    m.ModuleId AS ID ,
                                                m.ParentId ,
                                                m.FullName ,
                                                m.Icon ,
                                                m.SortCode,
                                                m.Category,
                                                'ģ��' AS Sort
                                      FROM      Base_Module m
                                                LEFT JOIN Base_ModulePermission mp ON mp.ModuleId = m.ModuleId");
            strSql.Append(@" WHERE     mp.ObjectId IN ('" + objectId.Replace(",", "','") + "')");
            strSql.Append(@" UNION     SELECT    b.ButtonId AS ID ,
                                                b.ModuleId AS ParentId ,
                                                b.FullName ,
                                                b.Icon ,
                                                b.SortCode,
                                                b.Category,
                                                '��ť' AS Sort
                                      FROM      Base_Button b
                                                LEFT JOIN Base_ButtonPermission bp ON bp.ModuleButtonId = b.ButtonId");
            strSql.Append(" WHERE     bp.ObjectId IN ('" + objectId.Replace(",", "','") + "')) A");
            strSql.Append(" ORDER BY SortCode ASC ");
            return Repository().FindTableBySql(strSql.ToString());
        }
    }
}