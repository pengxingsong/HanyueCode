using AnJie.ERP.Business;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using AnJie.ERP.ViewModel.OrderModule;
using AnJie.ERP.ViewModel.OutStockModule;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Web.Mvc;
using AnJie.ERP.Plugins.Express;
using AnJie.ERP.Plugins.OrderPrint;
using AnJie.ERP.Service;
using System.Text.RegularExpressions;
using System.Linq;
using AnJie.ERP.Plugins.ExpressDocking;

namespace AnJie.ERP.WebApp.Areas.OutStockModule.Controllers
{
    public class PrintController : PublicController<SaleOrderEntity>
    {
        private readonly SaleOrderBLL _orderBll = new SaleOrderBLL();

        private readonly ShipTypeTemplateBLL _templateBLL = new ShipTypeTemplateBLL();

        private readonly ShipTypeBLL _shipTypeBLL = new ShipTypeBLL();

        private readonly MerchantBLL _merchantBLL = new MerchantBLL();

        private readonly WarehouseBLL _warehouseBLL = new WarehouseBLL();

        private readonly PrintBatchBLL _printBatchBLL = new PrintBatchBLL();

        private readonly PrintPlanBLL _printPlanBLL = new PrintPlanBLL();

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
                    queryModel.StatusWithPrint = true;
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
        /// 打印快递单
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintExpressBill()
        {
            return View();
        }

        /// <summary>
        /// 选择打印机视图
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectPrinter()
        {
            return View();
        }


        public ActionResult PrintWayBill()
        {
            return View();
        }

        /// <summary>
        /// 保存批量打印历史
        /// </summary>
        /// <param name="orderNos"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public ActionResult SavePrintBatch(string orderNos, string templateId)
        {
            List<string> hasPrintedOrderNos = _orderBll.CheckOrderPrintStatus(orderNos);
            if (hasPrintedOrderNos != null && hasPrintedOrderNos.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var orderNo in hasPrintedOrderNos)
                {
                    sb.Append(orderNo + ",");
                }
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "[操作失败]以下订单不是待打印状态：" + sb.ToString() }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                var aryOrderNo = orderNos.Split(',');
                PrintBatchEntity batch = new PrintBatchEntity();
                batch.Create();
                batch.PrintTime = DateTime.Now;
                batch.ItemCount = 0;

                foreach (string orderNo in aryOrderNo)
                {
                    if (!string.IsNullOrEmpty(orderNo))
                    {
                        bool flag = _orderBll.UpdateIsPrinting(orderNo, isOpenTrans);
                        if (flag)
                        {
                            PrintBatchItemEntity batchItem = new PrintBatchItemEntity();
                            batchItem.Create();
                            batchItem.BatchId = batch.BatchId;
                            batchItem.OrderNo = orderNo;
                            batch.ItemCount = batch.ItemCount + 1;
                            database.Insert(batchItem, isOpenTrans);
                        }
                        else
                        {
                            throw new Exception(string.Format("订单{0}更新打印状态失败", orderNo));
                        }
                    }
                }
                database.Insert(batch);

                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = "更新成功" }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="templateId"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult GetPrintContent(string orderNo, string templateId, int pageIndex)
        {
            ShipTypeTemplateEntity template = _templateBLL.GetTemplate(templateId);
            var order = _orderBll.Repository().FindEntity("OrderNo", orderNo);
            var orderItems = _orderBll.GetOrderItemList(orderNo);
            if (order != null && orderItems.Count > 0)
            {
                string content = template.TemplateContent;

                var merchant = _merchantBLL.Repository().FindEntity("MerchantId", order.MerchantId);
                var warehouse = _warehouseBLL.Repository().FindEntity("WarehouseId", order.WarehouseId);

                content = content.Replace("发件人姓名", merchant.FullName);
                content = content.Replace("发件人电话", merchant.Phone);
                content = content.Replace("发件人手机号", merchant.Phone);
                content = content.Replace("发件人-省", merchant.Province);
                content = content.Replace("发件人-市", merchant.City);
                content = content.Replace("发件人-区", merchant.County);
                content = content.Replace("发件人地址", warehouse.Address);
                content = content.Replace("发件人邮编", warehouse.PostalCode);

                content = content.Replace("收件人姓名", order.ReceiveContact);
                content = content.Replace("收件人电话", order.ReceivePhone);
                content = content.Replace("收件人手机号", order.ReceiveCellPhone);
                content = content.Replace("收件人-省", order.Province);
                content = content.Replace("收件人-市", order.City);
                content = content.Replace("收件人-区", order.County);
                content = content.Replace("收件人地址", order.ReceiveAddress);
                content = content.Replace("收件人邮编", order.ReceiveZip);

                content = content.Replace("订单编号", order.OrderNo);
                content = content.Replace("派件备注", order.Remark);

                content = content.Replace("店铺名称", merchant.FullName);


                if (order.OrderDate.HasValue)
                {
                    content = content.Replace("发货日期", order.OrderDate.Value.ToString("yyyy-MM-dd"));
                }
                else
                {

                }
                template.TemplateContent = content;

                if (pageIndex > 0)
                {
                    Match match = Regex.Match(template.TemplateContent, @"LODOP\.PRINT_INITA.+?\);\r\n");
                    if (match.Success)
                    {
                        template.TemplateContent = template.TemplateContent.Replace(match.Groups[0].Value, "");
                    }

                    match = Regex.Match(template.TemplateContent, @"LODOP\.SET_PRINT_PAGESIZE.+?\);\r\n");
                    if (match.Success)
                    {
                        template.TemplateContent = template.TemplateContent.Replace(match.Groups[0].Value, "");
                    }
                }
            }
            return Content(template.ToJson());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateExpressNum()
        {
            return View();
        }

        /// <summary>
        /// 计算连续单号
        /// </summary>
        /// <param name="shipTypeId"></param>
        /// <param name="expressNum"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public ActionResult GetFillExpressNum(string shipTypeId, string expressNum, int qty)
        {
            ShipTypeEntity shipTypeEntity = _shipTypeBLL.Repository().FindEntity("ShipTypeId", shipTypeId);
            ExpressService expressService = new ExpressService();
            List<string> lstExpressNum = new List<string>();

            string resultCode = "1";
            string resultMessage = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(expressNum))
                {
                    IExpressStrategy express = expressService.GetExpress(shipTypeEntity.ExpressNumStrategy);
                    bool flag = express.VerifyExpressNum(expressNum);
                    if (flag)
                    {
                        lstExpressNum = express.GetNextExpressNum(expressNum, qty);
                    }
                    else
                    {
                        resultCode = "0";
                        resultMessage = "单号校验失败，请输入正确的单号";
                    }
                }
                else
                {
                    resultCode = "0";
                    resultMessage = "单号不能为空";
                }
            }
            catch (Exception ex)
            {
                resultCode = "0";
                resultMessage = ex.Message;
            }

            var jsonData = new
            {
                code = resultCode,
                message = resultMessage,
                rows = lstExpressNum
            };
            return Content(jsonData.ToJson());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderExpressNum"></param>
        /// <returns></returns>
        public ActionResult SubmitExpressNum(string orderExpressNum)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                List<OrderExpressNumViewModel> expressNumList = orderExpressNum.JonsToList<OrderExpressNumViewModel>();
                foreach (OrderExpressNumViewModel item in expressNumList)
                {
                    if (!string.IsNullOrEmpty(item.OrderNo))
                    {
                        bool flag = _orderBll.UpdateExpressNum(item.OrderNo, item.ExpressNum, false, isOpenTrans);
                        if (!flag)
                        {
                            throw new Exception(string.Format("订单{0}已录入运单号，批量更新失败。", item.OrderNo));
                        }
                    }
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = "更新成功" }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 查询打印历史
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryPrintHistory()
        {
            return View();
        }

        /// <summary>
        /// 查询打印方案
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryPrintPlan()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryPrintBatch()
        {
            string createUserId = ManageProvider.Provider.Current().UserId;
            var list = _printBatchBLL.GetList(createUserId);
            return Content(list.ToJson());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryPrintPlanList()
        {
            var list = _printPlanBLL.GetList();
            return Content(list.ToJson());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryPrintBatchItem(string batchId)
        {
            var list = _printBatchBLL.GetBatchItemList(batchId);
            return Content(list.ToJson());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectOrderPrinter()
        {
            return View();
        }

        public ActionResult ListOrderPrintTemplate()
        {
            PrintService printService = new PrintService();
            List<IOrderPrint> listData = printService.GetAllOrderPrintTemplate();
            return Content(listData.ToJson());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintShipmentBill()
        {
            return View();
        }

        /// <summary>
        /// 获取订单打印内容
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public ActionResult GetOrderPrintContent(string orderNo, string templateId)
        {
            PrintService printService = new PrintService();
            var order = _orderBll.GetOrderViewModel(orderNo);
            var orderItems = _orderBll.GetOrderItemViewModel(orderNo);
            if (order != null && orderItems.Count > 0)
            {

            }
            IOrderPrint template = printService.GetOrderPrintTemplate(templateId);
            string templateContent = "";

            if (template != null)
            {
                templateContent = template.GetTemplateContent(order, orderItems);
            }
            var jsonData = new
            {
                TemplateContent = templateContent
            };
            return Content(jsonData.ToJson());
        }

        public ActionResult GetWayBill(string orderNo, string shipTypeCode)
        {
            SaleOrderBLL saleOrderBLL = new SaleOrderBLL();

            var saleOrder = saleOrderBLL.GetSaleOrder(orderNo);
            var saleOrderItem = saleOrderBLL.GetOrderItemList(orderNo).FirstOrDefault();
            var appId = "1276314";
            var appKey = "319ee1b5-d572-4eb6-b2b9-228076c48eec";
            var url = "http://api.kdniao.cc/api/eorderservice";
            var printServer = ExpressDockingFactory.GetKdNiaoExpressDocking(url);
            KdNiaoRequestData kdnrd = new KdNiaoRequestData();
            kdnrd.AppKey = appKey;
            kdnrd.EBusinessID = appId;
            kdnrd.DataType = "2";
            kdnrd.RequestType = "1007";
            KdNiaoWaybillRequestDataContent kdnrdc = new KdNiaoWaybillRequestDataContent();
           
            kdnrdc.Receiver = new KdNiaoAddress()
            {
                ProvinceName = saleOrder.Province,
                CityName = saleOrder.City,
                ExpAreaName = saleOrder.County,
                Address = saleOrder.ReceiveAddress,
                Name = saleOrder.ReceiveContact,
                Mobile = saleOrder.ReceiveCellPhone,
                PostCode = saleOrder.ReceiveZip
            };
            var sendInfo = new WarehouseBLL().GetWarehouse(saleOrder.WarehouseId);
            var provinceCityBll = new BaseProvinceCityBll();
            kdnrdc.Sender = new KdNiaoAddress()
            {
                ProvinceName = provinceCityBll.GetNameByCode(sendInfo.ProvinceId),
                CityName = provinceCityBll.GetNameByCode(sendInfo.CityId),
                ExpAreaName = provinceCityBll.GetNameByCode(sendInfo.CountyId),
                Address = sendInfo.Address,
                Name = sendInfo.Contact,
                Mobile = sendInfo.Phone,
                PostCode = sendInfo.PostalCode
            };

            kdnrdc.Commodity = new List<KdNiaoCommodity>() {
                   new KdNiaoCommodity()
                   {
                        GoodsName=saleOrderItem.ProductName,
                        GoodsWeight=saleOrderItem.Weight.ToString(),
                        Goodsquantity=saleOrderItem.QtyScaned.ToString(),
                        GoodsVol=saleOrderItem.Volume.ToString()
                   }
            };
            kdnrdc.ShipperCode = shipTypeCode;
            kdnrdc.OrderCode = saleOrder.OrderNo;
            kdnrdc.ExpType = "1";
            kdnrdc.PayType = "1";
            kdnrdc.IsNotice = "1";
            kdnrdc.IsReturnPrintTemplate = "1";
            if (shipTypeCode == "YTO")
            {
                kdnrdc.CustomerName = "k210314881";
                kdnrdc.MonthCode = "f2ar5n8b";
            }
            kdnrd.RequestData = kdnrdc.ToJson();
            kdnrd.RequestDataContent = kdnrdc;
            kdnrd.DataSign = Encrypt.KdNiaoSingEncrypt(kdnrd.RequestData, appKey, "UTF-8");
            var kdnrp = printServer.GetWayBill(kdnrd);
            //if (!kdnrp.Success)
            //{
            //    return Content(kdnrp.ToJson());
            //}
            return Content(kdnrp.PrintTemplate);
        }

    }
}
