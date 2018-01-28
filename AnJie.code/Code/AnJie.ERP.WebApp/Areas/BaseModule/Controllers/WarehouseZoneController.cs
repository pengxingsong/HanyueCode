using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnJie.ERP.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// 库存信息控制器
    /// </summary>
    public class WarehouseZoneController : PublicController<WarehouseZoneEntity>
    {
        private readonly WarehouseZoneBLL _templateBLL = new WarehouseZoneBLL();

        /// <summary>
        /// 库区管理
        /// </summary>
        /// <param name="warehouseId">物流方式ID</param>
        /// <returns></returns>
        public ActionResult GridListJson(string warehouseId)
        {
            DataTable listData = _templateBLL.GetList(warehouseId);
            var jsonData = new
            {
                rows = listData,
            };
            return Content(jsonData.ToJson());
        }

        /// <summary>
        /// 库区管理返回列表Json
        /// </summary>
        /// <returns></returns>
        public ActionResult ListJson(string warehouseId)
        {
            DataTable listData = _templateBLL.GetList(warehouseId);
            return Content(listData.ToJson());
        }

        /// <summary>
        /// 删除库区
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DeleteZone(string keyValue)
        {
            try
            {
                var message = "删除失败。";
                int isOk = 0;
                int userCount = 0;
                // int UserCount = DataFactory.Database().FindCount<WarehouseAreaEntity>("CategoryId", KeyValue);
                if (userCount == 0)
                {
                    isOk = Repositoryfactory.Repository().Delete(keyValue);
                    if (isOk > 0)
                    {
                        message = "删除成功。";
                    }
                }
                else
                {
                    //  Message = "该分类内有商品，不能删除。";
                }
                WriteLog(isOk, keyValue, message);
                return Content(new JsonMessage { Success = true, Code = isOk.ToString(), Message = message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, keyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}