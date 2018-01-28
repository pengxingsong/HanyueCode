using System;
using System.Collections.Generic;
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
    public class PackageService
    {
        private readonly SaleOrderBLL _orderBll = new SaleOrderBLL();

        private static readonly string CartonCodeName = "Carton";

        private readonly BaseCodeRuleBll _codeRuleBll = new BaseCodeRuleBll();

        private readonly InventoryLocationBLL _inventoryLocationBLL = new InventoryLocationBLL();

        /// <summary>
        /// 生成拣货单号
        /// </summary>
        /// <returns></returns>
        private string CreateCartonNum()
        {
            string userId = ManageProvider.Provider.Current().UserId;
            return _codeRuleBll.GetBillCode(userId, CartonCodeName);
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="packageNum"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Package(string orderNo, int packageNum, out string message)
        {
            if (packageNum >= 99)
            {
                message = string.Format("订单[{0}]最多只能打99个包裹", orderNo);
                return false;
            }

            var order = new RepositoryFactory<SaleOrderEntity>().Repository().FindEntity("OrderNo", orderNo);
            if (order == null)
            {
                message = string.Format("订单[{0}]数据异常", orderNo);
                return false;
            }

            if (order.OutStockStatus != (int)OutStockStatus.PickFinished)
            {
                message = string.Format("订单[{0}]不是拣货完成状态，不能打包", order.OrderNo);
                return false;
            }

            List<CartonEntity> cartons = new List<CartonEntity>();
            for (int i = 1; i <= packageNum; i++)
            {
                CartonEntity carton = new CartonEntity();
                carton.Create();
                carton.CartonNum = CreateCartonNum() + i.ToString("00");
                carton.WarehouseId = order.WarehouseId;
                carton.MerchantId = order.MerchantId;
                carton.OrderNo = order.OrderNo;
                carton.ShipTypeId = order.ShipTypeId;
                carton.TotalCount = packageNum;
                carton.CurrentNum = i;
                carton.Status = 0;
                cartons.Add(carton);
            }

            var orderItems = _orderBll.GetOrderItemList(order.OrderNo);

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                order.Modify(order.OrderId);
                order.OutStockStatus = (int)OutStockStatus.Packaged;
                bool isSuccess = _orderBll.UpdateStatus(order, OutStockStatus.PickFinished, isOpenTrans);
                if (!isSuccess)
                {
                    throw new Exception("订单状态更新失败");
                }

                foreach (SaleOrderItemEntity orderItem in orderItems)
                {
                    bool flag = _inventoryLocationBLL.UpdateInventoryByOutStock(order.WarehouseId, InventoryLocationTransactionType.Package, orderItem.ProductId, "PACK", orderItem.Qty, isOpenTrans);
                    if (!flag)
                    {
                        throw new Exception("库存扣减失败");
                    }
                }

                foreach (CartonEntity carton in cartons)
                {
                    database.Insert(carton, isOpenTrans);
                    _codeRuleBll.OccupyBillCode(ManageProvider.Provider.Current().UserId, CartonCodeName,
                        isOpenTrans);
                }

                database.Commit();
                message = string.Format("订单[{0}]打包完成", order.OrderNo);
                return true;
            }
            catch (Exception ex)
            {
                database.Rollback();
                message = string.Format("订单[{0}]打包失败:{1}", order.OrderNo, ex.Message);
                return false;
            }
        }
    }
}
