using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using AnJie.ERP.Business;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;

namespace AnJie.ERP.Service
{
    public class OrderService
    {
        private readonly BaseCodeRuleBll _codeRuleBll = new BaseCodeRuleBll();

        private readonly ProductBLL _productBLL = new ProductBLL();

        private readonly SaleOrderBLL _saleOrderBLL = new SaleOrderBLL();

        private readonly ShipTypeBLL _shipTypeBLL = new ShipTypeBLL();

        private readonly InventoryBLL _inventoryBll = new InventoryBLL();

        private readonly WarehouseLocationBLL _locationBll = new WarehouseLocationBLL();

        private readonly InventoryLocationBLL _inventoryLocationBLL = new InventoryLocationBLL();

        private readonly PickItemBLL _pickItemBLL = new PickItemBLL();

        private readonly PickMasterBLL _pickMasterBll = new PickMasterBLL();

        private static readonly string SaleOrderCodeName = "SaleOrder";

        /// <summary>
        /// 订单导入
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dataResult"></param>
        /// <param name="errMessage"></param>
        /// <param name="importFile"></param>
        /// <returns></returns>
        public bool ImportOrder(SaleOrderImportFileEntity importFile, DataTable dt, out DataTable dataResult,
            out string errMessage)
        {
            string userId = ManageProvider.Provider.Current().UserId;

            //构造导入返回结果表
            DataTable resultTable = new DataTable("Result");
            resultTable.Columns.Add("rowid", typeof(string)); //行号
            resultTable.Columns.Add("locate", typeof(string)); //位置
            resultTable.Columns.Add("reason", typeof(string)); //原因
            errMessage = string.Empty;
            bool isSuccess = false;
            if (dt != null && dt.Rows.Count > 0)
            {
                IDatabase database = DataFactory.Database();
                try
                {
                    List<SaleOrderImportItemEntity> lstEntities = new List<SaleOrderImportItemEntity>();
                    foreach (DataRow item in dt.Rows)
                    {
                        var importEntity = new SaleOrderImportItemEntity();
                        importEntity.Create();
                        //												 			 	
                        importEntity.FileId = importFile.FileId;
                        importEntity.ProductCode = item["商品编码"].ToString();
                        if (string.IsNullOrWhiteSpace(importEntity.ProductCode))
                        {
                            throw new Exception("商品编码不能为空");
                        }
                        importEntity.SourceOrderNo = item["外部单号"].ToString();
                        if (string.IsNullOrWhiteSpace(importEntity.SourceOrderNo))
                        {
                            throw new Exception("外部订单号不能为空");
                        }
                        //string OrderDate = item["制单日期"].ToString();
                        importEntity.OrderDate = DateTime.Now;
                        importEntity.ReceiveContact = item["联系人"].ToString();
                        importEntity.ReceivePhone = item["固定电话"].ToString();
                        importEntity.ReceiveCellPhone = item["联系电话"].ToString();
                        importEntity.ReceiveZip = item["邮编"].ToString();
                        importEntity.ExpressNum = item["快递单号"].ToString();
                        importEntity.Province = item["所在省"].ToString();
                        importEntity.City = item["所在市"].ToString();
                        importEntity.County = item["所在县/区"].ToString();
                        importEntity.ReceiveAddress = item["详细地址"].ToString();
                        importEntity.Qty = int.Parse(item["数量"].ToString());
                        importEntity.SellerNote = item["卖家留言"].ToString();
                        importEntity.BuyerNote = item["买家留言"].ToString();
                        importEntity.ShipTypeName = item["物流方式"].ToString();
                        importEntity.Remark = item["备注"].ToString();

                        bool flag = _saleOrderBLL.IsExistSourceOrderNo(importEntity.SourceOrderNo);
                        if (flag)
                        {
                            throw new Exception(string.Format("外部单号{0}已存在，请作废后重新导入", importEntity.SourceOrderNo));
                        }

                        lstEntities.Add(importEntity);
                    }

                    lstEntities = lstEntities.OrderBy(a => a.SourceOrderNo).ToList();

                    var shipTypeList = _shipTypeBLL.GetList();

                    DbTransaction isOpenTrans = database.BeginTrans();

                    string sourceOrderNo = string.Empty;
                    string orderNo = string.Empty;
                    List<SaleOrderEntity> orderEntities = new List<SaleOrderEntity>();
                    List<SaleOrderItemEntity> orderItemEntities = new List<SaleOrderItemEntity>();
                    foreach (SaleOrderImportItemEntity orderImportEntity in lstEntities)
                    {

                        var shipType = shipTypeList.Find(a => a.ShipTypeName == orderImportEntity.ShipTypeName);
                        if (shipType == null)
                        {
                            throw new Exception(string.Format("物流方式{0}不存在", orderImportEntity.ShipTypeName));
                        }

                        SaleOrderEntity orderEntity = new SaleOrderEntity();
                        var select = orderEntities.Where(i => i.ReceiveContact == orderImportEntity.ReceiveContact
                                                              && i.ReceivePhone == orderImportEntity.ReceivePhone
                                                              &&
                                                              i.ReceiveCellPhone == orderImportEntity.ReceiveCellPhone
                                                              && i.ReceiveZip == orderImportEntity.ReceiveZip
                                                              && i.Province == orderImportEntity.Province
                                                              && i.City == orderImportEntity.City
                                                              && i.County == orderImportEntity.County
                                                              && i.ReceiveAddress == orderImportEntity.ReceiveAddress
                                                              && i.WarehouseId == importFile.WarehouseId
                                                              && i.ShipTypeId == shipType.ShipTypeId
                                                              && i.ExpressNum == orderImportEntity.ExpressNum
                                                              && i.SellerNote == orderImportEntity.SellerNote
                                                              && i.BuyerNote == orderImportEntity.BuyerNote
                                                              && i.Remark == orderImportEntity.Remark);
                        if (select.Any())
                        {
                            orderEntity = select.First();
                            var otherSourceOrderNos = orderEntity.SourceOrderNo.Split(',').ToList();
                            if (!otherSourceOrderNos.Contains(orderImportEntity.SourceOrderNo))
                            {
                                otherSourceOrderNos.Add(orderImportEntity.SourceOrderNo);
                            }
                            orderEntity.SourceOrderNo = String.Join(",", otherSourceOrderNos.ToArray());
                        }
                        else
                        {
                            orderEntity.Create();
                            orderEntity.OrderId = orderImportEntity.ItemId;
                            orderEntity.OrderNo = _codeRuleBll.GetBillCode(userId, SaleOrderCodeName);
                            orderEntity.SourceOrderNo = orderImportEntity.SourceOrderNo;
                            orderEntity.OrderDate = DateTime.Now;
                            orderEntity.WarehouseId = importFile.WarehouseId;
                            orderEntity.MerchantId = importFile.MerchantId;
                            orderEntity.MerchantMallId = importFile.MerchantMallId;
                            orderEntity.ShipTypeId = shipType.ShipTypeId;
                            orderEntity.Province = orderImportEntity.Province;
                            orderEntity.City = orderImportEntity.City;
                            orderEntity.County = orderImportEntity.County;
                            orderEntity.ReceiveAddress = orderImportEntity.ReceiveAddress;
                            orderEntity.ReceiveContact = orderImportEntity.ReceiveContact;
                            orderEntity.ReceivePhone = orderImportEntity.ReceivePhone;
                            orderEntity.ReceiveCellPhone = orderImportEntity.ReceiveCellPhone;
                            orderEntity.ReceiveZip = orderImportEntity.ReceiveZip;
                            orderEntity.SellerNote = orderImportEntity.SellerNote;
                            orderEntity.BuyerNote = orderImportEntity.BuyerNote;
                            orderEntity.ExpressNum = orderImportEntity.ExpressNum;
                            orderEntity.City = orderImportEntity.City;
                            orderEntity.Remark = orderImportEntity.Remark;
                            orderEntities.Add(orderEntity);

                            _codeRuleBll.OccupyBillCode(ManageProvider.Provider.Current().UserId, SaleOrderCodeName,
                                isOpenTrans);

                            sourceOrderNo = orderImportEntity.SourceOrderNo;
                            orderNo = orderEntity.OrderNo;
                        }

                        var product = _productBLL.GetProductByCode(orderImportEntity.ProductCode);
                        if (product == null)
                        {
                            throw new Exception(string.Format("商品编号{0}不存在", orderImportEntity.ProductCode));
                        }

                        var saleOrderItem = new SaleOrderItemEntity();
                        saleOrderItem.Create();
                        saleOrderItem.OrderId = orderEntity.OrderId;
                        saleOrderItem.OrderNo = orderNo;
                        saleOrderItem.ProductId = product.ProductId;
                        saleOrderItem.Code = product.Code;
                        saleOrderItem.ProductName = product.ProductName;
                        saleOrderItem.Weight = product.Weight;
                        saleOrderItem.Volume = product.Volume;
                        saleOrderItem.Specification = product.Specification;
                        saleOrderItem.BaseUnit = product.BaseUnit;
                        saleOrderItem.BarCode = product.BarCode;
                        saleOrderItem.Price = product.Price;
                        saleOrderItem.Qty = orderImportEntity.Qty;

                        orderItemEntities.Add(saleOrderItem);
                    }


                    database.Insert(importFile);
                    foreach (SaleOrderImportItemEntity orderImportEntity in lstEntities)
                    {
                        database.Insert(orderImportEntity, isOpenTrans);
                    }

                    foreach (SaleOrderEntity orderImportEntity in orderEntities)
                    {
                        database.Insert(orderImportEntity, isOpenTrans);
                    }

                    foreach (SaleOrderItemEntity orderImportEntity in orderItemEntities)
                    {
                        database.Insert(orderImportEntity, isOpenTrans);
                    }

                    database.Commit();
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    database.Rollback();
                    BaseSysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "异常错误：" + ex.Message);
                    isSuccess = false;
                    errMessage = ex.Message;
                }
            }
            dataResult = resultTable;
            return isSuccess;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public bool Audit(string orderNo, out string errMessage)
        {
            var entity = new RepositoryFactory<SaleOrderEntity>().Repository().FindEntity("OrderNo", orderNo);
            if (entity == null || entity.Status != (int)OrderStatus.WaitAudit)
            {
                errMessage = string.Format("订单{0}不是待审核状态，不能审核<br>", orderNo);
                return false;
            }

            if (entity.IsSuspended)
            {
                errMessage = string.Format("订单{0}已被挂起，不能操作<br>", orderNo);
                return false;
            }

            var orderItems = _saleOrderBLL.GetOrderItemList(entity.OrderNo);

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            try
            {

                entity.Modify(entity.OrderId);
                entity.Status = (int)OrderStatus.WaitConfirm;
                bool isSuccess = _saleOrderBLL.UpdateStatus(entity, OrderStatus.WaitAudit, isOpenTrans);
                if (!isSuccess)
                {
                    throw new Exception(string.Format("订单{0}更新状态失败<br>", orderNo));
                }

                //foreach (var item in orderItems)
                //{
                //    bool flag = _inventoryBll.UpdateInventoryByAllocate(entity.WarehouseId, item.ProductId, entity.MerchantId, item.Qty, isOpenTrans);
                //    if (!flag)
                //    {
                //        throw new Exception("更新已分配库存失败");
                //    }
                //}
                
                database.Commit();
                errMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                errMessage = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// 取消审核
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public bool CancelAudit(string orderNo,out string errMessage)
        {
            var entity = new RepositoryFactory<SaleOrderEntity>().Repository().FindEntity("OrderNo", orderNo);
            if (entity == null || entity.Status != (int)OrderStatus.WaitConfirm)
            {
                errMessage = string.Format("订单{0}不是已审核状态，不能取消审核<br>", orderNo);
                return false;
            }

            if (entity.IsSuspended)
            {
                errMessage = string.Format("订单{0}已被挂起，不能操作<br>", orderNo);
                return false;
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            try
            {
                entity.Modify(entity.OrderId);
                entity.Status = (int)OrderStatus.WaitAudit;
                bool isSuccess = _saleOrderBLL.UpdateStatus(entity, OrderStatus.WaitConfirm, isOpenTrans);
                if (!isSuccess)
                {
                    throw new Exception(string.Format("订单{0}更新状态失败<br>", orderNo));
                }

                database.Commit();
                errMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                errMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 确认发货
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public bool ConfirmShip(string orderNo, out string errMessage)
        {
            var order = new RepositoryFactory<SaleOrderEntity>().Repository().FindEntity("OrderNo", orderNo);
            if (order == null)
            {
                errMessage = string.Format("订单[{0}]数据异常", orderNo);
                return false;
            }

            if (order.Status != (int) OrderStatus.WaitConfirm)
            {
                errMessage = string.Format("订单{0}不是待发货状态，不能进行发货操作<br>", orderNo);
                return false;
            }

            if (order.IsSuspended)
            {
                errMessage = string.Format("订单{0}已被挂起，不能操作<br>", orderNo);
                return false;
            }

            var orderItems = _saleOrderBLL.GetOrderItemList(order.OrderNo);

            List<PickItemEntity> picks = new List<PickItemEntity>();
            foreach (SaleOrderItemEntity orderItem in orderItems)
            {
                //待分配数量
                int waitAllocateQty = orderItem.Qty;

                //商品库存
                var inventoryList = _inventoryLocationBLL.GetProductInventoryList(order.WarehouseId, orderItem.ProductId,
                    true);

                foreach (var inventoryEntity in inventoryList)
                {
                    //本储位拣货数量
                    int pickQty;

                    //当前储位可用数量
                    int availableQty = inventoryEntity.QtyOnHand - inventoryEntity.QtyAllocated -
                                       inventoryEntity.QtySuspense;
                    if (availableQty >= waitAllocateQty)
                    {
                        pickQty = waitAllocateQty;
                    }
                    else
                    {
                        pickQty = availableQty;
                    }

                    var location = _locationBll.GetLocationByCode(order.WarehouseId, inventoryEntity.LocationCode);
                    if (location == null)
                    {
                        continue;
                    }

                    waitAllocateQty -= pickQty;

                    var pick = new PickItemEntity();
                    pick.Create();
                    pick.WarehouseId = order.WarehouseId;
                    pick.ProductId = orderItem.ProductId;
                    pick.LocationCode = inventoryEntity.LocationCode;
                    pick.ZoneCode = location.AllocZone;
                    pick.ToLocationCode = "PACK";
                    pick.OrderNo = order.OrderNo;
                    pick.Qty = pickQty;
                    picks.Add(pick);

                    if (waitAllocateQty == 0)
                    {
                        break;
                    }
                }

                if (waitAllocateQty > 0)
                {
                    errMessage = string.Format("商品[{0}]库存不足，发货失败", orderItem.ProductName);
                    return false;
                }
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            try
            {
                order.Modify(order.OrderId);
                order.Status = (int) OrderStatus.WaitPick;
                bool isSuccess = _saleOrderBLL.UpdateStatus(order, OrderStatus.WaitConfirm, isOpenTrans);
                if (!isSuccess)
                {
                    throw new Exception("订单状态更新失败");
                }

                foreach (var pick in picks)
                {
                    bool flag = _inventoryLocationBLL.UpdateInventoryByAllocate(pick.WarehouseId, pick.ProductId,
                        pick.LocationCode,
                        pick.Qty, isOpenTrans);
                    if (flag)
                    {
                        database.Insert(pick, isOpenTrans);
                    }
                    else
                    {
                        throw new Exception("库存占用失败");
                    }
                }

                database.Commit();
                errMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                errMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 取消发货
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public bool CancelConfirmShip(string orderNo, out string errMessage)
        {
            var order = new RepositoryFactory<SaleOrderEntity>().Repository().FindEntity("OrderNo", orderNo);
            if (order == null)
            {
                errMessage = string.Format("订单[{0}]数据异常", orderNo);
                return false;
            }

            if (order.Status != (int) OrderStatus.WaitPick)
            {
                errMessage = string.Format("订单{0}不是已发货待拣货状态，不能进行取消发货操作<br>", orderNo);
                return false;
            }

            if (order.IsSuspended)
            {
                errMessage = string.Format("订单{0}已被挂起，不能操作<br>", orderNo);
                return false;
            }

            //获取到拣货明细

            var pickItemList = _pickItemBLL.GetPickItemListByOrderNo(order.OrderNo);
            var picks = pickItemList.Where(i => i.Status == (int) PickItemStatus.Initial).ToList();

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            try
            {
                order.Modify(order.OrderId);
                order.Status = (int) OrderStatus.WaitConfirm;
                bool isSuccess = _saleOrderBLL.UpdateStatus(order, OrderStatus.WaitPick, isOpenTrans);
                if (!isSuccess)
                {
                    throw new Exception("订单状态更新失败");
                }

                foreach (var pick in picks)
                {
                    bool flag = _inventoryLocationBLL.UpdateInventoryByAllocate(pick.WarehouseId, pick.ProductId,
                        pick.LocationCode,
                        -1*pick.Qty, isOpenTrans);
                    if (flag)
                    {
                        database.Delete<PickItemEntity>("ItemId", pick.ItemId, isOpenTrans);
                    }
                    else
                    {
                        throw new Exception("库存占用失败");
                    }
                }

                database.Commit();
                errMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                errMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 取消拣货
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public bool CancelCreatePick(string orderNo, out string errMessage)
        {
            var order = new RepositoryFactory<SaleOrderEntity>().Repository().FindEntity("OrderNo", orderNo);
            if (order == null)
            {
                errMessage = string.Format("订单[{0}]数据异常", orderNo);
                return false;
            }

            if (order.Status != (int) OrderStatus.WaitOutStock)
            {
                errMessage = string.Format("订单{0}不是待出库状态，不能进行取消拣货操作<br>", orderNo);
                return false;
            }

            if (order.IsSuspended)
            {
                errMessage = string.Format("订单{0}已被挂起，不能操作<br>", orderNo);
                return false;
            }

            //获取到拣货明细
            var pickItemList = _pickItemBLL.GetPickItemListByOrderNo(order.OrderNo);
            var pickMaster = new RepositoryFactory<PickMasterEntity>().Repository()
                .FindEntity("PickNo", pickItemList.First().PickNo);
            if (pickMaster == null)
            {
                errMessage = string.Format("订单{0}获取拣货单拣货单[{1}]数据异常", orderNo, pickItemList.First().PickNo);
                return false;
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            try
            {
                foreach (var pick in pickItemList)
                {
                    var isSuccess = _pickItemBLL.CancelUpdateOrderPickNo(order.OrderNo, pickMaster.PickNo, isOpenTrans);
                    if (!isSuccess)
                    {
                        throw new Exception("商品库存更新失败");
                    }

                    var orderEntity = new RepositoryFactory<SaleOrderEntity>().Repository()
                        .FindEntity("OrderNo", pick.OrderNo);
                    if (orderEntity == null)
                    {
                        throw new Exception(string.Format("订单[{0}]数据异常", orderNo));
                    }

                    if (orderEntity.IsSuspended)
                    {
                        throw new Exception(string.Format("订单{0}已被挂起，不能操作<br>", orderNo));
                    }

                    orderEntity.Modify(orderEntity.OrderId);
                    orderEntity.Status = (int) OrderStatus.WaitPick;

                    isSuccess = _saleOrderBLL.UpdateStatus(orderEntity, OrderStatus.WaitOutStock, isOpenTrans);
                    if (!isSuccess)
                    {
                        throw new Exception("订单状态更新失败");
                    }
                }
                database.Delete<PickMasterEntity>("PickId", pickMaster.PickId, isOpenTrans);

                database.Commit();
                errMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                errMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 取消出库
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public bool CancelOutStock(string orderNo, out string errMessage)
        {
            var order = new RepositoryFactory<SaleOrderEntity>().Repository().FindEntity("OrderNo", orderNo);
            if (order == null)
            {
                errMessage = string.Format("订单[{0}]数据异常", orderNo);
                return false;
            }

            if (order.Status != (int) OrderStatus.OutStock)
            {
                errMessage = string.Format("订单{0}不是已出库状态，不能进行取消出库操作<br>", orderNo);
                return false;
            }

            if (order.IsSuspended)
            {
                errMessage = string.Format("订单{0}已被挂起，不能操作<br>", orderNo);
                return false;
            }

            //获取到拣货明细
            var pickItemList = _pickItemBLL.GetPickItemListByOrderNo(order.OrderNo);
            var pickMaster = new RepositoryFactory<PickMasterEntity>().Repository()
                .FindEntity("PickNo", pickItemList.First().PickNo);
            if (pickMaster == null)
            {
                errMessage = string.Format("订单{0}获取拣货单拣货单[{1}]数据异常", orderNo, pickItemList.First().PickNo);
                return false;
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            try
            {

                if (pickMaster.Status != (int) PickMasterStatus.Initial)
                {
                    pickMaster.Modify(pickMaster.PickId);
                    pickMaster.Status = (int) PickMasterStatus.Initial;
                    var isSuccess = _pickMasterBll.UpdateStatus(pickMaster, PickMasterStatus.Picked, isOpenTrans);
                    if (!isSuccess)
                    {
                        throw new Exception("订单状态更新失败");
                    }
                }

                List<string> lstOrderNo = new List<string>();
                List<SaleOrderEntity> listOrderEnt = new List<SaleOrderEntity>();

                foreach (var pick in pickItemList)
                {
                    bool outStock1 = _inventoryLocationBLL.UpdateInventoryByOutStock(pick.WarehouseId,InventoryLocationTransactionType.CancelOutStock, pick.ProductId,
                        pick.LocationCode, -1 * pick.Qty, isOpenTrans);
                    if (!outStock1)
                    {
                        throw new Exception(string.Format("更新储位{0}库存失败", pick.LocationCode));
                    }

                    bool outStock2 = _inventoryBll.UpdateInventoryByOutStock(order.OrderNo, InventoryTransactionType.CancelOutStock, pick.WarehouseId,
                        order.MerchantId, pick.ProductId, -1 * pick.Qty, isOpenTrans);
                    if (!outStock2)
                    {
                        throw new Exception("更新在库库存失败");
                    }

                    if (!lstOrderNo.Contains(pick.OrderNo))
                    {
                        lstOrderNo.Add(pick.OrderNo);
                        var orderEntity = new RepositoryFactory<SaleOrderEntity>().Repository()
                            .FindEntity("OrderNo", pick.OrderNo);
                        if (orderEntity == null)
                        {
                            throw new Exception(string.Format("订单[{0}]数据异常", orderNo));
                        }

                        if (orderEntity.IsSuspended)
                        {
                            throw new Exception(string.Format("订单{0}已被挂起，不能操作<br>", orderNo));
                        }
                        listOrderEnt.Add(orderEntity);
                    }
                }

                foreach (string item in lstOrderNo)
                {
                    bool flag = _saleOrderBLL.UpdateOutStockStatus(item, OutStockStatus.Initial, isOpenTrans);
                    if (!flag)
                    {
                        throw new Exception(string.Format("订单[{0}]状态更新失败", item));
                    }
                }
                foreach (var item in listOrderEnt)
                {
                    item.Modify(order.OrderId);
                    item.Status = (int)OrderStatus.WaitOutStock;
                    var isSuccess = _saleOrderBLL.UpdateStatus(item, OrderStatus.OutStock, isOpenTrans);
                    if (!isSuccess)
                    {
                        throw new Exception("订单状态更新失败");
                    }
                }

                database.Commit();
                errMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                errMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 作废订单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public bool InvalidOrder(string orderNo, out string errMessage)
        {
            var order = new RepositoryFactory<SaleOrderEntity>().Repository().FindEntity("OrderNo", orderNo);
            if (order == null)
            {
                errMessage = string.Format("订单[{0}]数据异常", orderNo);
                return false;
            }

            if (order.Status != (int) OrderStatus.Initial && order.Status != (int) OrderStatus.WaitAudit &&
                order.Status != (int) OrderStatus.OutOfStock)
            {
                errMessage = string.Format("订单{0}不是 初始/缺货/待审核 状态，不能作废<br>", orderNo);
                return false;
            }

            OrderStatus currentStatus = (OrderStatus) order.Status;
            order.Modify(order.OrderId);
            order.Status = (int) OrderStatus.Canceled;

            var orderItems = _saleOrderBLL.GetOrderItemList(order.OrderNo);

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            try
            {
                var isSuccess = _saleOrderBLL.UpdateStatus(order, currentStatus, isOpenTrans);
                if (!isSuccess)
                {
                    throw new Exception("订单状态更新失败");
                }

                foreach (var item in orderItems)
                {
                    bool flag = _inventoryBll.UpdateInventoryByAllocate(order.WarehouseId, item.ProductId, order.MerchantId, -1 * item.Qty, isOpenTrans);
                    if (!flag)
                    {
                        throw new Exception("更新已分配库存失败");
                    }
                }

                database.Commit();
                errMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                errMessage = ex.Message;
                return false;
            }
        }
    }
}