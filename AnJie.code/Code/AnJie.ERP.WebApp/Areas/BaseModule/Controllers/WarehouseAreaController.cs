using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
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
    /// ��ҵ���������
    /// </summary>
    public class WarehouseAreaController : PublicController<WarehouseAreaEntity>
    {
        private readonly WarehouseAreaBLL _templateBLL = new WarehouseAreaBLL();

        /// <summary>
        /// ��ҵ�������
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
        /// ɾ����ҵ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DeleteArea(string keyValue)
        {
            try
            {
                var message = "ɾ��ʧ�ܡ�";
                int isOk = 0;
                int UserCount = 0;
               // int UserCount = DataFactory.Database().FindCount<WarehouseAreaEntity>("CategoryId", KeyValue);
                if (UserCount == 0)
                {
                    isOk = Repositoryfactory.Repository().Delete(keyValue);
                    if (isOk > 0)
                    {
                        message = "ɾ���ɹ���";
                    }
                }
                else
                {
                  //  Message = "����ҵ�������У�����ɾ����";
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