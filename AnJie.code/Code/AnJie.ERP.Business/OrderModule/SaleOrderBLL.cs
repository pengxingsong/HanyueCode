using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using AnJie.ERP.ViewModel.OrderModule;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System;
using System.Data;
using System.Web;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 订单主表
    /// </summary>
    public class SaleOrderBLL : RepositoryFactory<SaleOrderEntity>
    {
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<SaleOrderViewModel> GetOrderList(QueryOrderViewModel queryModel, JqGridParam jqgridparam)
        {
            string userId = ManageProvider.Provider.Current().UserId;
            string userName = ManageProvider.Provider.Current().UserName;

            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  SO.* ,
                                    W.WarehouseName ,
                                    M.FullName AS MerchantName ,
                                    MM.MallName ,
                                    S.ShipTypeName ,
                                    S.Code AS ShipTypeCode,
                                    ProductDetail = STUFF(( SELECT  ',' + product.ProductName + '('
                                                            + CONVERT(VARCHAR(20), item.Qty) + ')'
                                                            + product.BaseUnit
                                                    FROM    dbo.SaleOrder_Item item
                                                            INNER JOIN dbo.Product product ON item.ProductId = product.ProductId
                                                    WHERE   item.OrderNo = SO.OrderNo
                                                  FOR
                                                    XML PATH('')
                                                  ), 1, 1, '')
                            FROM    SaleOrder SO
                                    LEFT JOIN dbo.Warehouse W ON SO.WarehouseId = W.WarehouseId
                                    LEFT JOIN dbo.Merchant M ON SO.MerchantId = M.MerchantId
                                    LEFT JOIN dbo.Merchant_Mall MM ON SO.MerchantMallId = MM.MallId
                                    LEFT JOIN dbo.ShipType S ON SO.ShipTypeId = S.ShipTypeId
                            WHERE   1 = 1");

            //查询非挂单锁单
            if (queryModel.QueryType != "Suspend" && queryModel.QueryType != "Lock1" && queryModel.QueryType != "Lock2")
            {
                if (!string.IsNullOrEmpty(queryModel.WarehouseId))
                {
                    strSql.Append(" AND SO.WarehouseId = @WarehouseId ");
                    parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", queryModel.WarehouseId));
                }
                if (!string.IsNullOrEmpty(queryModel.MerchantId))
                {
                    strSql.Append(" AND SO.MerchantId = @MerchantId ");
                    parameter.Add(DbFactory.CreateDbParameter("@MerchantId", queryModel.MerchantId));
                }
                if (!string.IsNullOrEmpty(queryModel.OrderNo))
                {
                    //strSql.Append(" AND SO.OrderNo = @OrderNo ");
                    //parameter.Add(DbFactory.CreateDbParameter("@OrderNo", queryModel.OrderNo));
                    strSql.AppendFormat(@" AND SO.OrderNo LIKE '{0}'", '%' + queryModel.OrderNo + '%');
                }
                if (!string.IsNullOrEmpty(queryModel.SourceOrderNo))
                {
                    //strSql.Append(" AND SO.SourceOrderNo = @SourceOrderNo ");
                    //parameter.Add(DbFactory.CreateDbParameter("@SourceOrderNo", queryModel.SourceOrderNo));

                    strSql.AppendFormat(@" AND SO.SourceOrderNo LIKE '{0}'", '%' + queryModel.SourceOrderNo + '%');
                }
                if (!string.IsNullOrEmpty(queryModel.ReceiveContact))
                {
                    queryModel.ReceiveContact = HttpUtility.UrlDecode(queryModel.ReceiveContact);
                    strSql.AppendFormat(@" AND SO.ReceiveContact LIKE '{0}'", '%' + queryModel.ReceiveContact + '%');
                }
                if (!string.IsNullOrEmpty(queryModel.ReceivePhone))
                {
                    strSql.AppendFormat(@" AND (SO.ReceivePhone LIKE '{0}' OR SO.ReceiveCellPhone LIKE '{0}')", '%' + queryModel.ReceivePhone + '%');
                }
                if (!string.IsNullOrEmpty(queryModel.ShipTypeId))
                {
                    strSql.Append(" AND SO.ShipTypeId = @ShipTypeId ");
                    parameter.Add(DbFactory.CreateDbParameter("@ShipTypeId", queryModel.ShipTypeId));
                }
                if (!string.IsNullOrEmpty(queryModel.ExpressNum))
                {
                    //strSql.Append(" AND SO.ExpressNum = @ExpressNum ");
                    //parameter.Add(DbFactory.CreateDbParameter("@ExpressNum", queryModel.ExpressNum));
                    strSql.AppendFormat(@" AND SO.ExpressNum LIKE '{0}'", '%' + queryModel.ExpressNum + '%');
                }
                if (!string.IsNullOrEmpty(queryModel.StartTime))
                {
                    strSql.Append(" AND SO.OrderDate >= @StartTime ");
                    parameter.Add(DbFactory.CreateDbParameter("@StartTime",
                        CommonHelper.GetDateTime(queryModel.StartTime + " 00:00")));
                }
                if (!string.IsNullOrEmpty(queryModel.EndTime))
                {
                    strSql.Append(" AND SO.OrderDate <= @EndTime ");
                    parameter.Add(DbFactory.CreateDbParameter("@EndTime",
                        CommonHelper.GetDateTime(queryModel.EndTime + " 23:59:59")));
                }
                if (queryModel.Status.HasValue)
                {
                    strSql.Append(" AND SO.Status = @Status ");
                    parameter.Add(DbFactory.CreateDbParameter("@Status", queryModel.Status.Value));
                }
                if (queryModel.StatusWithPrint)
                {
                    strSql.AppendFormat(" AND SO.Status IN({0},{1}) ", (int)OrderStatus.WaitPick, (int)OrderStatus.WaitOutStock);
                    parameter.Add(DbFactory.CreateDbParameter("@StatusWithPrint", queryModel.StatusWithPrint));
                }
                if (queryModel.PrintStatus.HasValue)
                {
                    strSql.Append(" AND SO.PrintStatus = @PrintStatus ");
                    parameter.Add(DbFactory.CreateDbParameter("@PrintStatus", queryModel.PrintStatus.Value));
                }
                if (!string.IsNullOrEmpty(queryModel.ProductCode))
                {
                    strSql.AppendFormat(@" AND EXISTS ( SELECT *
                                                 FROM   dbo.SaleOrder_Item si
                                                        INNER JOIN dbo.Product product ON si.ProductId = product.ProductId
                                                 WHERE  si.OrderId = SO.OrderId
                                                        AND product.Code LIKE '{0}')", '%' + queryModel.ProductCode + '%');
                }
                if (!string.IsNullOrEmpty(queryModel.ProductName))
                {
                    strSql.AppendFormat(@" AND EXISTS ( SELECT *
                                                 FROM   dbo.SaleOrder_Item si
                                                        INNER JOIN dbo.Product product ON si.ProductId = product.ProductId
                                                 WHERE  si.OrderId = SO.OrderId
                                                        AND product.ProductName LIKE '{0}')", '%' + queryModel.ProductName + '%');
                }

                if (!string.IsNullOrEmpty(queryModel.Province))
                {
                    strSql.AppendFormat(@" AND SO.Province LIKE '{0}'", '%' + queryModel.Province + '%');
                }
                if (!string.IsNullOrEmpty(queryModel.City))
                {
                    strSql.AppendFormat(@" AND SO.City LIKE '{0}'", '%' + queryModel.City + '%');
                }
                if (!string.IsNullOrEmpty(queryModel.ReceiveAddress))
                {
                    strSql.AppendFormat(@" AND SO.ReceiveAddress LIKE '{0}'", '%' + queryModel.ReceiveAddress + '%');
                }
                if (!string.IsNullOrEmpty(queryModel.County))
                {
                    strSql.AppendFormat(@" AND SO.County LIKE '{0}'", '%' + queryModel.County + '%');
                }
                if (queryModel.LockMinute == null)
                {
                    queryModel.LockMinute = 0;
                }
                strSql.Append(" AND (SO.LockUserId='' OR SO.LockUserId IS NULL OR  SO.UnLockTime < GETDATE()) ");
                parameter.Add(DbFactory.CreateDbParameter("@LockUserId", userId));
                parameter.Add(DbFactory.CreateDbParameter("@LockUserName", userName));
                parameter.Add(DbFactory.CreateDbParameter("@LockMinute", queryModel.LockMinute));
                
                StringBuilder strSelectSql = new StringBuilder();
                if (jqgridparam.page == 0)
                {
                    jqgridparam.page = 1;
                }
                int num = (jqgridparam.page - 1) * jqgridparam.rows;
                int num1 = (jqgridparam.page) * jqgridparam.rows;
                string orderBy = "";
                if (!string.IsNullOrEmpty(jqgridparam.sidx))
                    orderBy = "Order By " + jqgridparam.sidx + " " + jqgridparam.sord + "";
                else
                    orderBy = "Order By (select 0)";
                strSelectSql.AppendFormat(@"
                        BEGIN TRAN;
                        UPDATE  SaleOrder WITH ( TABLOCKX )
                        SET     LockUserId = '' ,
                                LockUserName = '' ,
                                UnLockTime = DATEADD(MINUTE, -1, GETDATE())
                        WHERE   LockUserId = @LockUserId;
                        SELECT  COUNT(1) AS TotalRows
                        FROM    ( {0} ) AS t;
                        SELECT  *
                        INTO    ##SaleOrder
                        FROM    ( SELECT    ROW_NUMBER() OVER ( {1} ) AS rowNum ,
                                            *
                                  FROM      ( {0} ) AS T
                                ) AS N
                        WHERE   rowNum > {2}
                                AND rowNum <= {3};", strSql, orderBy, num, num1);
                if (queryModel.LockMinute > 0)
                {
                    strSelectSql.Append(@"
                        UPDATE  SaleOrder
                        SET     LockUserId = @LockUserId,
                                LockUserName = @LockUserName,
                                UnLockTime = DATEADD(MINUTE, @LockMinute, GETDATE())
                        WHERE   OrderId IN(SELECT OrderId
                                                FROM   ##SaleOrder
                                                WHERE  Status IN(-1, 0, 1, 2)); ");
                }
                strSelectSql.Append(@"
                        SELECT  *
                        FROM    ##SaleOrder;

                        DROP TABLE ##SaleOrder;

                        COMMIT;");
                DataSet ds = DbHelper.GetDataSet(CommandType.Text, strSelectSql.ToString(), parameter.ToArray());
                jqgridparam.records = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                return DatabaseReader.DataTableToList<SaleOrderViewModel>(ds.Tables[1]);
            }
            else
            {
                if (queryModel.QueryType == "Suspend")
                {
                    strSql.Append(" AND SO.IsSuspended = 1");
                }
                if (queryModel.QueryType == "Lock1")
                {
                    strSql.Append(" AND SO.LockUserId = @LockUserId ");
                    parameter.Add(DbFactory.CreateDbParameter("@LockUserId", userId));
                }
                if (queryModel.QueryType == "Lock2")
                {
                    strSql.Append(" AND SO.LockUserId != @LockUserId AND SO.LockUserId > '' ");
                    parameter.Add(DbFactory.CreateDbParameter("@LockUserId", userId));
                }
                return new Repository<SaleOrderViewModel>().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
            }
        }

        /// <summary>
        /// 释放指定订单锁单数据
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateUnLockUserIdByOrderNo(string orderNo, DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.SaleOrder
                            SET LockUserId = '', UnLockTime = DATEADD(MINUTE, -1, GETDATE())
                            WHERE OrderNo = @OrderNo ");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@OrderNo", orderNo)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray(), isOpenTrans) > 0;
        }

        /// <summary>
        /// 订单正在打印中
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateIsPrinting(string orderNo, DbTransaction isOpenTrans)
        {
            string printUserId = ManageProvider.Provider.Current().UserId;
            string printUserName = ManageProvider.Provider.Current().UserName;

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.SaleOrder
                            SET PrintStatus = @PrintStatus, PrintUserId = @PrintUserId,
                                PrintUserName = @PrintUserName
                            WHERE OrderNo = @OrderNo
                                AND PrintStatus = @CurrentStatus");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@PrintStatus", (int) PrintStatus.Printed),
                DbFactory.CreateDbParameter("@PrintUserId", printUserId),
                DbFactory.CreateDbParameter("@PrintUserName", printUserName),
                DbFactory.CreateDbParameter("@CurrentStatus", (int) PrintStatus.WaitPrint),
                DbFactory.CreateDbParameter("@OrderNo", orderNo)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray(), isOpenTrans) > 0;
        }

        /// <summary>
        /// 根据打印批次获取订单列表
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public List<SaleOrderViewModel> GetOrderListByPrintBatch(string batchId, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  SO.* ,
                                    W.WarehouseName ,
                                    M.FullName AS MerchantName ,
                                    MM.MallName ,
                                    S.ShipTypeName
                            FROM    SaleOrder SO
                                    INNER JOIN dbo.Print_BatchItem batchItem ON batchItem.OrderNo = SO.OrderNo
                                    LEFT JOIN dbo.Warehouse W ON SO.WarehouseId = W.WarehouseId
                                    LEFT JOIN dbo.Merchant M ON SO.MerchantId = M.MerchantId
                                    LEFT JOIN dbo.Merchant_Mall MM ON SO.MerchantMallId = MM.MallId
                                    LEFT JOIN dbo.ShipType S ON SO.ShipTypeId = S.ShipTypeId
                            WHERE   batchItem.BatchId = @BatchId");

            parameter.Add(DbFactory.CreateDbParameter("@BatchId", batchId));
            return new Repository<SaleOrderViewModel>().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public SaleOrderViewModel GetOrderViewModel(string orderNo)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT SO.*, W.WarehouseName, M.FullName AS MerchantName, MM.MallName,
                                S.ShipTypeName
                            FROM SaleOrder SO
                            LEFT JOIN dbo.Warehouse W ON SO.WarehouseId = W.WarehouseId
                            LEFT JOIN dbo.Merchant M ON SO.MerchantId = M.MerchantId
                            LEFT JOIN dbo.Merchant_Mall MM ON SO.MerchantMallId = MM.MallId
                            LEFT JOIN dbo.ShipType S ON SO.ShipTypeId = S.ShipTypeId");
            strSql.Append(" WHERE SO.OrderNo = @OrderNo ");
            parameter.Add(DbFactory.CreateDbParameter("@OrderNo", orderNo));
            return new Repository<SaleOrderViewModel>().FindEntityBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 更新面单号
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="expressNum"></param>
        /// <param name="overwrite"></param>
        /// <param name="isOpenTrans"></param>
        public bool UpdateExpressNum(string orderNo, string expressNum, bool overwrite, DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.SaleOrder
                            SET ExpressNum = @ExpressNum
                            WHERE OrderNo = @OrderNo");
            if (!overwrite)
            {
                strSql.Append(" And IsNULL(ExpressNum,'') = ''");
            }
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@OrderNo", orderNo),
                DbFactory.CreateDbParameter("@ExpressNum", expressNum),
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray(), isOpenTrans) > 0;
        }

        /// <summary>
        /// 订单明细列表
        /// </summary>
        /// <param name="orderNo">订单主键</param>
        /// <returns></returns>
        public List<SaleOrderItemEntity> GetOrderItemList(string orderNo)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM SaleOrder_Item WHERE OrderNo = @OrderNo");
            parameter.Add(DbFactory.CreateDbParameter("@OrderNo", orderNo));
            return DataFactory.Database().FindListBySql<SaleOrderItemEntity>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<SaleOrderItemEntity> GetOrderItemListByOrderId(string orderId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM SaleOrder_Item WHERE OrderId = @OrderId");
            parameter.Add(DbFactory.CreateDbParameter("@OrderId", orderId));
            return DataFactory.Database().FindListBySql<SaleOrderItemEntity>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 订单
        /// </summary>
        /// <param name="expressNum">物流单号</param>
        /// <returns></returns>
        public SaleOrderEntity GetSaleOrderByExpressNum(string expressNum)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                                FROM    dbo.SaleOrder
                                WHERE   ExpressNum = @ExpressNum");
            parameter.Add(DbFactory.CreateDbParameter("@ExpressNum", expressNum));
            return DataFactory.Database().FindEntityBySql<SaleOrderEntity>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public SaleOrderEntity GetSaleOrder(string orderNo)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                                FROM    dbo.SaleOrder
                                WHERE   OrderNo = @OrderNo");
            parameter.Add(DbFactory.CreateDbParameter("@OrderNo", orderNo));
            return DataFactory.Database().FindEntityBySql<SaleOrderEntity>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 订单明细列表
        /// </summary>
        /// <param name="expressNum">物流单号</param>
        /// <returns></returns>
        public List<SaleOrderItemEntity> GetOrderItemListByExpressNum(string expressNum)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  si.*
                                FROM    [SaleOrder_Item] si
                                        INNER JOIN dbo.SaleOrder so ON si.OrderNo = so.OrderNo
                                WHERE   so.ExpressNum = @ExpressNum");
            parameter.Add(DbFactory.CreateDbParameter("@ExpressNum", expressNum));
            return DataFactory.Database().FindListBySql<SaleOrderItemEntity>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="order"></param>
        /// <param name="currentStatus">订单当前状态</param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateStatus(SaleOrderEntity order, OutStockStatus currentStatus, DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.SaleOrder
                            SET OutStockStatus = @OutStockStatus, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE OrderId = @OrderId
                                AND Status = @CurrentStatus");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyUserId", order.ModifyUserId),
                DbFactory.CreateDbParameter("@ModifyUserName", order.ModifyUserName),
                DbFactory.CreateDbParameter("@ModifyDate", order.ModifyDate),
                DbFactory.CreateDbParameter("@OutStockStatus", order.OutStockStatus),
                DbFactory.CreateDbParameter("@CurrentStatus", currentStatus),
                DbFactory.CreateDbParameter("@OrderId", order.OrderId)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray(), isOpenTrans) > 0;
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="order"></param>
        /// <param name="currentStatus"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateStatus(SaleOrderEntity order, OrderStatus currentStatus, DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.SaleOrder
                            SET Status = @Status, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE OrderId = @OrderId");
            strSql.AppendFormat(" AND Status = {0}", (int)currentStatus);
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyUserId", order.ModifyUserId),
                DbFactory.CreateDbParameter("@ModifyUserName", order.ModifyUserName),
                DbFactory.CreateDbParameter("@ModifyDate", order.ModifyDate),
                DbFactory.CreateDbParameter("@Status", order.Status),
                DbFactory.CreateDbParameter("@OrderId", order.OrderId)
            };

            return Repository().ExecuteBySql(strSql, parameter.ToArray(), isOpenTrans) > 0;
        }

        /// <summary>
        /// 订单挂起
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool SuspendOrder(SaleOrderEntity order)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.SaleOrder
                            SET IsSuspended = @IsSuspended, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE OrderId = @OrderId");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyUserId", order.ModifyUserId),
                DbFactory.CreateDbParameter("@ModifyUserName", order.ModifyUserName),
                DbFactory.CreateDbParameter("@ModifyDate", order.ModifyDate),
                DbFactory.CreateDbParameter("@IsSuspended", order.IsSuspended),
                DbFactory.CreateDbParameter("@OrderId", order.OrderId)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray()) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="orderStatus"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateOutStockStatus(string orderNo, OutStockStatus orderStatus, DbTransaction isOpenTrans)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.SaleOrder
                            SET OutStockStatus = @OutStockStatus, ModifyDate = @ModifyDate
                            WHERE OrderNo = @OrderNo");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyDate", DateTime.Now),
                DbFactory.CreateDbParameter("@OutStockStatus", (int)orderStatus),
                DbFactory.CreateDbParameter("@OrderNo", orderNo)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray(), isOpenTrans) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="merchantId"></param>
        /// <param name="pickNo"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public List<SaleOrderViewModel> GetWaitPackageOrderList(string warehouseId, string merchantId, string pickNo, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT SO.*, W.WarehouseName, M.FullName AS MerchantName, MM.MallName,
                                S.ShipTypeName, PK.PickNo
                            FROM SaleOrder SO
                            INNER JOIN dbo.Warehouse W ON SO.WarehouseId = W.WarehouseId
                            INNER JOIN dbo.Merchant M ON SO.MerchantId = M.MerchantId
                            INNER JOIN dbo.Merchant_Mall MM ON SO.MerchantMallId = MM.MallId
                            INNER JOIN dbo.ShipType S ON SO.ShipTypeId = S.ShipTypeId
                            INNER JOIN dbo.Pick_Item PK ON PK.OrderNo = SO.OrderNo");

            strSql.Append(" Where SO.OutStockStatus = @OutStockStatus ");
            parameter.Add(DbFactory.CreateDbParameter("@OutStockStatus", OutStockStatus.PickFinished));

            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND SO.WarehouseId = @WarehouseId ");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }

            if (!string.IsNullOrEmpty(merchantId))
            {
                strSql.Append(" AND SO.MerchantId = @MerchantId ");
                parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            }
            if (!string.IsNullOrEmpty(pickNo))
            {
                strSql.Append(" AND PK.PickNo = @PickNo ");
                parameter.Add(DbFactory.CreateDbParameter("@PickNo", pickNo));
            }
            return new Repository<SaleOrderViewModel>().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        /// <summary>
        /// 是否存在来源订单
        /// </summary>
        /// <param name="sourceOrderNo"></param>
        /// <returns></returns>
        public bool IsExistSourceOrderNo(string sourceOrderNo)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT COUNT(1) FROM dbo.SaleOrder WHERE SourceOrderNo = @SourceOrderNo AND Status != @Status");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@Status", (int)OrderStatus.Canceled),
                DbFactory.CreateDbParameter("@SourceOrderNo", sourceOrderNo)
            };

            return DataFactory.Database().FindCountBySql(strSql.ToString(), parameter.ToArray()) > 0;
        }

        /// <summary>
        /// 修改物流方式
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateShipType(SaleOrderEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.SaleOrder
                            SET ShipTypeId = @ShipTypeId, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE OrderId = @OrderId");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyUserId", entity.ModifyUserId),
                DbFactory.CreateDbParameter("@ModifyUserName", entity.ModifyUserName),
                DbFactory.CreateDbParameter("@ModifyDate", entity.ModifyDate),
                DbFactory.CreateDbParameter("@ShipTypeId", entity.ShipTypeId),
                DbFactory.CreateDbParameter("@OrderId", entity.OrderId)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray()) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public bool UpdateWarehouse(SaleOrderEntity entity, OrderStatus orderStatus)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.SaleOrder
                            SET WarehouseId = @WarehouseId, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE OrderId = @OrderId
                                AND Status = @CurrentStatus");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyUserId", entity.ModifyUserId),
                DbFactory.CreateDbParameter("@ModifyUserName", entity.ModifyUserName),
                DbFactory.CreateDbParameter("@ModifyDate", entity.ModifyDate),
                DbFactory.CreateDbParameter("@CurrentStatus", orderStatus),
                DbFactory.CreateDbParameter("@WarehouseId", entity.WarehouseId),
                DbFactory.CreateDbParameter("@OrderId", entity.OrderId)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray()) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public List<SaleOrderItemViewModel> GetOrderItemViewModel(string orderNo)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM SaleOrder_Item WHERE OrderNo = @OrderNo");
            parameter.Add(DbFactory.CreateDbParameter("@OrderNo", orderNo));
            return DataFactory.Database().FindListBySql<SaleOrderItemViewModel>(strSql.ToString(), parameter.ToArray());
        }


        public List<string> CheckOrderPrintStatus(string orderNos)
        {
            StringBuilder sbOrderNos = new StringBuilder();
            var aryOrderNo = orderNos.Split(',');
            foreach (var orderNo in aryOrderNo)
            {
                if (!string.IsNullOrWhiteSpace(orderNo))
                {
                    sbOrderNos.AppendFormat("'{0}',", orderNo);
                }
            }
            List<string> lstOrderNo = new List<string>();
            string strSql = string.Format(@"SELECT OrderNo FROM SaleOrder WHERE PrintStatus != 0 
                                          AND OrderNo IN ( {0} )", sbOrderNos.ToString().TrimEnd(','));
            DataTable dtResult = DataFactory.Database().FindTableBySql(strSql.ToString());
            foreach (DataRow dataRow in dtResult.Rows)
            {
                lstOrderNo.Add(dataRow["OrderNo"].ToString());
            }
            return lstOrderNo;
        }

        public void ExecSPOrderAllocInventory()
        {
            Repository().ExecuteByProc("SP_Order_AllocInventory");
        }
    }
}