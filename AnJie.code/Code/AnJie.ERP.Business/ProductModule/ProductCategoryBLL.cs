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
    /// 商品分类管理
    /// </summary>
    public class ProductCategoryBLL : RepositoryFactory<ProductCategoryEntity>
    {
        /// <summary>
        /// 获取 商户、分类 列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  CategoryId ,
                                    Code ,
                                    CategoryName ,
                                    ParentId ,
                                    SortCode
                            FROM    Product_Category");

            strSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString());
        }

        /// <summary>
        /// 根据商户id获取分类列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT * FROM Product_Category ");

            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(" AND ( MerchantId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    strSql.Append(" ) )");
            //}
            strSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString());
        }
    }
}