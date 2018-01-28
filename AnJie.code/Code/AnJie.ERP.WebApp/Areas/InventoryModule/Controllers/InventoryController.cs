using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Utilities;
using AnJie.ERP.ViewModel.InventoryModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnJie.ERP.ViewModel.InStockModule;

namespace AnJie.ERP.WebApp.Areas.InventoryModule.Controllers
{
    /// <summary>
    /// Inventory控制器
    /// </summary>
    public class InventoryController : PublicController<InventoryEntity>
    {
        private readonly InventoryBLL _inventoryBLL = new InventoryBLL();

        private readonly InventoryTransactionBLL _inventoryTransactionBLL = new InventoryTransactionBLL();

        private readonly InventoryLocationBLL _inventoryLocationBLL = new InventoryLocationBLL();

        private readonly InventoryLocationTransactionBLL _inventoryLocationTransactionBLL = new InventoryLocationTransactionBLL();

        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult MerchantInventory()
        {
            return View();
        }

        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult LocationInventory()
        {
            return View();
        }

        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult InventoryTransaction()
        {
            return View();
        }

        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult InventoryLocationTransaction()
        {
            return View();
        }

        /// <summary>
        /// 商户库存列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MerchantInventoryGridList(string keywords, string warehouseId, string merchantId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<InventoryViewModel> listData = _inventoryBLL.GetInventoryList(keywords, warehouseId, merchantId, jqgridparam);
                var jsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = listData,
                };
                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 商户库存记录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult InventoryTransactionGridList(string keywords, string warehouseId, string merchantId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<InventoryTransactionViewModel> listData = _inventoryTransactionBLL.GetInventoryTransactionList(keywords, warehouseId, merchantId, jqgridparam);
                var jsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = listData,
                };
                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        
        /// <summary>
        /// 库存管理返回列表
        /// </summary>
        /// <returns></returns>
        public ActionResult LocationInventoryGridList(string keywords, string warehouseId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<InventoryLocationViewModel> listData = _inventoryLocationBLL.GetInventoryList(keywords, warehouseId,  jqgridparam);
                var jsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = listData,
                };
                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 库位库存记录
        /// </summary>
        /// <returns></returns>
        public ActionResult InventoryLocationTransactionGridList(string keywords, string warehouseId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<InventoryLocationTransactionViewModel> listData = _inventoryLocationTransactionBLL.GetInventoryLocationTransactionList(keywords, warehouseId, jqgridparam);
                var jsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = listData,
                };
                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 商品库存列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProductInventoryList(string warehouseId, string productId)
        {
            try
            {
                var jsonData = new
                {
                    rows = _inventoryBLL.GetProductInventoryList(warehouseId, productId, false),
                };
                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
    }
}