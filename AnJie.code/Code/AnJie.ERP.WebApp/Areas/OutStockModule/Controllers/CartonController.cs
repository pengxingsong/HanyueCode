using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnJie.ERP.ViewModel.OrderModule;

namespace AnJie.ERP.WebApp.Areas.OutStockModule.Controllers
{
    /// <summary>
    /// 包裹控制器
    /// </summary>
    public class CartonController : PublicController<CartonEntity>
    {
        private readonly CartonBLL _cartonBll = new CartonBLL();

        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="startTime">制单开始时间</param>
        /// <param name="endTime">制单结束时间</param>
        /// <param name="MerchantId"></param>
        /// <param name="jqgridparam">分页参数</param>
        /// <param name="WarehouseId"></param>
        /// <returns></returns>
        public ActionResult GetCartonList(string orderNo, string startTime, string endTime, string WarehouseId,
            string MerchantId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<SaleOrderViewModel> listData = _cartonBll.GetCartonList(WarehouseId, MerchantId, orderNo, null,
                    startTime, endTime, jqgridparam);
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
    }
}