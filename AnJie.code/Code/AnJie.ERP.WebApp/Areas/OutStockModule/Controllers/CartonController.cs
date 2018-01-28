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
    /// ����������
    /// </summary>
    public class CartonController : PublicController<CartonEntity>
    {
        private readonly CartonBLL _cartonBll = new CartonBLL();

        /// <summary>
        /// �����б�����Json��
        /// </summary>
        /// <param name="orderNo">������</param>
        /// <param name="startTime">�Ƶ���ʼʱ��</param>
        /// <param name="endTime">�Ƶ�����ʱ��</param>
        /// <param name="MerchantId"></param>
        /// <param name="jqgridparam">��ҳ����</param>
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
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }
    }
}