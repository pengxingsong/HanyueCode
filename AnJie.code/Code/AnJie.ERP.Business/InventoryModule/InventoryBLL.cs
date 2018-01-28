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
    /// <summary>
    /// Inventory
    /// </summary>
    public class InventoryBLL : RepositoryFactory<InventoryEntity>
    {
        /// <summary>
        /// 查询指定仓库商户的库存列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="warehouseId"></param>
        /// <param name="merchantId"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public List<InventoryViewModel> GetInventoryList(string keywords, string warehouseId, string merchantId,
            JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  I.* ,
                                    P.ProductName ,
                                    P.Code AS ProductCode ,
                                    W.WarehouseName ,
                                    M.FullName AS MerchantName
                            FROM    dbo.Inventory I
                                    INNER JOIN dbo.Warehouse W ON I.WarehouseId = W.WarehouseId
                                    INNER JOIN dbo.Product P ON I.ProductId = P.ProductId
                                    INNER JOIN dbo.Merchant M ON I.MerchantId = M.MerchantId
                            WHERE 1 = 1");
            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND I.WarehouseId = @WarehouseId ");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }
            if (!string.IsNullOrEmpty(merchantId))
            {
                strSql.Append(" AND I.MerchantId = @MerchantId ");
                parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.AppendFormat(@" AND (P.Code LIKE '{0}'
                                    OR P.ProductName LIKE '{0}'
                                    OR P.BriefName LIKE '{0}'
                                    OR P.BarCode LIKE '{0}')", '%' + keywords + '%');
            }

            return new Repository<InventoryViewModel>().FindListPageBySql(strSql.ToString(), parameter.ToArray(),
                ref jqgridparam);
        }

        /// <summary>
        /// 获取商品可用库存列表
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="productId"></param>
        /// <param name="onlyAvailable"></param>
        /// <returns></returns>
        public List<InventoryEntity> GetProductInventoryList(string warehouseId, string productId, bool onlyAvailable)
        {
            var strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM dbo.Inventory");
            strSql.Append(" WHERE WarehouseId = @WarehouseId");
            strSql.Append(" AND ProductId = @ProductId ");
            if (onlyAvailable)
            {
                strSql.Append(" AND QtyOnHand - QtyAllocated > 0 ");
            }
            parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            parameter.Add(DbFactory.CreateDbParameter("@ProductId", productId));

            return new Repository<InventoryEntity>().FindListBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 更新商户在库库存(收货、拣货移入)
        /// </summary>
        /// <param name="sourceNo"></param>
        /// <param name="warehouseId"></param>
        /// <param name="productId"></param>
        /// <param name="merchantId"></param>
        /// <param name="moveInQty"></param>
        /// <param name="isOpenTrans"></param>
        public bool UpdateInventoryByReceive(string sourceNo, InventoryTransactionType type, string warehouseId, string productId, string merchantId, int moveInQty,
            DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM dbo.Inventory WHERE WarehouseId = @WarehouseId");
            strSql.Append(" AND ProductId = @ProductId");
            strSql.Append(" AND MerchantId = @MerchantId");

            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@WarehouseId", warehouseId),
                DbFactory.CreateDbParameter("@ProductId", productId),
                DbFactory.CreateDbParameter("@MerchantId", merchantId)
            };

            InventoryTransactionEntity transaction = new InventoryTransactionEntity();
            transaction.Create();
            transaction.Type = (int)type;
            transaction.WarehouseId = warehouseId;
            transaction.ProductId = productId;
            transaction.MerchantFrom = merchantId;
            transaction.MerchantTo = merchantId;
            transaction.Qty = moveInQty;
            transaction.SourceNo = sourceNo;

            var isOK = new InventoryTransactionBLL().InsertTransaction(transaction, isOpenTrans);
            if (isOK)
            {
                var inventory = Repository().FindEntityBySql(strSql.ToString(), parameter.ToArray());
                if (string.IsNullOrEmpty(inventory?.InventoryId))
                {
                    inventory = new InventoryEntity();
                    inventory.Create();
                    inventory.WarehouseId = warehouseId;
                    inventory.ProductId = productId;
                    inventory.QtyOnHand = moveInQty;
                    inventory.MerchantId = merchantId;
                    return Repository().Insert(inventory, isOpenTrans) > 0;
                }
                else
                {
                    StringBuilder strUpdateSql = new StringBuilder();
                    strUpdateSql.Append(
                        @"UPDATE [Inventory] SET QtyOnHand = QtyOnHand + @QtyOnHand WHERE InventoryId = @InventoryId ");

                    List<DbParameter> updateParameters = new List<DbParameter>
                {
                    DbFactory.CreateDbParameter("@QtyOnHand", moveInQty),
                    DbFactory.CreateDbParameter("@InventoryId", inventory.InventoryId)
                };
                    return Repository().ExecuteBySql(strUpdateSql, updateParameters.ToArray(), isOpenTrans) > 0;
                }
            }
            else
            {
                return false;
            }

            
        }

        /// <summary>
        /// 更新储位占用库存(配货)
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="productId"></param>
        /// <param name="merchantId"></param>
        /// <param name="qtyAllocated"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateInventoryByAllocate(string warehouseId, string productId, string merchantId, int qtyAllocated,
            DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Inventory] SET QtyAllocated = QtyAllocated + @QtyAllocated ");
            strSql.Append(" WHERE ProductId = @ProductId");
            strSql.Append(" AND WarehouseId = @WarehouseId");
            strSql.Append(" AND MerchantId = @MerchantId");
            strSql.Append(" AND QtyOnHand - QtyAllocated - @QtyAllocated >= 0");

            var updateParameters = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@WarehouseId", warehouseId),
                DbFactory.CreateDbParameter("@ProductId", productId),
                DbFactory.CreateDbParameter("@MerchantId", merchantId),
                DbFactory.CreateDbParameter("@QtyAllocated", qtyAllocated)
            };
            return Repository().ExecuteBySql(strSql, updateParameters.ToArray(), isOpenTrans) > 0;
        }

        /// <summary>
        /// 收货取消
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="productId"></param>
        /// <param name="merchantId"></param>
        /// <param name="qtyReceipt"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateInventoryByUnReceive(string sourceNo, InventoryTransactionType type, string warehouseId, string productId, string merchantId, int qtyReceipt,
            DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                @"UPDATE [Inventory] SET QtyOnHand = QtyOnHand - @QtyReceipt, QtyAllocated = QtyAllocated - @QtyReceipt");
            strSql.Append(" WHERE ProductId = @ProductId");
            strSql.Append(" AND WarehouseId = @WarehouseId");
            strSql.Append(" AND MerchantId = @MerchantId ");
            strSql.Append(" AND QtyOnHand - @QtyReceipt >= 0");

            var updateParameters = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@WarehouseId", warehouseId),
                DbFactory.CreateDbParameter("@ProductId", productId),
                DbFactory.CreateDbParameter("@MerchantId", merchantId),
                DbFactory.CreateDbParameter("@QtyReceipt", qtyReceipt)
            };

            InventoryTransactionEntity transaction = new InventoryTransactionEntity();
            transaction.Create();
            transaction.Type = (int)type;
            transaction.WarehouseId = warehouseId;
            transaction.ProductId = productId;
            transaction.MerchantFrom = merchantId;
            transaction.MerchantTo = merchantId;
            transaction.Qty = -1 * qtyReceipt;
            transaction.SourceNo = sourceNo;

            var isOK = new InventoryTransactionBLL().InsertTransaction(transaction, isOpenTrans);
            if (isOK)
            {
                return Repository().ExecuteBySql(strSql, updateParameters.ToArray(), isOpenTrans) > 0;
            }
            else
            {
                return false;   
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="productId"></param>
        /// <param name="merchantId"></param>
        /// <param name="qtyOutStock"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateInventoryByOutStock(string sourceNo, InventoryTransactionType type, string warehouseId, string merchantId, string productId, int qtyOutStock, DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Inventory] SET QtyOnHand = QtyOnHand - @QtyOutStock, QtyAllocated = QtyAllocated - @QtyOutStock ");
            strSql.Append(" WHERE ProductId = @ProductId");
            strSql.Append(" AND WarehouseId = @WarehouseId");
            strSql.Append(" AND MerchantId = @MerchantId ");
            strSql.Append(" AND QtyOnHand - @QtyOutStock >= 0");

            var updateParameters = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@WarehouseId", warehouseId),
                DbFactory.CreateDbParameter("@ProductId", productId),
                DbFactory.CreateDbParameter("@MerchantId", merchantId),
                DbFactory.CreateDbParameter("@QtyOutStock", qtyOutStock)
            };
            InventoryTransactionEntity inventoryTransaction = new InventoryTransactionEntity();
            inventoryTransaction.Create();
            inventoryTransaction.Type = (int)type;
            inventoryTransaction.WarehouseId = warehouseId;
            inventoryTransaction.ProductId = productId;
            inventoryTransaction.MerchantFrom = merchantId;
            inventoryTransaction.MerchantTo = "";
            inventoryTransaction.Qty = -1 * qtyOutStock;
            inventoryTransaction.SourceNo = sourceNo;
            var isOK = new InventoryTransactionBLL().InsertTransaction(inventoryTransaction, isOpenTrans);
            if (isOK)
            {
                return Repository().ExecuteBySql(strSql, updateParameters.ToArray(), isOpenTrans) > 0;
            }
            else
            {
                return false;
            }
        }
    }
}