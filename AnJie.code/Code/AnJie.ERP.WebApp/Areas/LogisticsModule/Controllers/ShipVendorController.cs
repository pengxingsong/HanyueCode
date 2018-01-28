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
    /// 配送商管理表控制器
    /// </summary>
    public class ShipVendorController : PublicController<ShipVendorEntity>
    {
        private readonly ShipVendorBLL _shipVendorBll = new ShipVendorBLL();

        /// <summary>
        /// 配送商管理返回树Json
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
        /// 配送商管理返回列表Json
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
        /// 【配送商管理】返回列表Json
        /// </summary>
        /// <returns></returns>
        public ActionResult ListJson(string ParentId)
        {
            List<ShipVendorEntity> list = _shipVendorBll.GetList();
            return Content(list.ToJson());
        }

        /// <summary>
        /// 【配送商管理】删除数据
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DeleteShipVendor(string KeyValue)
        {
            try
            {
                var message = "删除失败。";
                int isOk = 0;
                int departmentCount = DataFactory.Database().FindCount<Base_Department>("ShipVendorId", KeyValue);
                if (departmentCount == 0)
                {
                    isOk = Repositoryfactory.Repository().Delete(KeyValue);
                    if (isOk > 0)
                    {
                        message = "删除成功。";
                    }
                }
                else
                {
                    message = "配送商内有店铺，不能删除。";
                }
                WriteLog(isOk, KeyValue, message);
                return Content(new JsonMessage { Success = true, Code = isOk.ToString(), Message = message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 【配送商管理】表单赋值
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetShipVendorForm(string KeyValue)
        {
            ShipVendorEntity entity = Repositoryfactory.Repository().FindEntity(KeyValue);
            string strJson = entity.ToJson();
            //自定义
            strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
            return Content(strJson);
        }

        /// <summary>
        /// 【配送商管理】提交表单
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="BuildFormJson">自定义表单</param>
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
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    ShipVendorEntity oldentity = Repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
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
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 返回 商户、分类Json树
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