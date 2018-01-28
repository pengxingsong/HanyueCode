using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.DataAccess;
using System.Data.Common;
using AnJie.ERP.Repository;

namespace AnJie.ERP.Service
{
    public class OutboundService
    {
        private readonly SaleOrderBLL _orderBll = new SaleOrderBLL();

        private readonly PickItemBLL _pickItemBll = new PickItemBLL();

        private readonly InventoryLocationBLL _inventoryLocationBLL = new InventoryLocationBLL();

        private readonly InventoryBLL _inventoryBLL = new InventoryBLL();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billNo"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ScanBillNo(string billNo, out string message)
        {
            SaleOrderEntity orderEntity = billNo.ToLower().StartsWith("so")
                ? _orderBll.GetSaleOrder(billNo)
                : _orderBll.GetSaleOrderByExpressNum(billNo);

            if (orderEntity == null)
            {
                message = string.Format("无效的物流单号或订单号[{0}]", billNo);
                return false;
            }

            if (string.IsNullOrWhiteSpace(orderEntity.ExpressNum))
            {
                message = string.Format("订单[{0}]没有匹配物流单号不能出库校验", orderEntity.OrderNo);
                return false;
            }

            if (orderEntity.Status != (int) OrderStatus.WaitOutStock)
            {
                message = string.Format("订单[{0}]不是待拣货状态，不能出库校验", orderEntity.OrderNo);
                return false;
            }

            message = "";
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billNo"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ScanFinished(string billNo, out string message)
        {
            SaleOrderEntity orderEntity = billNo.ToLower().StartsWith("so")
                ? _orderBll.GetSaleOrder(billNo)
                : _orderBll.GetSaleOrderByExpressNum(billNo);

            if (orderEntity == null)
            {
                message = string.Format("无效的物流单号或订单号[{0}]", billNo);
                return false;
            }

            if (orderEntity.Status != (int) OrderStatus.WaitOutStock)
            {
                message = string.Format("订单[{0}]不是待拣货状态，不能出库校验", orderEntity.OrderNo);
                return false;
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                orderEntity.Status = (int) OrderStatus.OutStock;

                bool flag = _orderBll.UpdateStatus(orderEntity, OrderStatus.WaitOutStock, null);
                if (!flag)
                {
                    message = "修改订单状态出现异常，请重新操作";
                    return false;
                }

                var picks = _pickItemBll.GetPickItemListByOrderNo(orderEntity.OrderNo);
                foreach (var item in picks)
                {
                    bool outStock1 = _inventoryLocationBLL.UpdateInventoryByOutStock(item.WarehouseId, InventoryLocationTransactionType.OutStock, item.ProductId,
                        item.LocationCode, item.Qty, isOpenTrans);
                    if (!outStock1)
                    {
                        throw new Exception(string.Format("更新储位{0}库存失败", item.LocationCode));
                    }

                    bool outStock2 = _inventoryBLL.UpdateInventoryByOutStock(orderEntity.OrderNo, InventoryTransactionType.OutStock,item.WarehouseId,
                        orderEntity.MerchantId, item.ProductId, item.Qty, isOpenTrans);
                    if (!outStock2)
                    {
                        throw new Exception("更新在库库存失败");
                    }
                }

                database.Commit();
            }
            catch (Exception ex)
            {
                database.Rollback();
                message = string.Format("订单[{0}]出库校验失败:{1}", orderEntity.OrderNo, ex.Message);
                return false;
            }

            message = "";
            return true;
        }
    }
}