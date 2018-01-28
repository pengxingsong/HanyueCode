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
    /// �������������
    /// </summary>
    public class AllocationController : PublicController<SaleOrderEntity>
    {
        private readonly SaleOrderBLL _orderBll = new SaleOrderBLL();

        /// <summary>
        /// ��ȡ����������б�
        /// </summary>
        /// <param name="orderNo">������</param>
        /// <param name="startTime">�Ƶ���ʼʱ��</param>
        /// <param name="endTime">�Ƶ�����ʱ��</param>
        /// <param name="MerchantId"></param>
        /// <param name="jqgridparam">��ҳ����</param>
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
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// ������ϸ�б�������Json��
        /// </summary>
        /// <param name="orderNo">������</param>
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
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// �ֶ����
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
                WriteLog(-1, orderIds, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
    }
}