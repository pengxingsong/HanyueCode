using AnJie.ERP.Business;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using AnJie.ERP.ViewModel.InStockModule;
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
using AnJie.ERP.Business.InStockModule;
using AnJie.ERP.Service;

namespace AnJie.ERP.WebApp.Areas.InStockModule.Controllers
{
    /// <summary>
    /// 收货单主表控制器
    /// </summary>
    public class ReceiptController : PublicController<ReceiptEntity>
    {
        private readonly ReceiptBLL _receiptBll = new ReceiptBLL();

        private readonly BaseCodeRuleBll _codeRuleBll = new BaseCodeRuleBll();

        private readonly ProductBLL _productBll = new ProductBLL();

        private readonly ReceiptService _receiptService = new ReceiptService();

        private static readonly string ReceiptCodeName = "Receipt";

        /// <summary>
        /// 获取自动单据编码
        /// </summary>
        /// <returns></returns>
        private string CreateReceiptCode()
        {
            string userId = ManageProvider.Provider.Current().UserId;
            return _codeRuleBll.GetBillCode(userId, ReceiptCodeName);
        }

        /// <summary>
        /// 收货单表单
        /// </summary>
        /// <returns></returns>
        public override ActionResult Form()
        {
            string keyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(keyValue))
            {
                ViewBag.ReceiptNo = this.CreateReceiptCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }

        /// <summary>
        /// 收货单列表（返回Json）
        /// </summary>
        /// <param name="receiptNo">收货单号</param>
        /// <param name="startTime">制单开始时间</param>
        /// <param name="endTime">制单结束时间</param>
        /// <param name="merchantId"></param>
        /// <param name="jqgridparam">分页参数</param>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public ActionResult GetReceiptList(string receiptNo, string startTime, string endTime, string warehouseId,
            string merchantId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<ReceiptViewModel> listData = _receiptBll.GetReceiptList(warehouseId, merchantId, receiptNo,
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

        /// <summary>
        /// 收货单明细列表（返回Json）
        /// </summary>
        /// <param name="receiptId">收货单主键</param>
        /// <returns></returns>
        public ActionResult GetReceiptItemList(string receiptId)
        {
            try
            {
                var jsonData = new
                {
                    rows = _receiptBll.GetReceiptItemList(receiptId),
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
        /// 提交收货单表单（新增、编辑）
        /// </summary>
        /// <param name="keyValue">收货单主键</param>
        /// <param name="entity">收货单实体</param>
        /// <param name="receiptItemJson">收货单明细</param>
        /// <returns></returns>
        public ActionResult SubmitReceiptForm(string keyValue, ReceiptEntity entity, string receiptItemJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string message = keyValue == "" ? "新增成功。" : "修改成功。";
                if (!string.IsNullOrEmpty(keyValue))
                {
                    var receipt = database.FindEntity<ReceiptEntity>("ReceiptId", keyValue);
                    if (receipt.Status != 0)
                    {
                        throw new Exception("非初始状态的收货单不能修改");
                    }
                    database.Delete<ReceiptEntity>("ReceiptId", keyValue, isOpenTrans);
                    database.Delete<ReceiptItemEntity>("ReceiptId", keyValue, isOpenTrans);
                    entity.Create();
                    entity.ReceiptDate = DateTime.Now;
                    entity.Modify(keyValue);
                    database.Insert(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    entity.ReceiptDate = DateTime.Now;
                    database.Insert(entity, isOpenTrans);
                    _codeRuleBll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ReceiptCodeName, isOpenTrans);
                }

                var receiptItemList = receiptItemJson.JonsToList<ReceiptItemViewModel>();
                foreach (var item in receiptItemList)
                {
                    if (!string.IsNullOrEmpty(item.ProductId))
                    {
                        ProductEntity product = _productBll.GetProduct(item.ProductId);
                        var orderItem = new ReceiptItemEntity();
                        orderItem.Create();
                        orderItem.ReceiptId = entity.ReceiptId;
                        orderItem.ProductId = product.ProductId;
                        orderItem.SourceNo = item.SourceNo;
                        orderItem.Code = product.Code;
                        orderItem.ProductName = product.ProductName;
                        orderItem.Qty = item.Qty;
                        database.Insert(orderItem, isOpenTrans);
                    }
                }
                database.Commit();
                return Content(new JsonMessage {Success = true, Code = "1", Message = message}.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "操作失败：" + ex.Message}.ToString());
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
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Audit(string keyValue)
        {
            try
            {
                var Message = "审核失败。";
                int IsOk = 0;

                var entity = Repositoryfactory.Repository().FindEntity("ReceiptId", keyValue);
                if (entity == null || entity.Status != 0)
                {
                    throw new Exception("该收货单不是初始状态，不能审核");
                }

                if (entity.IsLocked)
                {
                    throw new Exception("该收货单已锁定，不能审核");
                }

                entity.Modify(keyValue);
                IsOk = _receiptBll.Audit(entity);
                if (IsOk > 0)
                {
                    Message = "审核成功。";
                }
                WriteLog(IsOk, keyValue, Message);
                return Content(new JsonMessage {Success = true, Code = IsOk.ToString(), Message = Message}.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, keyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "操作失败：" + ex.Message}.ToString());
            }
        }

        /// <summary>
        /// 取消审核
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult CancelAudit(string keyValue)
        {
            try
            {
                var Message = "取消审核失败。";
                int IsOk = 0;

                var entity = Repositoryfactory.Repository().FindEntity("ReceiptId", keyValue);
                if (entity == null || entity.Status != 1)
                {
                    throw new Exception("该收货单不是已审核状态，不能取消审核");
                }
                if (entity.IsLocked)
                {
                    throw new Exception("该收货单已锁定，不能取消");
                }

                entity.Modify(keyValue);
                IsOk = _receiptBll.CancelAudit(entity);
                if (IsOk > 0)
                {
                    Message = "取消审核成功。";
                }
                WriteLog(IsOk, keyValue, Message);
                return Content(new JsonMessage {Success = true, Code = IsOk.ToString(), Message = Message}.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, keyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "操作失败：" + ex.Message}.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Invalid(string keyValue)
        {
            try
            {
                var Message = "作废失败。";
                int IsOk = 0;

                var entity = Repositoryfactory.Repository().FindEntity("ReceiptId", keyValue);
                if (entity == null || entity.Status != 0)
                {
                    throw new Exception("该收货单不是初始状态，不能作废");
                }

                if(entity.IsLocked)
                {
                    throw new Exception("该收货单已锁定，不能作废");
                }

                entity.Modify(keyValue);
                IsOk = _receiptBll.Invalid(entity);
                if (IsOk > 0)
                {
                    Message = "作废成功。";
                }
                WriteLog(IsOk, keyValue, Message);
                return Content(new JsonMessage {Success = true, Code = IsOk.ToString(), Message = Message}.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, keyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "操作失败：" + ex.Message}.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiptNo"></param>
        /// <returns></returns>
        public ActionResult FinishedReceipt(string receiptNo)
        {
            try
            {
                var Message = "审核失败。";
                int IsOk = 0;

                var entity = Repositoryfactory.Repository().FindEntity("ReceiptNo", receiptNo);
                if (entity == null || entity.Status != 2)
                {
                    throw new Exception("该收货单不是收货中状态，不能完成收货");
                }
                if (entity.IsLocked)
                {
                    throw new Exception("该收货单已锁定，完成收货");
                }

                entity.Modify(entity.ReceiptId);
                entity.Status = 3;
                IsOk = _receiptBll.UpdateReceiptStatus(entity);
                if (IsOk > 0)
                {
                    Message = "审核成功。";
                }
                WriteLog(IsOk, receiptNo, Message);
                return Content(new JsonMessage {Success = true, Code = IsOk.ToString(), Message = Message}.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, receiptNo, "操作失败：" + ex.Message);
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "操作失败：" + ex.Message}.ToString());
            }
        }

        /// <summary>
        /// 收货单收货
        /// </summary>
        /// <returns></returns>
        public ActionResult ReceiptReceive()
        {
            return View();
        }

        /// <summary>
        /// 输入收货单号
        /// </summary>
        /// <returns></returns>
        public ActionResult InputReceiptNo()
        {
            return View();
        }

        public ActionResult GetReceipt(string receiptNo)
        {
            ReceiptViewModel entity = _receiptBll.GetReceiptByReceiptNo(receiptNo);
            if (entity == null)
            {
                return Content(new ReceiptViewModel().ToJson());
            }
            return Content(entity.ToJson());
        }
        
        /// <summary>
        /// 锁定收货单
        /// </summary>
        /// <param name="receiptIds"></param>
        /// <returns></returns>
        public ActionResult LockReceipt(string receiptIds)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryReceipt = receiptIds.Split(',');
                foreach (var receiptId in aryReceipt)
                {
                    var entity = Repositoryfactory.Repository().FindEntity("ReceiptId", receiptId);
                    if (entity == null || entity.Status == -1)
                    {
                        sb.AppendFormat("收货单{0}已作废，不能锁定<br>", receiptId);
                        continue;
                    }

                    if (entity.IsLocked)
                    {
                        sb.AppendFormat("收货单{0}已锁定，不能重复操作<br>", entity.ReceiptNo);
                        continue;
                    }

                    entity.Modify(entity.ReceiptId);
                    entity.IsLocked = true;
                    bool flag = _receiptBll.UpdateLockedStatus(entity);
                    if (flag)
                    {
                        sb.AppendFormat("收货单{0}锁定成功<br>", entity.ReceiptNo);
                    }
                    else
                    {
                        sb.AppendFormat("收货单{0}更新锁定状态失败<br>", entity.ReceiptNo);
                    }
                }

                WriteLog(1, receiptIds, sb.ToString());
                return Content(new JsonMessage {Success = true, Code = "1", Message = sb.ToString()}.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, receiptIds, "操作失败：" + ex.Message);
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "操作失败：" + ex.Message}.ToString());
            }
        }

        /// <summary>
        /// 取消挂起
        /// </summary>
        /// <param name="receiptIds"></param>
        /// <returns></returns>
        public ActionResult UnLockReceipt(string receiptIds)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryReceiptId = receiptIds.Split(',');
                foreach (var receiptId in aryReceiptId)
                {
                    var entity = Repositoryfactory.Repository().FindEntity("ReceiptId", receiptId);
                    if (entity == null || !entity.IsLocked)
                    {
                        sb.AppendFormat("收货单{0}当前不是锁定状态<br>", receiptId);
                        continue;
                    }

                    entity.Modify(entity.ReceiptId);
                    entity.IsLocked = false;
                    bool flag = _receiptBll.UpdateLockedStatus(entity);
                    if (flag)
                    {
                        sb.AppendFormat("收货单{0}已取消锁定<br>", entity.ReceiptNo);
                    }
                    else
                    {
                        sb.AppendFormat("收货单{0}更新失败<br>", entity.ReceiptNo);
                    }
                }

                WriteLog(1, receiptIds, sb.ToString());
                return Content(new JsonMessage {Success = true, Code = "1", Message = sb.ToString()}.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, receiptIds, "操作失败：" + ex.Message);
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "操作失败：" + ex.Message}.ToString());
            }
        }

        /// <summary>
        /// 收货单收货
        /// </summary>
        /// <param name="receiptIds"></param>
        /// <returns></returns>
        public ActionResult ReceiptQuickReceive(string receiptIds)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryReceipt = receiptIds.Split(',');
                foreach (var receiptId in aryReceipt)
                {
                    var entity = Repositoryfactory.Repository().FindEntity("ReceiptId", receiptId);
                    if (entity == null)
                    {
                        sb.AppendFormat("收货单{0}不存在<br>", receiptId);
                        continue;
                    }

                    if (entity.Status != (int)ReceiptStatus.Audited)
                    {
                        sb.AppendFormat("收货单{0}不是已审核状态，不能收货<br>", entity.ReceiptNo);
                        continue;
                    }

                    if (entity.IsLocked)
                    {
                        sb.AppendFormat("收货单{0}已锁定，不能收货<br>", entity.ReceiptNo);
                        continue;
                    }

                    bool flag = _receiptService.ReceiptQuickReceive(entity);
                    if (flag)
                    {
                        sb.AppendFormat("收货单{0}收货成功<br>", entity.ReceiptNo);
                    }
                    else
                    {
                        sb.AppendFormat("收货单{0}收货失败<br>", entity.ReceiptNo);
                    }
                }

                WriteLog(1, receiptIds, sb.ToString());
                return Content(new JsonMessage { Success = true, Code = "1", Message = sb.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, receiptIds, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 取消收货
        /// </summary>
        /// <param name="receiptIds"></param>
        /// <returns></returns>
        public ActionResult UnReceiptReceive(string receiptIds)
        {
            try
            {
                var sb = new StringBuilder();
                string[] aryReceiptId = receiptIds.Split(',');
                foreach (var receiptId in aryReceiptId)
                {
                    var entity = Repositoryfactory.Repository().FindEntity("ReceiptId", receiptId);
                    if (entity == null)
                    {
                        sb.AppendFormat("收货单{0}不存在<br>", receiptId);
                        continue;
                    }

                    if (entity.Status != (int)ReceiptStatus.Received)
                    {
                        sb.AppendFormat("收货单{0}不是已收货状态，不能取消收货<br>", entity.ReceiptNo);
                        continue;
                    }

                    if (entity.IsLocked)
                    {
                        sb.AppendFormat("收货单{0}已锁定，不能取消收货<br>", entity.ReceiptNo);
                        continue;
                    }

                    bool flag = _receiptService.UnReceiptReceive(entity);
                    if (flag)
                    {
                        sb.AppendFormat("收货单{0}取消收货成功<br>", entity.ReceiptNo);
                    }
                    else
                    {
                        sb.AppendFormat("收货单{0}取消收货失败<br>", entity.ReceiptNo);
                    }
                }

                WriteLog(1, receiptIds, sb.ToString());
                return Content(new JsonMessage { Success = true, Code = "1", Message = sb.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, receiptIds, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}