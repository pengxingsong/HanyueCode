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
    /// �ɹ������������
    /// </summary>
    public class PurchaseOrderController : PublicController<PurchaseOrderEntity>
    {
        readonly PurchaseOrderBLL orderBLL = new PurchaseOrderBLL();

        readonly BaseCodeRuleBll codeRuleBLL = new BaseCodeRuleBll();

        readonly ProductBLL productBLL = new ProductBLL();

        private static readonly string PurchaseOrderCodeName = "PurchaseOrder";

        /// <summary>
        /// ��ȡ�Զ����ݱ���
        /// </summary>
        /// <returns></returns>
        private string CreateOrderCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return codeRuleBLL.GetBillCode(UserId, PurchaseOrderCodeName);
        }

        /// <summary>
        /// ������
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
        /// �����б�����Json��
        /// </summary>
        /// <param name="OrderNo">������</param>
        /// <param name="StartTime">�Ƶ���ʼʱ��</param>
        /// <param name="EndTime">�Ƶ�����ʱ��</param>
        /// <param name="jqgridparam">��ҳ����</param>
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
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// ������ϸ�б�����Json��
        /// </summary>
        /// <param name="OrderId">��������</param>
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
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// �ύ���������������༭��
        /// </summary>
        /// <param name="KeyValue">��������</param>
        /// <param name="entity">����ʵ��</param>
        /// <param name="PurchaseOrderItemJson">������ϸ</param>
        /// <returns></returns>
        public ActionResult SubmitOrderForm(string KeyValue, PurchaseOrderEntity entity, string PurchaseOrderItemJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "�����ɹ���" : "�޸ĳɹ���";
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
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
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
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult Audit(string KeyValue)
        {
            try
            {
                var Message = "���ʧ�ܡ�";
                int IsOk = 0;

                var entity = Repositoryfactory.Repository().FindEntity("OrderId", KeyValue);
                if (entity == null || entity.Status != 0)
                {
                    throw new Exception("�ö������ǳ�ʼ״̬���������");
                }

                entity.Modify(KeyValue);
                IsOk = orderBLL.Audit(entity);
                if (IsOk > 0)
                {
                    Message = "��˳ɹ���";
                }
                WriteLog(IsOk, KeyValue, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult Invalid(string KeyValue)
        {
            try
            {
                var Message = "���ʧ�ܡ�";
                int IsOk = 0;

                var entity = Repositoryfactory.Repository().FindEntity("OrderId", KeyValue);
                if (entity == null || entity.Status != 0)
                {
                    throw new Exception("�òɹ������ǳ�ʼ״̬����������");
                }

                entity.Modify(KeyValue);
                IsOk = orderBLL.Invalid(entity);
                if (IsOk > 0)
                {
                    Message = "���ϳɹ���";
                }
                WriteLog(IsOk, KeyValue, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
    }
}