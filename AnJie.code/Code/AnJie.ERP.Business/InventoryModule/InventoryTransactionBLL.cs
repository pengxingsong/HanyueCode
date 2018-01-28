using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AnJie.ERP.ViewModel.InventoryModule;
using System;
using System.Data;
using System.Data.Common;
using AnJie.ERP.DataAccess;

namespace AnJie.ERP.Business
{
    public class InventoryTransactionBLL : RepositoryFactory<InventoryTransactionEntity>
    {
        /// <summary>
        /// 查询指定仓库商户的库存流水列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="warehouseId"></param>
        /// <param name="merchantId"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public List<InventoryTransactionViewModel> GetInventoryTransactionList(string keywords, string warehouseId, string merchantId,
            JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"
                SELECT  I.* ,
                        P.ProductName ,
                        P.Code AS ProductCode ,
                        W.WarehouseName ,
                        MF.FullName AS MerchantFromName,
                        MT.FullName AS MerchantTotName
                FROM    dbo.Inventory_Transaction I
                        INNER JOIN dbo.Warehouse W ON I.WarehouseId = W.WarehouseId
                        INNER JOIN dbo.Product P ON I.ProductId = P.ProductId
                        INNER JOIN dbo.Merchant MF ON I.MerchantFrom = MF.MerchantId
                        LEFT JOIN dbo.Merchant MT ON I.MerchantTo = MT.MerchantId
                WHERE 1 = 1");

            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND I.WarehouseId = @WarehouseId ");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }
            if (!string.IsNullOrEmpty(merchantId))
            {
                strSql.Append(" AND I.MerchantFrom = @MerchantId ");
                parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.AppendFormat(@" AND (P.Code LIKE '{0}'
                                    OR P.ProductName LIKE '{0}'
                                    OR P.BriefName LIKE '{0}'
                                    OR P.BarCode LIKE '{0}'
                                    OR I.MerchantTo LIKE '{0}'
                                    OR I.SourceNo LIKE '{0}')", '%' + keywords + '%');
            }

            return new Repository<InventoryTransactionViewModel>().FindListPageBySql(strSql.ToString(), parameter.ToArray(),
                ref jqgridparam);
        }

        /// <summary>
        /// 增加流水
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool InsertTransaction(InventoryTransactionEntity entity, DbTransaction isOpenTrans)
        {
            return Repository().Insert(entity, isOpenTrans) > 0;
        }
    }
}
