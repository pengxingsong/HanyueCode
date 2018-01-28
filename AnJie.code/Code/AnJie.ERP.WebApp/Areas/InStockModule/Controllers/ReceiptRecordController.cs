using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AnJie.ERP.Business.InStockModule;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Repository;
using AnJie.ERP.ViewModel.InStockModule;

namespace AnJie.ERP.WebApp.Areas.InStockModule.Controllers
{
    /// <summary>
    /// Receipt_Record控制器
    /// </summary>
    public class ReceiptRecordController : PublicController<ReceiptRecordEntity>
    {
        private readonly ReceiptBLL _receiptBll = new ReceiptBLL();

        private readonly WarehouseLocationBLL _locationBLL = new WarehouseLocationBLL();

        private readonly InventoryBLL _inventoryBLL = new InventoryBLL();

        private readonly InventoryLocationBLL _inventoryLocationBLL = new InventoryLocationBLL();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiptNo"></param>
        /// <param name="receiptRecordJson"></param>
        /// <returns></returns>
        public ActionResult SubmitReceiptRecord(string receiptNo, string receiptRecordJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "操作成功。";

                ReceiptEntity receipt = _receiptBll.Repository().FindEntity("ReceiptNo", receiptNo);
                if (receipt == null)
                {
                    throw new Exception("收货单不存在");
                }

                var receiptItemList = _receiptBll.GetReceiptItemList(receipt.ReceiptId);

                List<ReceiptRecordViewModel> receiptRecordList = receiptRecordJson.JonsToList<ReceiptRecordViewModel>();
                List<ReceiptRecordEntity> record = new List<ReceiptRecordEntity>();
                foreach (ReceiptRecordViewModel item in receiptRecordList)
                {
                    if (!string.IsNullOrEmpty(item.ItemId))
                    {
                        var recordItem = new ReceiptRecordEntity();
                        recordItem.Create();
                        recordItem.ReceiptId = receipt.ReceiptId;
                        recordItem.ReceiptItemId = item.ItemId;
                        recordItem.ReceivedQty = item.ReceivedQty;
                        recordItem.LocationCode = item.LocationCode;
                        recordItem.Status = 0;

                        foreach (var receiptItemEntity in receiptItemList)
                        {
                            if (receiptItemEntity.ItemId == recordItem.ReceiptItemId)
                            {
                                recordItem.ProductId = receiptItemEntity.ProductId;
                                if (receiptItemEntity.Qty < receiptItemEntity.ReceivedQty + recordItem.ReceivedQty)
                                {
                                    throw new Exception(string.Format("{0}收货数量超过预计收货数量", receiptItemEntity.ProductName));
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(recordItem.ProductId))
                        {
                            throw new Exception("商品编号无效");
                        }

                        WarehouseLocationEntity location = _locationBLL.GetLocationByCode(receipt.WarehouseId,
                            recordItem.LocationCode);
                        if (location == null)
                        {
                            throw new Exception(string.Format("当前仓库没有该储位{0}", recordItem.LocationCode));
                        }
                        recordItem.LocationId = location.LocationId;

                        if (recordItem.ReceivedQty > 0)
                        {
                            record.Add(recordItem);
                        }
                    }
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
                        receipt.ReceiptId,
                        item.Value, isOpenTrans);

                    _inventoryLocationBLL.UpdateInventoryByMoveIn(receipt.WarehouseId, InventoryLocationTransactionType.Receive, itemValue[0], "",
                        itemValue[1],
                        item.Value, isOpenTrans);
                }

                receipt.Modify(receipt.ReceiptId);
                receipt.Status = (int) ReceiptStatus.Receiving;
                _receiptBll.UpdateReceiptStatus(receipt);

                database.Commit();
                return Content(new JsonMessage {Success = true, Code = "1", Message = Message}.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "操作失败：" + ex.Message}.ToString());
            }
        }
    }
}