using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using AnJie.ERP.Business;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;

namespace AnJie.ERP.Service
{
    public class PickService
    {
        private readonly SaleOrderBLL _orderBll = new SaleOrderBLL();

        private readonly PickItemBLL _pickItemBll = new PickItemBLL();

        private readonly PickMasterBLL _pickMasterBll = new PickMasterBLL();

        private static readonly string PickMasterCodeName = "PickMaster";

        private readonly BaseCodeRuleBll _codeRuleBll = new BaseCodeRuleBll();

        private readonly InventoryLocationBLL _inventoryLocationBLL = new InventoryLocationBLL();

        /// <summary>
        /// 生成拣货单号
        /// </summary>
        /// <returns></returns>
        private string CreatePickMasterCode()
        {
            string userId = ManageProvider.Provider.Current().UserId;
            return _codeRuleBll.GetBillCode(userId, PickMasterCodeName);
        }

        /// <summary>
        /// 生成拣货单
        /// </summary>
        /// <param name="aryOrderNo"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool CreatePickMaster(string[] aryOrderNo, out string message)
        {
            string warehouseId = string.Empty;
            List<SaleOrderEntity> orderList = new List<SaleOrderEntity>();
            foreach (string orderNo in aryOrderNo)
            {
                var order = new RepositoryFactory<SaleOrderEntity>().Repository().FindEntity("OrderNo", orderNo);
                if (order == null)
                {
                    message = string.Format("订单[{0}]查询失败", orderNo);
                    return false;
                }

                if (!string.IsNullOrEmpty(warehouseId) && warehouseId != order.WarehouseId)
                {
                    message = "非同一仓库的订单不能生成拣货单";
                    return false;
                }

                if (string.IsNullOrEmpty(warehouseId))
                {
                    warehouseId = order.WarehouseId;
                }

                if (order.Status != (int)OrderStatus.WaitPick)
                {
                    message = string.Format("订单[{0}]不是待拣货状态，不能生成拣货单", order.OrderNo);
                    return false;
                }
                orderList.Add(order);
            }

            PickMasterEntity pickMaster = new PickMasterEntity();
            pickMaster.Create();
            pickMaster.WarehouseId = warehouseId;
            pickMaster.PickNo = CreatePickMasterCode();

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            try
            {
                foreach (SaleOrderEntity order in orderList)
                {
                    order.Modify(order.OrderId);
                    order.Status = (int)OrderStatus.WaitOutStock;

                    bool isSuccess = _orderBll.UpdateStatus(order, OrderStatus.WaitPick, isOpenTrans);
                    if (!isSuccess)
                    {
                        throw new Exception("订单状态更新失败");
                    }
                    isSuccess = _pickItemBll.UpdateOrderPickNo(order.OrderNo, pickMaster.PickNo, isOpenTrans);
                    if (!isSuccess)
                    {
                        throw new Exception("商品库存更新失败");
                    }
                }

                database.Insert(pickMaster, isOpenTrans);
                _codeRuleBll.OccupyBillCode(ManageProvider.Provider.Current().UserId, PickMasterCodeName, isOpenTrans);

                database.Commit();
                message = "生成拣货单成功";
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                message = string.Format("生成拣货单失败:{0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pickNo"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool PickConfirm(string pickNo, out string message)
        {
            var pickMaster = new RepositoryFactory<PickMasterEntity>().Repository().FindEntity("PickNo", pickNo);
            if (pickMaster == null)
            {
                message = string.Format("拣货单[{0}]数据异常", pickNo);
                return false;
            }

            if (pickMaster.Status != (int)PickMasterStatus.Initial &&
                pickMaster.Status != (int)PickMasterStatus.Picking)
            {
                message = string.Format("拣货单[{0}]不是初始或开始确认状态，确认失败", pickMaster.PickNo);
                return false;
            }

            var picks = _pickItemBll.GetPickItemListByPickNo(pickMaster.PickNo);

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                pickMaster.Modify(pickMaster.PickId);
                pickMaster.Status = (int)PickMasterStatus.Picked;
                bool isSuccess = _pickMasterBll.UpdateStatus(pickMaster, PickMasterStatus.Picking, isOpenTrans);
                if (!isSuccess)
                {
                    throw new Exception("订单状态更新失败");
                }

                List<string> lstOrderNo = new List<string>();
                foreach (var pick in picks)
                {
                    bool moveIn = _inventoryLocationBLL.UpdateInventoryByMoveIn(pick.WarehouseId, InventoryLocationTransactionType.Picked, pick.ProductId, pick.LocationCode, pick.ToLocationCode, pick.Qty, isOpenTrans);
                    if (!moveIn)
                    {
                        throw new Exception("更新目的储位库存失败");
                    }

                    bool moveOut = _inventoryLocationBLL.UpdateInventoryByPicked(pick.WarehouseId, InventoryLocationTransactionType.Picked, pick.ProductId, pick.ToLocationCode, pick.LocationCode, pick.Qty, isOpenTrans);
                    if (!moveOut)
                    {
                        throw new Exception("更新在库库存失败");
                    }

                    if (!lstOrderNo.Contains(pick.OrderNo))
                    {
                        lstOrderNo.Add(pick.OrderNo);
                    }
                }

                foreach (string orderNo in lstOrderNo)
                {
                    bool flag = _orderBll.UpdateOutStockStatus(orderNo, OutStockStatus.PickFinished, isOpenTrans);
                    if (!flag)
                    {
                        throw new Exception(string.Format("订单[{0}]状态更新失败", orderNo));
                    }
                }

                database.Commit();
                message = string.Format("拣货单[{0}]已确认完成", pickMaster.PickNo);
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                message = string.Format("拣货单[{0}]确认失败:{1}", pickMaster.PickNo, ex.Message);
                return false;
            }
        }
    }
}