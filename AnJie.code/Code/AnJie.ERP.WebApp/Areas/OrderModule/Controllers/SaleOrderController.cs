using AnJie.ERP.Business;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Service;
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

namespace AnJie.ERP.WebApp.Areas.OrderModule.Controllers
{
    /// <summary>
    /// 订单管理控制器
    /// </summary>
    public class SaleOrderController : PublicController<SaleOrderEntity>
    {
        private readonly SaleOrderBLL _orderBll = new SaleOrderBLL();

        private readonly BaseCodeRuleBll _codeRuleBll = new BaseCodeRuleBll();

        private readonly ProductBLL _productBll = new ProductBLL();

        private readonly OrderService _orderService = new OrderService();

        private static readonly string SaleOrderCodeName = "SaleOrder";

        /// <summary>
        /// 获取自动单据编码
        /// </summary>
        /// <returns></returns>
        private string CreateOrderCode()
        {
            string userId = ManageProvider.Provider.Current().UserId;
            return _codeRuleBll.GetBillCode(userId, SaleOrderCodeName);
        }

        /// <summary>
        /// 订单表单
        /// </summary>
        /// <returns></returns>
        public override ActionResult Form()
        {
            string keyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(keyValue))
            {
                ViewBag.OrderNo = this.CreateOrderCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }

        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <returns></returns>
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
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult GetOrderItemList(string orderId)
        {
            try
            {
                var jsonData = new
                {
                    rows = _orderBll.GetOrderItemListByOrderId(orderId),
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
        /// 提交订单表单（新增、编辑）
        /// </summary>
        /// <param name="orderId">订单主键</param>
        /// <param name="entity">订单实体</param>
        /// <param name="saleOrderItemJson">订单明细</param>
        /// <returns></returns>
        public ActionResult SubmitOrderForm(string orderId, SaleOrderEntity entity, string saleOrderItemJson)
        {
            SaleOrderEntity oldEntity = null;
            IDatabase database = DataFactory.Database();
            if (!string.IsNullOrEmpty(orderId))
            {
                oldEntity = database.FindEntity<SaleOrderEntity>("OrderId", orderId);
            }

            if (oldEntity != null && oldEntity.Status != (int)OrderStatus.Initial && oldEntity.Status != (int)OrderStatus.WaitAudit)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：只有初始和待审核订单才能修改" }.ToString());
            }

            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string message = orderId == "" ? "新增成功。" : "修改成功。";
                if (oldEntity != null)
                {
                    database.Delete<SaleOrderEntity>("OrderId", oldEntity.OrderId, isOpenTrans);
                    database.Delete<SaleOrderItemEntity>("OrderId", oldEntity.OrderId, isOpenTrans);
                    entity.Create();
                    entity.OrderDate = oldEntity.OrderDate;
                    entity.Status = oldEntity.Status;
                    entity.Modify(orderId);
                    database.Insert(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    entity.OrderDate = DateTime.Now;
                    database.Insert(entity, isOpenTrans);
                    _codeRuleBll.OccupyBillCode(ManageProvider.Provider.Current().UserId, SaleOrderCodeName, isOpenTrans);
                    
                }
                List<SaleOrderItemViewModel> orderItemList = saleOrderItemJson.JonsToList<SaleOrderItemViewModel>();
                foreach (SaleOrderItemViewModel item in orderItemList)
                {
                    if (!string.IsNullOrEmpty(item.ProductId))
                    {
                        ProductEntity product = _productBll.GetProduct(item.ProductId);
                        var orderItem = new SaleOrderItemEntity();
                        orderItem.Create();
                        orderItem.OrderId = entity.OrderId;
                        orderItem.OrderNo = entity.OrderNo;
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
                    }
                }
                database.Commit();
                if (oldEntity == null)
                {
                    try
                    {
                        _orderBll.ExecSPOrderAllocInventory();
                    }
                    catch
                    {
                    }
                }
                return Content(new JsonMessage { Success = true, Code = entity.OrderId, Message = message }.ToString());
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
        /// 审核订单
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        public ActionResult Audit(string orderNos)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');
                foreach (var orderNo in aryOrderNo)
                {
                    string errMessage;
                    bool flag = _orderService.Audit(orderNo, out errMessage);
                    if (flag)
                    {
                        sb.AppendFormat("订单{0}审核成功<br>", orderNo);
                    }
                    else
                    {
                        sb.AppendFormat("{0}<br>", errMessage);
                    }

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
        /// 取消审核
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        public ActionResult CancelAudit(string orderNos)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');
                foreach (var orderNo in aryOrderNo)
                {
                    string errMessage;
                    bool flag = _orderService.CancelAudit(orderNo, out errMessage);
                    if (flag)
                    {
                        sb.AppendFormat("订单{0}取消审核成功<br>", orderNo);
                    }
                    else
                    {
                        sb.AppendFormat("{0}<br>", errMessage);
                    }
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
        /// 确认发货
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        public ActionResult ConfirmShip(string orderNos)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');
                foreach (var orderNo in aryOrderNo)
                {
                    string errMessage;
                    bool flag = _orderService.ConfirmShip(orderNo, out errMessage);
                    if (flag)
                    {
                        sb.AppendFormat("订单{0}发货成功<br>", orderNo);
                    }
                    else
                    {
                        sb.AppendFormat("{0}<br>", errMessage);
                    }
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
        /// 取消发货
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        public ActionResult CancelConfirmShip(string orderNos)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');
                foreach (var orderNo in aryOrderNo)
                {
                    string errMessage;
                    bool flag = _orderService.CancelConfirmShip(orderNo, out errMessage);
                    if (flag)
                    {
                        sb.AppendFormat("订单{0}取消发货成功<br>", orderNo);
                    }
                    else
                    {
                        sb.AppendFormat("{0}<br>", errMessage);
                    }
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
        /// 取消拣货
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        public ActionResult CancelCreatePick(string orderNos)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');
                foreach (var orderNo in aryOrderNo)
                {
                    string errMessage;
                    bool flag = _orderService.CancelCreatePick(orderNo, out errMessage);
                    if (flag)
                    {
                        sb.AppendFormat("订单{0}取消拣货成功<br>", orderNo);
                    }
                    else
                    {
                        sb.AppendFormat("{0}<br>", errMessage);
                    }
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
        /// 取消出库
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        public ActionResult CancelOutStock(string orderNos)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');
                foreach (var orderNo in aryOrderNo)
                {
                    string errMessage;
                    bool flag = _orderService.CancelOutStock(orderNo, out errMessage);
                    if (flag)
                    {
                        sb.AppendFormat("订单{0}取消出库成功<br>", orderNo);
                    }
                    else
                    {
                        sb.AppendFormat("{0}<br>", errMessage);
                    }
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
        /// 作废订单
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        public ActionResult Invalid(string orderNos)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');
                foreach (var orderNo in aryOrderNo)
                {
                    string errMessage;
                    bool flag = _orderService.InvalidOrder(orderNo, out errMessage);
                    sb.AppendFormat(flag ? "订单{0}作废成功<br>" : "订单{0}更新状态失败:{1}<br>", orderNo, errMessage);
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
        /// 订单挂起
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        public ActionResult SuspendOrder(string orderNos)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');
                foreach (var orderNo in aryOrderNo)
                {
                    var entity = Repositoryfactory.Repository().FindEntity("OrderNo", orderNo);
                    if (entity == null || entity.Status == (int)OrderStatus.Canceled)
                    {
                        sb.AppendFormat("订单{0}已作废，不能挂起<br>", orderNo);
                        continue;
                    }

                    if (entity.Status == (int)OrderStatus.OutStock)
                    {
                        sb.AppendFormat("订单{0}已出库，不能挂起<br>", orderNo);
                        continue;
                    }

                    if (entity.IsSuspended)
                    {
                        sb.AppendFormat("订单{0}已挂起，不能重复操作<br>", orderNo);
                        continue;
                    }

                    entity.Modify(entity.OrderId);
                    entity.IsSuspended = true;
                    bool flag = _orderBll.SuspendOrder(entity);
                    if (flag)
                    {
                        sb.AppendFormat("订单{0}挂起成功<br>", orderNo);
                    }
                    else
                    {
                        sb.AppendFormat("订单{0}更新失败<br>", orderNo);
                    }
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
        /// 取消挂起
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        public ActionResult CancelSuspend(string orderNos)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');
                foreach (var orderNo in aryOrderNo)
                {
                    var entity = Repositoryfactory.Repository().FindEntity("OrderNo", orderNo);
                    if (entity == null || !entity.IsSuspended)
                    {
                        sb.AppendFormat("订单{0}当前不是挂起状态<br>", orderNo);
                        continue;
                    }

                    entity.Modify(entity.OrderId);
                    entity.IsSuspended = false;
                    bool flag = _orderBll.SuspendOrder(entity);
                    if (flag)
                    {
                        sb.AppendFormat("订单{0}已取消挂起<br>", orderNo);
                    }
                    else
                    {
                        sb.AppendFormat("订单{0}更新失败<br>", orderNo);
                    }
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

        #region 订单导入
        /// <summary>
        /// 订单导入弹出框页面
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ImportOrder()
        {
            return View();
        }

        /// <summary>
        /// 订单导入
        /// </summary>
        /// <returns></returns>
        public ActionResult SubmitImportOrder()
        {
            bool isSuccess = false;//导入状态
            string errMessage = string.Empty;
            var dataResult = new DataTable();//导入错误记录表
            try
            {
                string WarehouseId = Request.Form["WarehouseId"];
                if (string.IsNullOrEmpty(WarehouseId))
                {
                    throw new Exception("上传文件失败：请选择默认出货仓库");
                }
                string MerchantId = HttpContext.Request.Form["MerchantId"];
                if (string.IsNullOrEmpty(MerchantId))
                {
                    throw new Exception("上传文件失败：请选择订单所属商户");
                }
                string MerchantMallId = HttpContext.Request.Form["MerchantMallId"];
                if (string.IsNullOrEmpty(MerchantMallId))
                {
                    throw new Exception("上传文件失败：请选择订单所属店铺");
                }

                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["filePath"];//获取上传的文件
                if (file != null && file.FileName != "")
                {
                    string fullname = file.FileName;
                    var extension = System.IO.Path.GetExtension(fullname);
                    if (extension != null)
                    {
                        string fileType = extension.ToLower();
                        if (fileType == ".xls" || fileType == ".xlsx")
                        {
                            string fileId = Guid.NewGuid().ToString();
                            string filename = fileId + fileType;

                            bool flag = UploadHelper.FileUpload(file, Server.MapPath("~/UploadFile/ImportOrder/"),
                                filename, out errMessage);
                            if (flag)
                            {

                                SaleOrderImportFileEntity importFile = new SaleOrderImportFileEntity();
                                importFile.Create();
                                importFile.FileId = fileId;
                                importFile.WarehouseId = WarehouseId;
                                importFile.MerchantId = MerchantId;
                                importFile.MerchantMallId = MerchantMallId;
                                importFile.FileName = fullname;

                                DataTable dt = ImportExcel.ImportExcelFile(Server.MapPath("~/UploadFile/ImportOrder/") + filename);
                                flag = _orderService.ImportOrder(importFile, dt, out dataResult, out errMessage);
                                if (flag)
                                {
                                    isSuccess = true;
                                }
                            }
                            else
                            {
                                throw new Exception("订单导入失败：" + errMessage);
                            }
                        }
                        else
                        {
                            throw new Exception("订单导入失败：文件格式不正确");
                        }
                    }
                    else
                    {
                        throw new Exception("订单导入失败：文件格式不正确");
                    }
                }
                else
                {
                    throw new Exception("请选择上传文件");
                }
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "异常错误：" + ex.Message);
                errMessage = ex.Message;
                isSuccess = false;
            }

            if (dataResult.Rows.Count > 0)
            {
                isSuccess = false;
            }
            var data = new
            {
                status = isSuccess ? "true" : "false",
                result = dataResult,
                message = errMessage
            };
            return Content(data.ToJson());
        }
        #endregion

        #region 修改物流方式
        /// <summary>
        /// 修改物流方式
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ModifyShipType()
        {
            return View();
        }

        /// <summary>
        /// 修改物流方式
        /// </summary>
        /// <param name="orderNos"></param>
        /// <param name="shipTypeId"></param>
        public ActionResult SubmitModifyShipType(string orderNos, string shipTypeId)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');
                foreach (var orderNo in aryOrderNo)
                {
                    var entity = Repositoryfactory.Repository().FindEntity("OrderNo", orderNo);
                    if (entity == null || entity.PrintStatus != (int)PrintStatus.WaitPrint)
                    {
                        sb.AppendFormat("订单{0}不是待打印状态，不能修改物流方式<br>", orderNo);
                        continue;
                    }

                    if (entity.ShipTypeId != shipTypeId)
                    {
                        entity.Modify(entity.OrderId);
                        entity.ShipTypeId = shipTypeId;
                        bool flag = _orderBll.UpdateShipType(entity);

                        if (flag)
                        {
                            sb.AppendFormat("订单{0}修改物流方式成功<br>", orderNo);
                        }
                        else
                        {
                            sb.AppendFormat("订单{0}修改物流方式失败<br>", orderNo);
                        }
                    }
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
        #endregion

        #region 修改发货仓库
        /// <summary>
        /// 修改发货仓库
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ModifyWarehouse()
        {
            return View();
        }

        /// <summary>
        /// 修改发货仓库
        /// </summary>
        /// <param name="orderNos"></param>
        /// <param name="warehouseId"></param>
        public ActionResult SubmitModifyWarehouse(string orderNos, string warehouseId)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');
                foreach (var orderNo in aryOrderNo)
                {
                    var entity = Repositoryfactory.Repository().FindEntity("OrderNo", orderNo);
                    if (entity == null || entity.Status != (int)OrderStatus.Initial)
                    {
                        sb.AppendFormat("订单{0}不是初始状态，不能修改发货仓库<br>", orderNo);
                        continue;
                    }

                    if (entity.WarehouseId != warehouseId)
                    {
                        entity.Modify(entity.OrderId);
                        entity.WarehouseId = warehouseId;
                        bool flag = _orderBll.UpdateWarehouse(entity, OrderStatus.Initial);

                        if (flag)
                        {
                            sb.AppendFormat("订单{0}修改发货仓库成功<br>", orderNo);
                        }
                        else
                        {
                            sb.AppendFormat("订单{0}修改发货仓库失败<br>", orderNo);
                        }
                    }
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
        #endregion



        /// <summary>
        /// 高级解锁订单
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        public ActionResult UnLock(string orderNos)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');

                foreach (var orderNo in aryOrderNo)
                {
                    //var entity = Repositoryfactory.Repository().FindEntity("OrderNo", orderNo);
                    //if (string.IsNullOrWhiteSpace(entity.LockUserId))
                    //{
                    //    sb.AppendFormat("<font color='blue'>订单{0}不需要解锁</font><br>", orderNo);
                    //}
                    //else
                    //{
                    bool flag = _orderBll.UpdateUnLockUserIdByOrderNo(orderNo, null);
                    if (flag)
                    {
                        sb.AppendFormat("订单{0}解锁成功<br>", orderNo);
                    }
                    else
                    {
                        sb.AppendFormat("订单{0}解锁失败<br>", orderNo);
                    }
                    //}
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
        /// 解锁订单
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        public ActionResult UnLockByUserId(string orderNos)
        {
            try
            {
                var userId = ManageProvider.Provider.Current().UserId;
                var sb = new StringBuilder();
                string[] aryOrderNo = orderNos.Split(',');

                foreach (var orderNo in aryOrderNo)
                {
                    var entity = Repositoryfactory.Repository().FindEntity("OrderNo", orderNo);
                    if (string.IsNullOrWhiteSpace(entity.LockUserId))
                    {
                        sb.AppendFormat("<font color='blue'>订单{0}不需要解锁</font><br>", orderNo);
                    }
                    else
                    {
                        if (entity.LockUserId != userId)
                        {
                            sb.AppendFormat("<font color='red'>订单{0}无权限解锁,锁单人是{1}</font><br>", orderNo, entity.LockUserName);
                        }
                        else
                        {
                            bool flag = _orderBll.UpdateUnLockUserIdByOrderNo(orderNo, null);
                            if (flag)
                            {
                                sb.AppendFormat("订单{0}解锁成功<br>", orderNo);
                            }
                            else
                            {
                                sb.AppendFormat("订单{0}解锁失败<br>", orderNo);
                            }
                        }
                    }
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
        /// 订单表单
        /// </summary>
        /// <returns></returns>
        public ActionResult SplitOrder()
        {
            string keyValue = Request["KeyValue"];
            ViewBag.SplitOrderNo = this.CreateOrderCode();
            return View();
        }
        public ActionResult SubmitSplitOrder(string splitOrderNo, SaleOrderEntity entity, string saleOrderItemJson)
        {
            SaleOrderEntity oldEntity = null;
            IDatabase database = DataFactory.Database();

            try
            {
                oldEntity = database.FindEntity<SaleOrderEntity>("OrderId", entity.OrderId);

                if (oldEntity != null && oldEntity.Status != (int)OrderStatus.OutOfStock && oldEntity.Status != (int)OrderStatus.Initial && oldEntity.Status != (int)OrderStatus.WaitAudit)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：只有初始或缺货或待审核订单才能拆分订单" }.ToString());
                }

                List<SplitSaleOrderItemViewModel> splitItemList = saleOrderItemJson.JonsToList<SplitSaleOrderItemViewModel>();
                var orderItemList = _orderBll.GetOrderItemListByOrderId(oldEntity.OrderId);
                if (splitItemList.Sum(i => int.Parse(i.SplitQty)) == orderItemList.Sum(i => i.Qty))
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：不能将所有商品都拆分出去" }.ToString());
                }
                foreach (var item in splitItemList)
                {
                    var whereItem = orderItemList.Where(i => i.ItemId == item.ItemId);
                    if (whereItem.Count() > 0)
                    {
                        if (int.Parse(item.SplitQty) > whereItem.First().Qty)
                        {
                            return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("操作失败：商品[{0}]拆分数据数量{1}大于可拆分数量{2}", whereItem.First().ProductId, item.SplitQty, whereItem.First().Qty) }.ToString());
                        }
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：商品明细数据不存在，请刷新重试" }.ToString());
                    }
                }

                DbTransaction isOpenTrans = database.BeginTrans();
                try
                {
                    string message = "拆分订单成功。";

                    entity.Create();
                    entity.OrderNo = splitOrderNo;
                    entity.OrderDate = DateTime.Now;
                    database.Insert(entity, isOpenTrans);
                    _codeRuleBll.OccupyBillCode(ManageProvider.Provider.Current().UserId, SaleOrderCodeName, isOpenTrans);

                    foreach (var item in splitItemList)
                    {
                        var oldItem = orderItemList.Where(i => i.ItemId == item.ItemId).First();
                        var orderItem = new SaleOrderItemEntity();
                        orderItem.Create();
                        orderItem.OrderId = entity.OrderId;
                        orderItem.OrderNo = entity.OrderNo;
                        orderItem.ProductId = oldItem.ProductId;
                        orderItem.Code = oldItem.Code;
                        orderItem.ProductName = oldItem.ProductName;
                        orderItem.Volume = oldItem.Volume;
                        orderItem.Weight = oldItem.Weight;
                        orderItem.BarCode = oldItem.BarCode;
                        orderItem.BaseUnit = oldItem.BaseUnit;
                        orderItem.Price = oldItem.Price;
                        orderItem.Specification = oldItem.Specification;
                        orderItem.Qty = int.Parse(item.SplitQty);
                        database.Insert(orderItem, isOpenTrans);

                        database.Delete<SaleOrderItemEntity>("ItemId", oldItem.ItemId, isOpenTrans);
                        if (int.Parse(item.SplitQty) != oldItem.Qty)
                        {
                            var oldOrderItem = new SaleOrderItemEntity();
                            oldOrderItem.Create();
                            oldOrderItem.OrderId = oldEntity.OrderId;
                            oldOrderItem.OrderNo = oldEntity.OrderNo;
                            oldOrderItem.ProductId = oldItem.ProductId;
                            oldOrderItem.Code = oldItem.Code;
                            oldOrderItem.ProductName = oldItem.ProductName;
                            oldOrderItem.Volume = oldItem.Volume;
                            oldOrderItem.Weight = oldItem.Weight;
                            oldOrderItem.BarCode = oldItem.BarCode;
                            oldOrderItem.BaseUnit = oldItem.BaseUnit;
                            oldOrderItem.Price = oldItem.Price;
                            oldOrderItem.Specification = oldItem.Specification;
                            oldOrderItem.Qty = oldItem.Qty - int.Parse(item.SplitQty);
                            database.Insert(oldOrderItem, isOpenTrans);
                        }
                    }

                    database.Commit();
                    return Content(new JsonMessage { Success = true, Code = "1", Message = message }.ToString());
                }
                catch (Exception ex)
                {
                    database.Rollback();
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        public ActionResult MergeOrder()
        {
            string keyValue = Request["KeyValue"];
            ViewBag.MergeOrderNo = this.CreateOrderCode();
            return View();
        }
        public ActionResult CheckMergeOrderNo(string orderIds)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryOrderId = orderIds.Split(',');
                List<SaleOrderEntity> orderList = new List<SaleOrderEntity>();

                foreach (var orderId in aryOrderId)
                {
                    var entity = Repositoryfactory.Repository().FindEntity("OrderId", orderId);

                    if (entity != null && entity.Status != (int)OrderStatus.OutOfStock
                        && entity.Status != (int)OrderStatus.Initial
                        && entity.Status != (int)OrderStatus.WaitAudit)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：只有初始或缺货或待审核订单才能合并订单" }.ToString());
                    }
                    orderList.Add(entity);
                }

                //判断是否一致，不一致则不通过
                var group = orderList.GroupBy(i => new { Province = i.Province, City = i.City, County = i.County, ReceiveAddress = i.ReceiveAddress, ReceiveContact = i.ReceiveContact, ReceiveCellPhone = i.ReceiveCellPhone, ReceivePhone = i.ReceivePhone });
                if (group.Count() > 1)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：不符合订单合并条件( 省/市/区/地址/手机/电话/联系人相同)" }.ToString());
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
        public ActionResult SetMergeOrder(string KeyValue)
        {
            try
            {
                string[] aryOrderId = KeyValue.Split(',');
                List<string> sourceOrderNos = new List<string>();
                SaleOrderEntity entity = null;
                foreach (var orderId in aryOrderId)
                {
                    if (entity == null)
                    {
                        entity = Repositoryfactory.Repository().FindEntity("OrderId", orderId);
                        sourceOrderNos = entity.SourceOrderNo.Split(',').ToList();                    }
                    else
                    {
                        var otherentity = Repositoryfactory.Repository().FindEntity("OrderId", orderId);
                        entity.OrderNo += "," + otherentity.OrderNo;
                        var otherSourceOrderNos = otherentity.SourceOrderNo.Split(',').ToList();
                        foreach (var item in otherSourceOrderNos)
                        {
                            if (!sourceOrderNos.Contains(item))
                            {
                                sourceOrderNos.Add(item);
                            }
                        }
                    }
                }
                entity.SourceOrderNo = String.Join(",", sourceOrderNos.ToArray());
                return Content(entity.ToJson());
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        public ActionResult SetMergeOrderItemList(string KeyValue)
        {
            try
            {
                List<SaleOrderItemEntity> itemList = new List<SaleOrderItemEntity>();
                string[] aryOrderId = KeyValue.Split(',');
                foreach (var orderId in aryOrderId)
                {
                    var items = _orderBll.GetOrderItemListByOrderId(orderId);
                    foreach (var item in items)
                    {
                        itemList.Add(item);
                    }
                }
                return Content(itemList.ToJson());
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        public ActionResult SubmitMergeOrder(string orderIds, string mergeOrderNo, SaleOrderEntity entity)
        {
            IDatabase database = DataFactory.Database();
            try
            {
                string[] aryOrderId = orderIds.Split(',');
                var sourceOrderNo = string.Empty;
                List<SaleOrderEntity> orderList = new List<SaleOrderEntity>();
                List<SaleOrderItemEntity> itemList = new List<SaleOrderItemEntity>();
                foreach (var orderId in aryOrderId)
                {
                    SaleOrderEntity oldEntity = database.FindEntity<SaleOrderEntity>("OrderId", orderId);

                    if (oldEntity != null && oldEntity.Status != (int)OrderStatus.OutOfStock && oldEntity.Status != (int)OrderStatus.Initial && oldEntity.Status != (int)OrderStatus.WaitAudit)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：只有初始或缺货或待审核订单才能合并订单" }.ToString());
                    }
                    orderList.Add(entity);
                    var orderItemList = _orderBll.GetOrderItemListByOrderId(oldEntity.OrderId);
                    foreach (var item in orderItemList)
                    {
                        itemList.Add(item);
                    }
                }

                //判断是否一致，不一致则不通过
                var group = orderList.GroupBy(i => new { Province = i.Province, City = i.City, County = i.County, ReceiveAddress = i.ReceiveAddress, ReceiveContact = i.ReceiveContact, ReceiveCellPhone = i.ReceiveCellPhone, ReceivePhone = i.ReceivePhone });
                if (group.Count() > 1)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：不符合订单合并条件( 省/市/区/地址/手机/电话/联系人相同)" }.ToString());
                }
                DbTransaction isOpenTrans = database.BeginTrans();
                try
                {
                    string message = "合并订单成功。";

                    entity.Create();
                    entity.OrderNo = mergeOrderNo;
                    entity.OrderDate = DateTime.Now;
                    database.Insert(entity, isOpenTrans);
                    _codeRuleBll.OccupyBillCode(ManageProvider.Provider.Current().UserId, SaleOrderCodeName, isOpenTrans);

                    foreach (var item in itemList)
                    {
                        var orderItem = new SaleOrderItemEntity();
                        orderItem.Create();
                        orderItem.OrderId = entity.OrderId;
                        orderItem.OrderNo = entity.OrderNo;
                        orderItem.ProductId = item.ProductId;
                        orderItem.Code = item.Code;
                        orderItem.ProductName = item.ProductName;
                        orderItem.Volume = item.Volume;
                        orderItem.Weight = item.Weight;
                        orderItem.BarCode = item.BarCode;
                        orderItem.BaseUnit = item.BaseUnit;
                        orderItem.Price = item.Price;
                        orderItem.Specification = item.Specification;
                        orderItem.Qty = item.Qty; 
                        database.Insert(orderItem, isOpenTrans);
                    }

                    foreach (var orderId in aryOrderId)
                    {
                        database.Delete<SaleOrderEntity>("OrderId", orderId, isOpenTrans);
                        database.Delete<SaleOrderItemEntity>("OrderId", orderId, isOpenTrans);
                    }

                    database.Commit();
                    return Content(new JsonMessage { Success = true, Code = "1", Message = message }.ToString());
                }
                catch (Exception ex)
                {
                    database.Rollback();
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}