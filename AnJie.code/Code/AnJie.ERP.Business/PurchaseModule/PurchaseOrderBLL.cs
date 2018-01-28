using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using AnJie.ERP.ViewModel.PurchaseModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 采购单主表
    /// </summary>
    public class PurchaseOrderBLL : RepositoryFactory<PurchaseOrderEntity>
    {
        private readonly BaseCodeRuleBll _codeRuleBLL = new BaseCodeRuleBll();

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="orderNo">订单号</param>
        /// <param name="startTime">订单开始时间</param>
        /// <param name="endTime">订单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public List<PurchaseOrderViewModel> GetOrderList(string warehouseId, string merchantId, string orderNo, string startTime, string endTime, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT PO.*, W.WarehouseName,
                                M.FullName AS MerchantName
                            FROM [PurchaseOrder] PO
                            INNER JOIN dbo.Warehouse W ON PO.WarehouseId = W.WarehouseId
                            INNER JOIN dbo.Merchant M ON PO.MerchantId = M.MerchantId WHERE 1=1");
            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND PO.WarehouseId = @WarehouseId ");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }
            if (!string.IsNullOrEmpty(merchantId))
            {
                strSql.Append(" AND PO.MerchantId = @MerchantId ");
                parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            }
            if (!string.IsNullOrEmpty(orderNo))
            {
                strSql.Append(" AND PO.OrderNo = @OrderNo ");
                parameter.Add(DbFactory.CreateDbParameter("@OrderNo", orderNo));
            }

            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                strSql.Append(" AND PO.OrderDate Between @StartTime AND @EndTime ");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", CommonHelper.GetDateTime(startTime + " 00:00")));
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", CommonHelper.GetDateTime(endTime + " 23:59")));
            }
            return new Repository<PurchaseOrderViewModel>().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        /// <summary>
        /// 订单明细列表
        /// </summary>
        /// <param name="orderId">订单主键</param>
        /// <returns></returns>
        public List<PurchaseOrderItemEntity> GetOrderItemList(string orderId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM PurchaseOrder_Item WHERE 1=1");
            strSql.Append(" AND OrderId = @OrderId");
            parameter.Add(DbFactory.CreateDbParameter("@OrderId", orderId));
            return DataFactory.Database().FindListBySql<PurchaseOrderItemEntity>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 审核订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int Audit(PurchaseOrderEntity order)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                PurchaseOrderEntity purchaseOrder = DataFactory.Database().FindEntity<PurchaseOrderEntity>(order.OrderId);
                if (purchaseOrder == null)
                {
                    throw new Exception("采购单不存在");
                }

                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"UPDATE dbo.PurchaseOrder
                            SET Status = 1, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE OrderId = @OrderId
                                AND Status = 0");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@ModifyUserId", order.ModifyUserId));
                parameter.Add(DbFactory.CreateDbParameter("@ModifyUserName", order.ModifyUserName));
                parameter.Add(DbFactory.CreateDbParameter("@ModifyDate", order.ModifyDate));
                parameter.Add(DbFactory.CreateDbParameter("@OrderId", order.OrderId));
                int result = database.ExecuteBySql(strSql, parameter.ToArray(), isOpenTrans);
                if (result > 0)
                {
                    string userId = ManageProvider.Provider.Current().UserId;

                    ReceiptEntity entity = new ReceiptEntity();
                    entity.ReceiptId = CommonHelper.GetGuid;
                    entity.ReceiptNo = _codeRuleBLL.GetBillCode(userId, "Receipt");
                    entity.ReceiptDate = purchaseOrder.OrderDate;
                    entity.ReceiptType = 1;
                    entity.WarehouseId = purchaseOrder.WarehouseId;
                    entity.MerchantId = purchaseOrder.MerchantId;
                    entity.SourceNo = purchaseOrder.OrderNo;
                    entity.Status = 0;
                    entity.Create();

                    database.Insert(entity, isOpenTrans);

                    List<PurchaseOrderItemEntity> orderItemList = GetOrderItemList(order.OrderId);

                    foreach (PurchaseOrderItemEntity item in orderItemList)
                    {
                        var orderItem = new ReceiptItemEntity();
                        orderItem.Create();
                        orderItem.ReceiptId = entity.ReceiptId;
                        orderItem.ProductId = item.ProductId;
                        orderItem.Code = item.Code;
                        orderItem.ProductName = item.ProductName;
                        orderItem.Qty = item.Qty;
                        database.Insert(orderItem, isOpenTrans);
                    }
                }
                database.Commit();
                return result;
            }
            catch
            {
                database.Rollback();
                return -1;
            }
        }

        public int Invalid(PurchaseOrderEntity order)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                PurchaseOrderEntity purchaseOrder = DataFactory.Database().FindEntity<PurchaseOrderEntity>(order.OrderId);
                if (purchaseOrder == null)
                {
                    throw new Exception("采购单不存在");
                }

                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"UPDATE dbo.PurchaseOrder
                            SET Status = -1, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE OrderId = @OrderId
                                AND Status = 0");
                List<DbParameter> parameter = new List<DbParameter>
                {
                    DbFactory.CreateDbParameter("@ModifyUserId", order.ModifyUserId),
                    DbFactory.CreateDbParameter("@ModifyUserName", order.ModifyUserName),
                    DbFactory.CreateDbParameter("@ModifyDate", order.ModifyDate),
                    DbFactory.CreateDbParameter("@OrderId", order.OrderId)
                };
                int result = database.ExecuteBySql(strSql, parameter.ToArray(), isOpenTrans);
                
                database.Commit();
                return result;
            }
            catch
            {
                database.Rollback();
                return -1;
            }
        }
    }
}