using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Service;
using AnJie.ERP.Utilities;

namespace AnJie.ERP.WebApp.Areas.OutStockModule.Controllers
{
    public class OutboundController : Controller
    {
        private readonly SaleOrderBLL _orderBll = new SaleOrderBLL();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 订单明细列表（返回Json）
        /// </summary>
        /// <param name="billNo">订单号</param>
        /// <returns></returns>
        public ActionResult GetOrderItemList(string billNo)
        {
            try
            {
                List<SaleOrderItemEntity> items = billNo.ToLower().StartsWith("so")
                    ? _orderBll.GetOrderItemList(billNo)
                    : _orderBll.GetOrderItemListByExpressNum(billNo);
                var jsonData = new
                {
                    rows = items
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
        /// 
        /// </summary>
        /// <param name="billNo"></param>
        /// <returns></returns>
        public ActionResult ScanBillNo(string billNo)
        {
            try
            {
                var outboundService = new OutboundService();
                string message;
                bool flag = outboundService.ScanBillNo(billNo, out message);
                if (!flag)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = message }.ToString());
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        public ActionResult ScanFinished(string billNo)
        {
            try
            {
                var outboundService = new OutboundService();
                string message;
                bool flag = outboundService.ScanFinished(billNo, out message);
                if (!flag)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = message }.ToString());
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}