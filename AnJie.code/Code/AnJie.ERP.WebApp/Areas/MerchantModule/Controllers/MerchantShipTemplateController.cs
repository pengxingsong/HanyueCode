using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using AnJie.ERP.ViewModel.MerchantModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnJie.ERP.WebApp.Areas.MerchantModule.Controllers
{
    /// <summary>
    /// �̻��˷ѽ���ģ�������
    /// </summary>
    public class MerchantShipTemplateController : PublicController<MerchantShipTemplateEntity>
    {
        private readonly MerchantShipTemplateBLL _templateBll = new MerchantShipTemplateBLL();

        /// <summary>
        /// �̻��������б�Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GridListJson(string WarehouseId, string MerchantId)
        {
            var listData = _templateBll.GetTemplateList(WarehouseId, MerchantId);
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
        /// <param name="OrderId">��������</param>
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