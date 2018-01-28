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
    /// Inventory_Location
    /// </summary>
    public class InventoryLocationBLL : RepositoryFactory<InventoryLocationEntity>
    {
        /// <summary>
        /// 查询指定仓库商户的库存列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="warehouseId"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public List<InventoryLocationViewModel> GetInventoryList(string keywords, string warehouseId, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  I.* ,
                                    P.ProductName ,
                                    P.Code AS ProductCode ,
                                    W.WarehouseName 
                            FROM    dbo.Inventory_Location I
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
                                    OR P.BarCode LIKE '{0}')", '%' + keywords + '%');
            }

            return new Repository<InventoryLocationViewModel>().FindListPageBySql(strSql.ToString(), parameter.ToArray(),
                ref jqgridparam);
        }

        /// <summary>
        /// 获取商品可用库存列表
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="productId"></param>
        /// <param name="onlyAvailable"></param>
        /// <returns></returns>
        public List<InventoryLocationEntity> GetProductInventoryList(string warehouseId, string productId, bool onlyAvailable)
        {
            var strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  inventory.InventoryId ,
                                    inventory.WarehouseId ,
                                    inventory.ProductId ,
                                    inventory.LocationCode ,
                                    inventory.QtyOnHand ,
                                    inventory.QtyAllocated ,
                                    inventory.QtyMoveIn ,
                                    inventory.QtySuspense ,
                                    inventory.CreateDate ,
                                    inventory.CreateUserId ,
                                    inventory.CreateUserName ,
                                    inventory.ModifyDate ,
                                    inventory.ModifyUserId ,
                                    inventory.ModifyUserName
                            FROM    dbo.Inventory_Location inventory
                                    INNER JOIN dbo.Warehouse_Location location ON location.WarehouseId = inventory.WarehouseId
                                    AND location.Code = inventory.LocationCode");
            strSql.Append(" WHERE inventory.WarehouseId = @WarehouseId");
            strSql.Append(" AND inventory.ProductId = @ProductId ");
            if (onlyAvailable)
            {
                strSql.Append(" AND QtyOnHand - QtyAllocated - QtySuspense > 0 ");
            }
            parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            parameter.Add(DbFactory.CreateDbParameter("@ProductId", productId));

            return new Repository<InventoryLocationEntity>().FindListBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 更新储位在库库存(收货、拣货移入)
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="productId"></param>
        /// <param name="locationFrom"></param>
        /// <param name="locationTo"></param>
        /// <param name="moveInQty"></param>
        /// <param name="isOpenTrans"></param>
        public bool UpdateInventoryByMoveIn(string warehouseId, InventoryLocationTransactionType type, string productId, string locationFrom, string locationTo, int moveInQty, DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM dbo.Inventory_Location WHERE WarehouseId = @WarehouseId");
            strSql.Append(" AND ProductId = @ProductId");
            strSql.Append(" AND LocationCode = @LocationCode");

            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@WarehouseId", warehouseId),
                DbFactory.CreateDbParameter("@ProductId", productId),
                DbFactory.CreateDbParameter("@LocationCode", locationTo)
            };

            InventoryLocationTransactionEntity locationTransaction = new InventoryLocationTransactionEntity();
            locationTransaction.Create();
            locationTransaction.Type = (int)type;
            locationTransaction.WarehouseId = warehouseId;
            locationTransaction.ProductId = productId;
            locationTransaction.LocationFrom = locationFrom;
            locationTransaction.LocationTo = locationTo;
            locationTransaction.Qty = moveInQty;

            var isOK = new InventoryLocationTransactionBLL().InsertTransaction(locationTransaction, isOpenTrans);
            if (isOK)
            {
                var inventory = Repository().FindEntityBySql(strSql.ToString(), parameter.ToArray());
                if (string.IsNullOrEmpty(inventory?.InventoryId))
                {
                    inventory = new InventoryLocationEntity();
                    inventory.Create();
                    inventory.WarehouseId = warehouseId;
                    inventory.ProductId = productId;
                    inventory.LocationCode = locationTo;
                    inventory.QtyOnHand = moveInQty;
                    return Repository().Insert(inventory, isOpenTrans) > 0;
                }
                else
                {
                    StringBuilder strUpdateSql = new StringBuilder();
                    strUpdateSql.Append(@"UPDATE [Inventory_Location] SET QtyOnHand = QtyOnHand + @QtyOnHand WHERE InventoryId = @InventoryId ");

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
        /// <param name="locationCode"></param>
        /// <param name="qtyAllocated"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateInventoryByAllocate(string warehouseId, string productId, string locationCode, int qtyAllocated, DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Inventory_Location] SET QtyAllocated = QtyAllocated + @QtyAllocated ");
            strSql.Append(" WHERE ProductId = @ProductId");
            strSql.Append(" AND WarehouseId = @WarehouseId");
            strSql.Append(" AND LocationCode = @LocationCode ");
            strSql.Append(" AND QtyOnHand - QtyAllocated - QtySuspense - @QtyAllocated >= 0");

            var updateParameters = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@WarehouseId", warehouseId),
                DbFactory.CreateDbParameter("@ProductId", productId),
                DbFactory.CreateDbParameter("@LocationCode", locationCode),
                DbFactory.CreateDbParameter("@QtyAllocated", qtyAllocated)
            };
            return Repository().ExecuteBySql(strSql, updateParameters.ToArray(), isOpenTrans) > 0;
        }

        /// <summary>
        /// 更新储位占用库存(拣货完成)
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="productId"></param>
        /// <param name="locationFrom"></param>
        /// <param name="locationCode"></param>
        /// <param name="qtyPicked"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateInventoryByPicked(string warehouseId, InventoryLocationTransactionType type, string productId, string locationFrom, string locationCode, int qtyPicked, DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Inventory_Location] SET QtyOnHand = QtyOnHand - @QtyPicked, QtyAllocated = QtyAllocated - @QtyPicked ");
            strSql.Append(" WHERE ProductId = @ProductId");
            strSql.Append(" AND WarehouseId = @WarehouseId");
            strSql.Append(" AND LocationCode = @LocationCode ");
            strSql.Append(" AND QtyOnHand - @QtyPicked >= 0 AND QtyAllocated - @QtyPicked >= 0");

            var updateParameters = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@WarehouseId", warehouseId),
                DbFactory.CreateDbParameter("@ProductId", productId),
                DbFactory.CreateDbParameter("@LocationCode", locationCode),
                DbFactory.CreateDbParameter("@QtyPicked", qtyPicked)
            };
            InventoryLocationTransactionEntity locationTransaction = new InventoryLocationTransactionEntity();
            locationTransaction.Create();
            locationTransaction.Type = (int)type;
            locationTransaction.WarehouseId = warehouseId;
            locationTransaction.ProductId = productId;
            locationTransaction.LocationFrom = locationFrom;
            locationTransaction.LocationTo = locationCode;
            locationTransaction.Qty = -1 * qtyPicked;
            var isOK = new InventoryLocationTransactionBLL().InsertTransaction(locationTransaction, isOpenTrans);
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
        /// <param name="locationCode"></param>
        /// <param name="qtyOutStock"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateInventoryByOutStock(string warehouseId, InventoryLocationTransactionType type, string productId, string locationCode, int qtyOutStock, DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Inventory_Location] SET QtyOnHand = QtyOnHand - @QtyOutStock, QtyAllocated = QtyAllocated - @QtyOutStock ");
            strSql.Append(" WHERE ProductId = @ProductId");
            strSql.Append(" AND WarehouseId = @WarehouseId");
            strSql.Append(" AND LocationCode = @LocationCode ");
            strSql.Append(" AND QtyOnHand - @QtyOutStock >= 0 AND QtyAllocated - @QtyOutStock >= 0");

            var updateParameters = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@WarehouseId", warehouseId),
                DbFactory.CreateDbParameter("@ProductId", productId),
                DbFactory.CreateDbParameter("@LocationCode", locationCode),
                DbFactory.CreateDbParameter("@QtyOutStock", qtyOutStock)
            };
            InventoryLocationTransactionEntity locationTransaction = new InventoryLocationTransactionEntity();
            locationTransaction.Create();
            locationTransaction.Type = (int)type;
            locationTransaction.WarehouseId = warehouseId;
            locationTransaction.ProductId = productId;
            locationTransaction.LocationFrom = locationCode;
            locationTransaction.LocationTo = "";
            locationTransaction.Qty = -1 * qtyOutStock;
            var isOK = new InventoryLocationTransactionBLL().InsertTransaction(locationTransaction, isOpenTrans);
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
        /// <param name="locationCode"></param>
        /// <param name="qtyReceipt"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateInventoryByUnReceive(string warehouseId, InventoryLocationTransactionType type, string productId, string locationCode, int qtyReceipt, DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Inventory_Location] SET QtyOnHand = QtyOnHand - @QtyReceipt ");
            strSql.Append(" WHERE ProductId = @ProductId");
            strSql.Append(" AND WarehouseId = @WarehouseId");
            strSql.Append(" AND LocationCode = @LocationCode ");
            strSql.Append(" AND QtyOnHand - @QtyReceipt >= 0");

            var updateParameters = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@WarehouseId", warehouseId),
                DbFactory.CreateDbParameter("@ProductId", productId),
                DbFactory.CreateDbParameter("@LocationCode", locationCode),
                DbFactory.CreateDbParameter("@QtyReceipt", qtyReceipt)
            };
            InventoryLocationTransactionEntity locationTransaction = new InventoryLocationTransactionEntity();
            locationTransaction.Create();
            locationTransaction.Type = (int)type;
            locationTransaction.WarehouseId = warehouseId;
            locationTransaction.ProductId = productId;
            locationTransaction.LocationFrom = locationCode;
            locationTransaction.LocationTo = "";
            locationTransaction.Qty = -1 * qtyReceipt;

            var isOK = new InventoryLocationTransactionBLL().InsertTransaction(locationTransaction, isOpenTrans);
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