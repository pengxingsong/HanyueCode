using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using AnJie.ERP.Business;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;

namespace AnJie.ERP.Service
{
    public class AllocationService
    {
        private readonly SaleOrderBLL _orderBll = new SaleOrderBLL();

        private readonly InventoryBLL _inventoryBll = new InventoryBLL();

        private readonly InventoryLocationBLL _inventoryLocationBLL = new InventoryLocationBLL();

        private readonly WarehouseLocationBLL _locationBll = new WarehouseLocationBLL();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool OrderAllocate(string orderId, out string message)
        {
            var order = new RepositoryFactory<SaleOrderEntity>().Repository().FindEntity("OrderId", orderId);
            if (order == null)
            {
                message = string.Format("订单[{0}]数据异常", orderId);
                return false;
            }

            if (order.OutStockStatus != (int)OutStockStatus.Initial)
            {
                message = string.Format("订单出库状态[{0}]不是初始状态，配货失败", order.OrderNo);
                return false;
            }

            var orderItems = _orderBll.GetOrderItemList(order.OrderNo);

            List<PickItemEntity> picks = new List<PickItemEntity>();
            foreach (SaleOrderItemEntity orderItem in orderItems)
            {
                //待分配数量
                int waitAllocateQty = orderItem.Qty;

                //商品库存
                var inventoryList = _inventoryLocationBLL.GetProductInventoryList(order.WarehouseId, orderItem.ProductId, true);

                foreach (var inventoryEntity in inventoryList)
                {
                    //本储位拣货数量
                    int pickQty = 0;

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
                    message = string.Format("商品[{0}]库存不足，配货失败", orderItem.ProductName);
                    return false;
                }
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            try
            {
                order.Modify(order.OrderId);
                order.OutStockStatus = (int)OutStockStatus.Allocated;
                bool isSuccess = _orderBll.UpdateStatus(order, OutStockStatus.Initial, isOpenTrans);
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
                message = string.Format("订单[{0}]配货成功", order.OrderNo);
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                message = string.Format("订单[{0}]配货失败:{1}", order.OrderNo, ex.Message);
                return false;
            }
        }
    }
}
