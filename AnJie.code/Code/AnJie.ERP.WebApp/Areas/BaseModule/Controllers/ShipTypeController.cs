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
using AnJie.ERP.Plugins.Express;
using AnJie.ERP.Service;

namespace AnJie.ERP.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// ������ʽ���������
    /// </summary>
    public class ShipTypeController : PublicController<ShipTypeEntity>
    {
        private readonly ShipTypeBLL _shipTypeBLL = new ShipTypeBLL();

        /// <summary>
        /// ���������б��б�JSON
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam)
        {
            Stopwatch watch = CommonHelper.TimerStart();
            List<ShipTypeEntity> listData = _shipTypeBLL.GetPageList(ref jqgridparam);
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

        /// <summary>
        /// ���� ������ʽJson��
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            List<ShipTypeEntity> list = _shipTypeBLL.GetList();
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();

            TreeJsonEntity tree = new TreeJsonEntity();
            tree.id = "1";
            tree.text = "ȫ��";
            tree.value = "";
            tree.parentId = "0";
            tree.isexpand = true;
            tree.complete = true;
            tree.hasChildren = true;
            tree.img = "/Content/Images/Icon16/basket_shopping.png";
            treeList.Add(tree);

            foreach (ShipTypeEntity entity in list)
            {
                TreeJsonEntity node = new TreeJsonEntity();
                node.id = entity.ShipTypeId;
                node.text = entity.ShipTypeName;
                node.value = entity.ShipTypeId;
                node.parentId = "1";
                node.isexpand = true;
                node.complete = true;
                node.hasChildren = false;
                node.img = "/Content/Images/Icon16/basket_shopping.png";
                
                treeList.Add(node);
            }

            return Content(treeList.TreeToJson());
        }

        /// <summary>
        /// ����������ʽ�����б�JSON
        /// </summary>
        /// <param name="shipTypeId"></param>
        /// <returns></returns>
        public ActionResult GridShipTypeListJson(string shipTypeId)
        {
            List<ShipTypeConfigEntity> listData = _shipTypeBLL.GetShipTypeConfigList(shipTypeId);
            var jsonData = new
            {
                rows = listData,
            };
            return Content(jsonData.ToJson());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ListJson()
        {
            List<ShipTypeEntity> listData = _shipTypeBLL.GetList();
            return Content(listData.ToJson());
        }

        public ActionResult ListExpressNumStrategy()
        {
            ExpressService expressService = new ExpressService();
            List<IExpressStrategy> listData = expressService.GetAllExpress();
            return Content(listData.ToJson());
        }

        /// <summary>
        /// �ύ������ʽ�����������༭��
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="entity">������ʽ����</param>
        /// <param name="parameterJson">������ʽ����</param>
        /// <returns></returns>
        public ActionResult SubmitShipTypeForm(string keyValue, ShipTypeEntity entity, string parameterJson)
        {
            try
            {
                string message = keyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                int isOk = _shipTypeBLL.SubmitShipTypeForm(keyValue, entity, parameterJson);
                return Content(new JsonMessage { Success = true, Code = isOk.ToString(), Message = message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// ��ѯǰ��50���û���Ϣ������JSON��
        /// </summary>
        /// <param name="keywords">��ѯ�ؼ���</param>
        /// <returns></returns>
        public ActionResult AutoComplete(string keywords)
        {
            DataTable listData = _shipTypeBLL.SearchShipTypeList(keywords);
            return Content(listData.ToJson());
        }
    }
}