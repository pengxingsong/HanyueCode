using AnJie.ERP.Business;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using AnJie.ERP.ViewModel.OrderModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AnJie.ERP.Service;

namespace AnJie.ERP.WebApp.Areas.OutStockModule.Controllers
{
    /// <summary>
    /// 订单配货控制器
    /// </summary>
    public class AllocationController : PublicController<SaleOrderEntity>
    {
        private readonly SaleOrderBLL _orderBll = new SaleOrderBLL();

        /// <summary>
        /// 获取待配货订单列表
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="startTime">制单开始时间</param>
        /// <param name="endTime">制单结束时间</param>
        /// <param name="MerchantId"></param>
        /// <param name="jqgridparam">分页参数</param>
        /// <param name="WarehouseId"></param>
        /// <returns></returns>
        public ActionResult GetOrderList(string orderNo, string startTime, string endTime, string WarehouseId, string MerchantId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<SaleOrderViewModel> listData = new List<SaleOrderViewModel>();
                //= _orderBll.GetWaitOutSotckOrderList(WarehouseId, MerchantId, orderNo, OutStockStatus.Initial, startTime, endTime, jqgridparam);
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
        /// 订单明细列表（返回Json）
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        public ActionResult GetOrderItemList(string orderNo)
        {
            try
            {
                var jsonData = new
                {
                    rows = _orderBll.GetOrderItemList(orderNo),
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
        /// 手动配货
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public ActionResult Allocate(string orderIds)
        {
            try
            {
                var sb = new StringBuilder();
                var allocationService = new AllocationService();
                string[] aryOrderId = orderIds.Split(',');
                foreach (string orderId in aryOrderId)
                {
                    string message;
                    bool flag = allocationService.OrderAllocate(orderId, out message);
                    if (!flag)
                    {
                        sb.AppendFormat("{0}<br/>", message);
                    }
                    else
                    {
                        sb.AppendFormat("{0}<br/>", message);
                    }
                }
                 
                WriteLog(1, orderIds, sb.ToString());
                return Content(new JsonMessage { Success = true, Code = "1", Message = sb.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, orderIds, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}