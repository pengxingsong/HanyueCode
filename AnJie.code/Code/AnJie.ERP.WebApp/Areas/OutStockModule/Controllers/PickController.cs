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
using AnJie.ERP.ViewModel.OutStockModule;

namespace AnJie.ERP.WebApp.Areas.OutStockModule.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class PickController : PublicController<PickItemEntity>
    {
        private readonly SaleOrderBLL _orderBll = new SaleOrderBLL();

        private readonly PickMasterBLL _pickMasterBll = new PickMasterBLL();

        private readonly PickItemBLL _pickItemBLL = new PickItemBLL();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetOrderList(QueryOrderViewModel queryModel, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<SaleOrderViewModel> listData = null;
                if (queryModel.QueryType == "PrintBatch")
                {
                    if (string.IsNullOrWhiteSpace(queryModel.PrintBatchId))
                    {
                        throw new Exception("打印批次号为空");
                    }
                    listData = _orderBll.GetOrderListByPrintBatch(queryModel.PrintBatchId, jqgridparam);
                }
                else if (!string.IsNullOrWhiteSpace(queryModel.QueryType))
                {
                    queryModel.Status = (int)OrderStatus.WaitPick;
                    listData = _orderBll.GetOrderList(queryModel, jqgridparam);
                }
                else
                {
                    listData = new List<SaleOrderViewModel>();
                }

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
        /// 
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        public ActionResult CreatePickMaster(string orderNos)
        {
            try
            {
                var sb = new StringBuilder();
                var pickService = new PickService();
                string[] aryOrderNo = orderNos.Split(',');

                string message;
                bool flag = pickService.CreatePickMaster(aryOrderNo, out message);
                if (!flag)
                {
                    sb.AppendFormat("{0}<br/>", message);
                }
                else
                {
                    sb.AppendFormat("{0}<br/>", message);
                }

                WriteLog(1, orderNos, sb.ToString());
                return Content(new JsonMessage { Success = true, Code = "1", Message = sb.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, orderNos, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PickList()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pickNo"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="WarehouseId"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetPickMasterList(string pickNo, string startTime, string endTime, string WarehouseId,
            JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<PickMasterViewModel> listData = _pickMasterBll.GetPickMasterList(WarehouseId, pickNo, startTime,
                    endTime, jqgridparam);
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
        /// 拣货单明细
        /// </summary>
        /// <param name="pickNo"></param>
        /// <returns></returns>
        public ActionResult GetPickItemListByPickNo(string pickNo)
        {
            try
            {
                var jsonData = new
                {
                    rows = _pickItemBLL.GetPickItemListByPickNo(pickNo),
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
        /// 获取订单拣货明细
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult GetPickItemListByOrderNo(string orderNo)
        {
            try
            {
                var jsonData = new
                {
                    rows = _pickItemBLL.GetPickItemListByOrderNo(orderNo),
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
        /// 拣货确认
        /// </summary>
        /// <param name="pickNo"></param>
        /// <returns></returns>
        public ActionResult PickConfirm(string pickNo)
        {
            try
            {
                var pickService = new PickService();

                int isOk = 1;
                string message;
                bool flag = pickService.PickConfirm(pickNo, out message);
                if (!flag)
                {
                    isOk = -1;
                }
                WriteLog(isOk, pickNo, message);
                return Content(new JsonMessage { Success = true, Code = isOk.ToString(), Message = message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, pickNo, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintPickBill()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectPrinter()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pickId"></param>
        /// <returns></returns>
        public ActionResult GetPrintContent(string pickId)
        {
            //var itemList = _handoverBll.GetHandOverItemList(handoverId);
            //var handoverEntity = _handoverBll.GetEntity(handoverId);
            //string templateContent = _handoverService.GetPrintTemplateContent(handoverEntity, itemList);


            StringBuilder sb = new StringBuilder();

            sb.Append("LODOP.PRINT_INIT(\"拣货单\");");
            sb.Append("LODOP.ADD_PRINT_RECT(70, 27, 634, 242, 0, 1);");
            sb.Append("LODOP.ADD_PRINT_TEXT(29, 236, 279, 38, \"拣货明细\");");
            sb.Append("LODOP.SET_PRINT_STYLEA(2, \"FontSize\", 18);");
            sb.Append("LODOP.SET_PRINT_STYLEA(2, \"Bold\", 1);");
            //sb.Append(string.Format("LODOP.ADD_PRINT_HTM(88, 40, 321, 185, \"{0}\");", templateContent.Replace("\"","'")));
            sb.Append(string.Format("LODOP.ADD_PRINT_HTM(88, 40, 321, 185, \"{0}\");", "测试拣货模板内容"));
            var jsonData = new
            {
                TemplateContent = sb.ToString()
            };
            return Content(jsonData.ToJson());
        }
    }
}
