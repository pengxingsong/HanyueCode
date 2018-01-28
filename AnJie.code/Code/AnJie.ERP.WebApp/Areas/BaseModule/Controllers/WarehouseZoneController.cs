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
    /// �����Ϣ������
    /// </summary>
    public class WarehouseZoneController : PublicController<WarehouseZoneEntity>
    {
        private readonly WarehouseZoneBLL _templateBLL = new WarehouseZoneBLL();

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="warehouseId">������ʽID</param>
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
        /// �����������б�Json
        /// </summary>
        /// <returns></returns>
        public ActionResult ListJson(string warehouseId)
        {
            DataTable listData = _templateBLL.GetList(warehouseId);
            return Content(listData.ToJson());
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DeleteZone(string keyValue)
        {
            try
            {
                var message = "ɾ��ʧ�ܡ�";
                int isOk = 0;
                int userCount = 0;
                // int UserCount = DataFactory.Database().FindCount<WarehouseAreaEntity>("CategoryId", KeyValue);
                if (userCount == 0)
                {
                    isOk = Repositoryfactory.Repository().Delete(keyValue);
                    if (isOk > 0)
                    {
                        message = "ɾ���ɹ���";
                    }
                }
                else
                {
                    //  Message = "�÷���������Ʒ������ɾ����";
                }
                WriteLog(isOk, keyValue, message);
                return Content(new JsonMessage { Success = true, Code = isOk.ToString(), Message = message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, keyValue, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
    }
}