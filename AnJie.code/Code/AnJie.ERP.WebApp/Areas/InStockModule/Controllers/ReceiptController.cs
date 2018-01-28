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
    /// �ջ������������
    /// </summary>
    public class ReceiptController : PublicController<ReceiptEntity>
    {
        private readonly ReceiptBLL _receiptBll = new ReceiptBLL();

        private readonly BaseCodeRuleBll _codeRuleBll = new BaseCodeRuleBll();

        private readonly ProductBLL _productBll = new ProductBLL();

        private readonly ReceiptService _receiptService = new ReceiptService();

        private static readonly string ReceiptCodeName = "Receipt";

        /// <summary>
        /// ��ȡ�Զ����ݱ���
        /// </summary>
        /// <returns></returns>
        private string CreateReceiptCode()
        {
            string userId = ManageProvider.Provider.Current().UserId;
            return _codeRuleBll.GetBillCode(userId, ReceiptCodeName);
        }

        /// <summary>
        /// �ջ�����
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
        /// �ջ����б�����Json��
        /// </summary>
        /// <param name="receiptNo">�ջ�����</param>
        /// <param name="startTime">�Ƶ���ʼʱ��</param>
        /// <param name="endTime">�Ƶ�����ʱ��</param>
        /// <param name="merchantId"></param>
        /// <param name="jqgridparam">��ҳ����</param>
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
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// �ջ�����ϸ�б�����Json��
        /// </summary>
        /// <param name="receiptId">�ջ�������</param>
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
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// �ύ�ջ��������������༭��
        /// </summary>
        /// <param name="keyValue">�ջ�������</param>
        /// <param name="entity">�ջ���ʵ��</param>
        /// <param name="receiptItemJson">�ջ�����ϸ</param>
        /// <returns></returns>
        public ActionResult SubmitReceiptForm(string keyValue, ReceiptEntity entity, string receiptItemJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string message = keyValue == "" ? "�����ɹ���" : "�޸ĳɹ���";
                if (!string.IsNullOrEmpty(keyValue))
                {
                    var receipt = database.FindEntity<ReceiptEntity>("ReceiptId", keyValue);
                    if (receipt.Status != 0)
                    {
                        throw new Exception("�ǳ�ʼ״̬���ջ��������޸�");
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
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message}.ToString());
            }
        }

        /// <summary>
        /// ��Ʒ�б�
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductList()
        {
            return View();
        }

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Audit(string keyValue)
        {
            try
            {
                var Message = "���ʧ�ܡ�";
                int IsOk = 0;

                var entity = Repositoryfactory.Repository().FindEntity("ReceiptId", keyValue);
                if (entity == null || entity.Status != 0)
                {
                    throw new Exception("���ջ������ǳ�ʼ״̬���������");
                }

                if (entity.IsLocked)
                {
                    throw new Exception("���ջ������������������");
                }

                entity.Modify(keyValue);
                IsOk = _receiptBll.Audit(entity);
                if (IsOk > 0)
                {
                    Message = "��˳ɹ���";
                }
                WriteLog(IsOk, keyValue, Message);
                return Content(new JsonMessage {Success = true, Code = IsOk.ToString(), Message = Message}.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, keyValue, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message}.ToString());
            }
        }

        /// <summary>
        /// ȡ�����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult CancelAudit(string keyValue)
        {
            try
            {
                var Message = "ȡ�����ʧ�ܡ�";
                int IsOk = 0;

                var entity = Repositoryfactory.Repository().FindEntity("ReceiptId", keyValue);
                if (entity == null || entity.Status != 1)
                {
                    throw new Exception("���ջ������������״̬������ȡ�����");
                }
                if (entity.IsLocked)
                {
                    throw new Exception("���ջ���������������ȡ��");
                }

                entity.Modify(keyValue);
                IsOk = _receiptBll.CancelAudit(entity);
                if (IsOk > 0)
                {
                    Message = "ȡ����˳ɹ���";
                }
                WriteLog(IsOk, keyValue, Message);
                return Content(new JsonMessage {Success = true, Code = IsOk.ToString(), Message = Message}.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, keyValue, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message}.ToString());
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
                var Message = "����ʧ�ܡ�";
                int IsOk = 0;

                var entity = Repositoryfactory.Repository().FindEntity("ReceiptId", keyValue);
                if (entity == null || entity.Status != 0)
                {
                    throw new Exception("���ջ������ǳ�ʼ״̬����������");
                }

                if(entity.IsLocked)
                {
                    throw new Exception("���ջ�������������������");
                }

                entity.Modify(keyValue);
                IsOk = _receiptBll.Invalid(entity);
                if (IsOk > 0)
                {
                    Message = "���ϳɹ���";
                }
                WriteLog(IsOk, keyValue, Message);
                return Content(new JsonMessage {Success = true, Code = IsOk.ToString(), Message = Message}.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, keyValue, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message}.ToString());
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
                var Message = "���ʧ�ܡ�";
                int IsOk = 0;

                var entity = Repositoryfactory.Repository().FindEntity("ReceiptNo", receiptNo);
                if (entity == null || entity.Status != 2)
                {
                    throw new Exception("���ջ��������ջ���״̬����������ջ�");
                }
                if (entity.IsLocked)
                {
                    throw new Exception("���ջ���������������ջ�");
                }

                entity.Modify(entity.ReceiptId);
                entity.Status = 3;
                IsOk = _receiptBll.UpdateReceiptStatus(entity);
                if (IsOk > 0)
                {
                    Message = "��˳ɹ���";
                }
                WriteLog(IsOk, receiptNo, Message);
                return Content(new JsonMessage {Success = true, Code = IsOk.ToString(), Message = Message}.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, receiptNo, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message}.ToString());
            }
        }

        /// <summary>
        /// �ջ����ջ�
        /// </summary>
        /// <returns></returns>
        public ActionResult ReceiptReceive()
        {
            return View();
        }

        /// <summary>
        /// �����ջ�����
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
        /// �����ջ���
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
                        sb.AppendFormat("�ջ���{0}�����ϣ���������<br>", receiptId);
                        continue;
                    }

                    if (entity.IsLocked)
                    {
                        sb.AppendFormat("�ջ���{0}�������������ظ�����<br>", entity.ReceiptNo);
                        continue;
                    }

                    entity.Modify(entity.ReceiptId);
                    entity.IsLocked = true;
                    bool flag = _receiptBll.UpdateLockedStatus(entity);
                    if (flag)
                    {
                        sb.AppendFormat("�ջ���{0}�����ɹ�<br>", entity.ReceiptNo);
                    }
                    else
                    {
                        sb.AppendFormat("�ջ���{0}��������״̬ʧ��<br>", entity.ReceiptNo);
                    }
                }

                WriteLog(1, receiptIds, sb.ToString());
                return Content(new JsonMessage {Success = true, Code = "1", Message = sb.ToString()}.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, receiptIds, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message}.ToString());
            }
        }

        /// <summary>
        /// ȡ������
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
                        sb.AppendFormat("�ջ���{0}��ǰ��������״̬<br>", receiptId);
                        continue;
                    }

                    entity.Modify(entity.ReceiptId);
                    entity.IsLocked = false;
                    bool flag = _receiptBll.UpdateLockedStatus(entity);
                    if (flag)
                    {
                        sb.AppendFormat("�ջ���{0}��ȡ������<br>", entity.ReceiptNo);
                    }
                    else
                    {
                        sb.AppendFormat("�ջ���{0}����ʧ��<br>", entity.ReceiptNo);
                    }
                }

                WriteLog(1, receiptIds, sb.ToString());
                return Content(new JsonMessage {Success = true, Code = "1", Message = sb.ToString()}.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, receiptIds, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message}.ToString());
            }
        }

        /// <summary>
        /// �ջ����ջ�
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
                        sb.AppendFormat("�ջ���{0}������<br>", receiptId);
                        continue;
                    }

                    if (entity.Status != (int)ReceiptStatus.Audited)
                    {
                        sb.AppendFormat("�ջ���{0}���������״̬�������ջ�<br>", entity.ReceiptNo);
                        continue;
                    }

                    if (entity.IsLocked)
                    {
                        sb.AppendFormat("�ջ���{0}�������������ջ�<br>", entity.ReceiptNo);
                        continue;
                    }

                    bool flag = _receiptService.ReceiptQuickReceive(entity);
                    if (flag)
                    {
                        sb.AppendFormat("�ջ���{0}�ջ��ɹ�<br>", entity.ReceiptNo);
                    }
                    else
                    {
                        sb.AppendFormat("�ջ���{0}�ջ�ʧ��<br>", entity.ReceiptNo);
                    }
                }

                WriteLog(1, receiptIds, sb.ToString());
                return Content(new JsonMessage { Success = true, Code = "1", Message = sb.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, receiptIds, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// ȡ���ջ�
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
                        sb.AppendFormat("�ջ���{0}������<br>", receiptId);
                        continue;
                    }

                    if (entity.Status != (int)ReceiptStatus.Received)
                    {
                        sb.AppendFormat("�ջ���{0}�������ջ�״̬������ȡ���ջ�<br>", entity.ReceiptNo);
                        continue;
                    }

                    if (entity.IsLocked)
                    {
                        sb.AppendFormat("�ջ���{0}������������ȡ���ջ�<br>", entity.ReceiptNo);
                        continue;
                    }

                    bool flag = _receiptService.UnReceiptReceive(entity);
                    if (flag)
                    {
                        sb.AppendFormat("�ջ���{0}ȡ���ջ��ɹ�<br>", entity.ReceiptNo);
                    }
                    else
                    {
                        sb.AppendFormat("�ջ���{0}ȡ���ջ�ʧ��<br>", entity.ReceiptNo);
                    }
                }

                WriteLog(1, receiptIds, sb.ToString());
                return Content(new JsonMessage { Success = true, Code = "1", Message = sb.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, receiptIds, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
    }
}