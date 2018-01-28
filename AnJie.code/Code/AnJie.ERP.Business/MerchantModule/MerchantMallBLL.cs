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
    /// 商户店铺表
    /// </summary>
    public class MerchantMallBLL : RepositoryFactory<MerchantMallEntity>
    {
        /// <summary>
        /// 获取 商户、店铺 列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    MerchantId,				    --商户ID
												MerchantId AS MallId ,  --店铺ID
                                                Code ,					    --编码
                                                FullName AS MallName ,  --名称
                                                '' AS ParentId ,	        --节点ID
                                                SortCode,				    --排序编码
                                                'Merchant' AS Sort		    --分类
                                      FROM      Merchant		    	    --商户表
                                      UNION
                                      SELECT    MerchantId,				    --商户ID
												MallId,			        --店铺ID
                                                Code ,					    --编码
                                                MallName ,			    --名称
                                                MerchantId AS ParentId ,    --节点ID
                                                SortCode,				    --排序编码
                                                'Mall' AS Sort	        --分类
                                      FROM      Merchant_Mall		    --店铺表
                                    ) T WHERE 1=1 ");
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(" AND ( MerchantId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    strSql.Append(" ) )");
            //}
            strSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString());
        }

        /// <summary>
        /// 根据商户id获取分类列表
        /// </summary>
        /// <param name="merchantId">商户ID</param>
        /// <returns></returns>
        public DataTable GetList(string merchantId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    c.MallId ,			    --主键
                                                m.FullName AS MerchantName ,--所属商户
                                                c.MerchantId ,				--所属商户Id
                                                c.Code ,					--编码
                                                c.MallName ,    		--部门名称                                              
                                                c.Enabled ,					--有效
                                                c.SortCode,                 --排序码
                                                c.Remark					--说明
                                      FROM      Merchant_Mall c
                                                INNER JOIN Merchant m ON c.MerchantId = m.MerchantId
                                    ) T WHERE 1=1 ");
            List<DbParameter> parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(merchantId))
            {
                strSql.Append(" AND MerchantId = @MerchantId");
                parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            }
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(" AND ( MerchantId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    strSql.Append(" ) )");
            //}
            strSql.Append(" ORDER BY MerchantId ASC, SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }


        public DataTable GetListAll()
        {
            return Repository().FindTableByProc("Proc_S_MerchantMall");
        }


    }
}