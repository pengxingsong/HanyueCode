using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System;

namespace AnJie.ERP.Business
{
    public class ProductMerchantIdBLL : RepositoryFactory<ProductMerchantIdEntity>
    {

        public List<ProductMerchantIdEntity> GetProductMerchantIdList(int topNum, string merchantId, string keyword)
        {
            List<DbParameter> parameter = new List<DbParameter>();

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"
                SELECT  TOP {0}
                        P.* ,
                        PM.MerchantId
                FROM    dbo.Product AS P
                        JOIN dbo.Product_Merchant AS PM ON PM.ProductId = P.ProductId
                WHERE   1=1 ", topNum);

            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@" AND (P.Code LIKE @keyword
                                    OR P.ProductName LIKE @keyword
                                    OR P.BriefName LIKE @keyword
                                    OR P.BarCode LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            if (!string.IsNullOrWhiteSpace(merchantId))
            {
                strSql.Append(" AND PM.MerchantId = @MerchantId");
                parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            }
            strSql.Append(" ORDER BY P.SortCode ASC");
            return Repository().FindListBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}
