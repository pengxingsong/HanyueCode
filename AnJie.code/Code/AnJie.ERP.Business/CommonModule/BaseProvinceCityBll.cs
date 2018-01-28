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
    /// ʡ����
    /// </summary>
    public class BaseProvinceCityBll : RepositoryFactory<Base_ProvinceCity>
    {
        /// <summary>
        /// ��ȡʡ���С��� �б�
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<Base_ProvinceCity> GetList(string parentId)
        {
            StringBuilder whereSql = new StringBuilder();
            whereSql.Append(" AND ParentId = @ParentId Order By SortCode ASC");
            List<DbParameter> parameter = new List<DbParameter> { DbFactory.CreateDbParameter("@ParentId", parentId) };
            return this.Repository().FindList(whereSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// ���ݴ����ȡ����
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetNameByCode(string code)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  FullName
                                FROM    dbo.Base_ProvinceCity
                                WHERE   Code = @Code");
            parameter.Add(DbFactory.CreateDbParameter("@Code", code));
            var ent= DataFactory.Database().FindEntityBySql<Base_ProvinceCity>(strSql.ToString(), parameter.ToArray());

            return ent == null ? "" : ent.FullName;
        }
    }
}