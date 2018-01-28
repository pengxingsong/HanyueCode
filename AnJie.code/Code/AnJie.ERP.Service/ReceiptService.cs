using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using AnJie.ERP.Business;
using AnJie.ERP.Business.InStockModule;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;

namespace AnJie.ERP.Service
{
    public class ReceiptService
    {
        private readonly ReceiptBLL _receiptBll = new ReceiptBLL();

        private readonly ReceiptRecordBLL _receiptRecordBLL = new ReceiptRecordBLL();
        
        private readonly InventoryBLL _inventoryBLL = new InventoryBLL();
        
        private readonly InventoryLocationBLL _inventoryLocationBLL = new InventoryLocationBLL();

        private readonly WarehouseBLL _warehouseBLL = new WarehouseBLL();

        private readonly WarehouseLocationBLL _warehouseLocationBLL = new WarehouseLocationBLL();

        /// <summary>
        /// 收货单快速收货
        /// </summary>
        /// <param name="receipt"></param>
        /// <returns></returns>
        public bool ReceiptQuickReceive(ReceiptEntity receipt)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                var warehouse = _warehouseBLL.GetWarehouse(receipt.WarehouseId);
                if (string.IsNullOrWhiteSpace(warehouse?.ReceiptLocationId))
                {
                    throw new Exception("该仓库没有设置默认的收货储位");
                }

                var location = _warehouseLocationBLL.GetLocation(warehouse.WarehouseId, warehouse.ReceiptLocationId);
                if (string.IsNullOrWhiteSpace(location?.Code))
                {
                    throw new Exception("该仓库没有设置默认的收货储位");
                }

                var receiptItemList = _receiptBll.GetReceiptItemList(receipt.ReceiptId);

                List<ReceiptRecordEntity> record = new List<ReceiptRecordEntity>();
                foreach (var receiptItemEntity in receiptItemList)
                {
                    var recordItem = new ReceiptRecordEntity();
                    recordItem.Create();
                    recordItem.ReceiptId = receipt.ReceiptId;
                    recordItem.ReceiptItemId = receiptItemEntity.ItemId;
                    recordItem.ReceivedQty = receiptItemEntity.Qty;
                    recordItem.ProductId = receiptItemEntity.ProductId;
                    recordItem.LocationCode = location.Code;
                    recordItem.LocationId = location.LocationId;
                    recordItem.Status = 0;
                    record.Add(recordItem);
                }

                if (record.Count == 0)
                {
                    throw new Exception("没有有效的收货记录");
                }

                foreach (var receiptRecordEntity in record)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("UPDATE [Receipt_Item] SET ReceivedQty = ReceivedQty + {0} WHERE ItemId = '{1}'",
                        receiptRecordEntity.ReceivedQty.ToString(), receiptRecordEntity.ReceiptItemId);
                    database.ExecuteBySql(sb, isOpenTrans);

                    database.Insert(receiptRecordEntity, isOpenTrans);
                }

                record = record.OrderBy(a => a.ProductId).ThenBy(a => a.LocationCode).ToList();
                Dictionary<string, int> dicInventory = new Dictionary<string, int>();
                foreach (var receiptRecordEntity in record)
                {
                    string key = string.Format("{0}${1}", receiptRecordEntity.ProductId,
                        receiptRecordEntity.LocationCode);
                    if (!dicInventory.ContainsKey(key))
                    {
                        dicInventory.Add(key, receiptRecordEntity.ReceivedQty);
                    }
                    else
                    {
                        dicInventory[key] = dicInventory[key] + receiptRecordEntity.ReceivedQty;
                    }
                }

                foreach (var item in dicInventory)
                {
                    string[] itemValue = item.Key.Split('$');
                    _inventoryBLL.UpdateInventoryByReceive(receipt.ReceiptNo, InventoryTransactionType.Receive, receipt.WarehouseId, itemValue[0],
                        receipt.MerchantId,
                        item.Value, isOpenTrans);

                    _inventoryLocationBLL.UpdateInventoryByMoveIn(receipt.WarehouseId, InventoryLocationTransactionType.Receive, itemValue[0], "",
                        itemValue[1],
                        item.Value, isOpenTrans);
                }

                receipt.Modify(receipt.ReceiptId);
                receipt.Status = (int) ReceiptStatus.Received;
                _receiptBll.UpdateReceiptStatus(receipt);

                database.Commit();
                return true;

            }
            catch (Exception ex)
            {
                database.Rollback();
                return false;
            }
        }

        /// <summary>
        /// 取消收货
        /// </summary>
        /// <param name="receipt"></param>
        /// <returns></returns>
        public bool UnReceiptReceive(ReceiptEntity receipt)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                var receiptRecord = _receiptRecordBLL.GetReceiptRecordList(receipt.ReceiptId);
                receiptRecord = receiptRecord.OrderBy(a => a.ProductId).ThenBy(a => a.LocationCode).ToList();
                Dictionary<string, int> dicInventory = new Dictionary<string, int>();
                foreach (var receiptRecordEntity in receiptRecord)
                {
                    string key = string.Format("{0}${1}", receiptRecordEntity.ProductId,
                        receiptRecordEntity.LocationCode);
                    if (!dicInventory.ContainsKey(key))
                    {
                        dicInventory.Add(key, receiptRecordEntity.ReceivedQty);
                    }
                    else
                    {
                        dicInventory[key] = dicInventory[key] + receiptRecordEntity.ReceivedQty;
                    }
                }

                foreach (var item in dicInventory)
                {
                    string[] itemValue = item.Key.Split('$');
                    bool flag = _inventoryBLL.UpdateInventoryByUnReceive(receipt.ReceiptNo,InventoryTransactionType.CancelReceive, receipt.WarehouseId, itemValue[0],
                        receipt.MerchantId,
                        item.Value, isOpenTrans);
                    if (!flag)
                    {
                        throw new Exception("扣减已收货商户库存失败");
                    }

                    flag = _inventoryLocationBLL.UpdateInventoryByUnReceive(receipt.WarehouseId, InventoryLocationTransactionType.CancelReceive, itemValue[0],
                        itemValue[1],
                        item.Value, isOpenTrans);
                    if (!flag)
                    {
                        throw new Exception("扣减已收货库位库存失败");
                    }
                }

                receipt.Modify(receipt.ReceiptId);
                receipt.Status = (int) ReceiptStatus.Audited;
                _receiptBll.UpdateReceiptStatus(receipt);

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("UPDATE [Receipt_Item] SET ReceivedQty = 0 WHERE ReceiptId = '{0}';",
                    receipt.ReceiptId);
                sb.AppendFormat("UPDATE [Receipt_Record] SET Status = -1 WHERE ReceiptId = '{0}' And Status = 0;",
                    receipt.ReceiptId);

                database.ExecuteBySql(sb, isOpenTrans);

                database.Commit();
                return true;

            }
            catch (Exception ex)
            {
                database.Rollback();
                return false;
            }
        }
    }
}