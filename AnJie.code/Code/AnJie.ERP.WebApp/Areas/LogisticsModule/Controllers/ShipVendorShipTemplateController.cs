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
    /// 配送商运费结算模板控制器
    /// </summary>
    public class ShipVendorShipTemplateController : PublicController<ShipVendorShipTemplateEntity>
    {
        private readonly ShipVendorShipTemplateBLL _templateBll = new ShipVendorShipTemplateBLL();

        /// <summary>
        /// 商户管理返回列表Json
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
        /// 表单视图
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ItemForm()
        {
            return View();
        }

        /// <summary>
        /// 模板区域价格列表（返回Json）
        /// </summary>
        /// <param name="TemplateId">模板主键</param>
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
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
    }
}