using AnJie.ERP.Business;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using AnJie.ERP.ViewModel.PurchaseModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnJie.ERP.WebApp.Areas.PurchaseModule.Controllers
{
    /// <summary>
    /// 采购单主表控制器
    /// </summary>
    public class PurchaseOrderController : PublicController<PurchaseOrderEntity>
    {
        readonly PurchaseOrderBLL orderBLL = new PurchaseOrderBLL();

        readonly BaseCodeRuleBll codeRuleBLL = new BaseCodeRuleBll();

        readonly ProductBLL productBLL = new ProductBLL();

        private static readonly string PurchaseOrderCodeName = "PurchaseOrder";

        /// <summary>
        /// 获取自动单据编码
        /// </summary>
        /// <returns></returns>
        private string CreateOrderCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return codeRuleBLL.GetBillCode(UserId, PurchaseOrderCodeName);
        }

        /// <summary>
        /// 订单表单
        /// </summary>
        /// <returns></returns>
        public override ActionResult Form()
        {
            string KeyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.OrderNo = this.CreateOrderCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }

        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="OrderNo">订单号</param>
        /// <param name="StartTime">制单开始时间</param>
        /// <param name="EndTime">制单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GetOrderList(string OrderNo, string StartTime, string EndTime, string WarehouseId, string MerchantId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<PurchaseOrderViewModel> ListData = orderBLL.GetOrderList(WarehouseId, MerchantId, OrderNo, StartTime, EndTime, jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
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
        /// <param name="OrderId">订单主键</param>
        /// <returns></returns>
        public ActionResult GetOrderItemList(string OrderId)
        {
            try
            {
                var JsonData = new
                {
                    rows = orderBLL.GetOrderItemList(OrderId),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 提交订单表单（新增、编辑）
        /// </summary>
        /// <param name="KeyValue">订单主键</param>
        /// <param name="entity">订单实体</param>
        /// <param name="PurchaseOrderItemJson">订单明细</param>
        /// <returns></returns>
        public ActionResult SubmitOrderForm(string KeyValue, PurchaseOrderEntity entity, string PurchaseOrderItemJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "修改成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    database.Delete<PurchaseOrderEntity>("OrderId", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    entity.OrderDate = DateTime.Now;
                    database.Insert(entity, isOpenTrans);
                    codeRuleBLL.OccupyBillCode(ManageProvider.Provider.Current().UserId, PurchaseOrderCodeName, isOpenTrans);
                }
                List<PurchaseOrderItemViewModel> OrderItemList = PurchaseOrderItemJson.JonsToList<PurchaseOrderItemViewModel>();
                int index = 1;
                foreach (PurchaseOrderItemViewModel item in OrderItemList)
                {
                    if (!string.IsNullOrEmpty(item.ProductId))
                    {
                        ProductEntity product = productBLL.GetProduct(item.ProductId);
                        var orderItem = new PurchaseOrderItemEntity();
                        orderItem.Create();
                        orderItem.OrderId = entity.OrderId;
                        orderItem.ProductId = product.ProductId;
                        orderItem.Code = product.Code;
                        orderItem.ProductName = product.ProductName;
                        orderItem.Volume = product.Volume;
                        orderItem.Weight = product.Weight;
                        orderItem.BarCode = product.BarCode;
                        orderItem.BaseUnit = product.BaseUnit;
                        orderItem.Price = product.Price;
                        orderItem.Specification = product.Specification;
                        orderItem.Qty = int.Parse(item.Qty);
                        database.Insert(orderItem, isOpenTrans);
                        index++;
                    }
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductList()
        {
            return View();
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult Audit(string KeyValue)
        {
            try
            {
                var Message = "审核失败。";
                int IsOk = 0;

                var entity = Repositoryfactory.Repository().FindEntity("OrderId", KeyValue);
                if (entity == null || entity.Status != 0)
                {
                    throw new Exception("该订单不是初始状态，不能审核");
                }

                entity.Modify(KeyValue);
                IsOk = orderBLL.Audit(entity);
                if (IsOk > 0)
                {
                    Message = "审核成功。";
                }
                WriteLog(IsOk, KeyValue, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult Invalid(string KeyValue)
        {
            try
            {
                var Message = "审核失败。";
                int IsOk = 0;

                var entity = Repositoryfactory.Repository().FindEntity("OrderId", KeyValue);
                if (entity == null || entity.Status != 0)
                {
                    throw new Exception("该采购单不是初始状态，不能作废");
                }

                entity.Modify(KeyValue);
                IsOk = orderBLL.Invalid(entity);
                if (IsOk > 0)
                {
                    Message = "作废成功。";
                }
                WriteLog(IsOk, KeyValue, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}