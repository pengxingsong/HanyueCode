using AnJie.ERP.Business;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnJie.ERP.WebApp.Areas.LogisticsModule.Controllers
{
    /// <summary>
    /// �����̹���������
    /// </summary>
    public class ShipVendorController : PublicController<ShipVendorEntity>
    {
        private readonly ShipVendorBLL _shipVendorBll = new ShipVendorBLL();

        /// <summary>
        /// �����̹�������Json
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            List<ShipVendorEntity> list = _shipVendorBll.GetList();
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();
            foreach (ShipVendorEntity item in list)
            {
                TreeJsonEntity tree = new TreeJsonEntity();
                tree.id = item.ShipVendorId;
                tree.text = item.FullName;
                tree.value = item.ShipVendorId;
                tree.Attribute = "Code";
                tree.AttributeValue = item.Code;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = false;
                tree.parentId = "0";
                tree.img = "/Content/Images/Icon16/account_balances.png";
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }

        /// <summary>
        /// �����̹������б�Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GridListJson()
        {
            List<ShipVendorEntity> listData = _shipVendorBll.GetList();
            var jsonData = new
            {
                rows = listData,
            };
            return Content(jsonData.ToJson());
        }

        /// <summary>
        /// �������̹��������б�Json
        /// </summary>
        /// <returns></returns>
        public ActionResult ListJson(string ParentId)
        {
            List<ShipVendorEntity> list = _shipVendorBll.GetList();
            return Content(list.ToJson());
        }

        /// <summary>
        /// �������̹���ɾ������
        /// </summary>
        /// <param name="KeyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DeleteShipVendor(string KeyValue)
        {
            try
            {
                var message = "ɾ��ʧ�ܡ�";
                int isOk = 0;
                int departmentCount = DataFactory.Database().FindCount<Base_Department>("ShipVendorId", KeyValue);
                if (departmentCount == 0)
                {
                    isOk = Repositoryfactory.Repository().Delete(KeyValue);
                    if (isOk > 0)
                    {
                        message = "ɾ���ɹ���";
                    }
                }
                else
                {
                    message = "���������е��̣�����ɾ����";
                }
                WriteLog(isOk, KeyValue, message);
                return Content(new JsonMessage { Success = true, Code = isOk.ToString(), Message = message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// �������̹�������ֵ
        /// </summary>
        /// <param name="KeyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetShipVendorForm(string KeyValue)
        {
            ShipVendorEntity entity = Repositoryfactory.Repository().FindEntity(KeyValue);
            string strJson = entity.ToJson();
            //�Զ���
            strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
            return Content(strJson);
        }

        /// <summary>
        /// �������̹����ύ��
        /// </summary>
        /// <param name="entity">ʵ�����</param>
        /// <param name="KeyValue">����ֵ</param>
        /// <param name="BuildFormJson">�Զ����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitShipVendorForm(ShipVendorEntity entity, string KeyValue, string BuildFormJson)
        {
            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    ShipVendorEntity oldentity = Repositoryfactory.Repository().FindEntity(KeyValue);//��ȡû����֮ǰʵ�����
                    entity.Modify(KeyValue);
                    IsOk = database.Update(entity, isOpenTrans);
                    this.WriteLog(IsOk, entity, oldentity, KeyValue, Message);
                }
                else
                {
                    entity.Create();
                    IsOk = database.Insert(entity, isOpenTrans);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
                }
                Base_FormAttributeBll.Instance.SaveBuildForm(BuildFormJson, entity.ShipVendorId, ModuleId, isOpenTrans);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// ���� �̻�������Json��
        /// </summary>
        /// <returns></returns>
        public ActionResult GetShipVendorWarehouseTree()
        {
            var dt = _shipVendorBll.GetShipVendorWarehouseTree();
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string nodeId = row["NodeId"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "ParentId='" + nodeId + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = nodeId;
                    tree.text = row["NodeName"].ToString();
                    tree.value = row["Code"].ToString();
                    tree.parentId = row["ParentId"].ToString();
                    tree.Attribute = "Type";
                    tree.AttributeValue = row["Sort"].ToString();
                    tree.AttributeA = "NodeId";
                    tree.AttributeValueA = row["NodeId"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    if (row["ParentId"].ToString() == "0")
                    {
                        tree.img = "/Content/Images/Icon16/basket_shopping.png";
                    }
                    else if (row["sort"].ToString() == "Warehouse")
                    {
                        tree.img = "/Content/Images/Icon16/basket_shopping.png";
                    }
                    else if (row["sort"].ToString() == "Merchant")
                    {
                        tree.img = "/Content/Images/Icon16/chart_organisation.png";
                    }
                    treeList.Add(tree);
                }
            }
            return Content(treeList.TreeToJson());
        }
    }
}