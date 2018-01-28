using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Utilities;
using AnJie.ERP.ViewModel.LogisticsModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnJie.ERP.WebApp.Areas.LogisticsModule.Controllers
{
    /// <summary>
    /// �������˷ѽ���ģ�������
    /// </summary>
    public class ShipVendorShipTemplateController : PublicController<ShipVendorShipTemplateEntity>
    {
        private readonly ShipVendorShipTemplateBLL _templateBll = new ShipVendorShipTemplateBLL();

        /// <summary>
        /// �̻��������б�Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GridListJson(string WarehouseId, string ShipVendorId)
        {
            List<ShipVendorShipTemplateViewModel> listData = _templateBll.GetTemplateList(WarehouseId, ShipVendorId);
            var jsonData = new
            {
                rows = listData
            };
            return Content(jsonData.ToJson());
        }

        /// <summary>
        /// ����ͼ
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ItemForm()
        {
            return View();
        }

        /// <summary>
        /// ģ������۸��б�����Json��
        /// </summary>
        /// <param name="TemplateId">ģ������</param>
        /// <returns></returns>
        public ActionResult GetTemplateItemList(string TemplateId)
        {
            try
            {
                var jsonData = new
                {
                    rows = _templateBll.GetTemplateItemList(TemplateId),
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