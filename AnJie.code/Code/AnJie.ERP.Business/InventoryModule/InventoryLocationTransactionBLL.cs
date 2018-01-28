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
    public class InventoryLocationTransactionBLL : RepositoryFactory<InventoryLocationTransactionEntity>
    {
        /// <summary>
        /// 查询指定库位库存流水列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="warehouseId"></param>
        /// <param name="merchantId"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public List<InventoryLocationTransactionViewModel> GetInventoryLocationTransactionList(string keywords, string warehouseId,JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"
                SELECT  I.* ,
                        P.ProductName ,
                        P.Code AS ProductCode ,
                        W.WarehouseName
                FROM    dbo.Inventory_LocationTransaction I
                        INNER JOIN dbo.Warehouse W ON I.WarehouseId = W.WarehouseId
                        INNER JOIN dbo.Product P ON I.ProductId = P.ProductId
                WHERE   1 = 1");
            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND I.WarehouseId = @WarehouseId ");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }

            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.AppendFormat(@" AND (P.Code LIKE '{0}'
                                    OR P.ProductName LIKE '{0}'
                                    OR P.BriefName LIKE '{0}'
                                    OR P.BarCode LIKE '{0}'
                                    OR I.LocationFrom LIKE '{0}'
                                    OR I.LocationTo LIKE '{0}')", '%' + keywords + '%');
            }

            return new Repository<InventoryLocationTransactionViewModel>().FindListPageBySql(strSql.ToString(), parameter.ToArray(),
                ref jqgridparam);
        }

        /// <summary>
        /// 增加流水
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool InsertTransaction(InventoryLocationTransactionEntity entity, DbTransaction isOpenTrans)
        {
            return Repository().Insert(entity, isOpenTrans) > 0;
        }
    }
}
