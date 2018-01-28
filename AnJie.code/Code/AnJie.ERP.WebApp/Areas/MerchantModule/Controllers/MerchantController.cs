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
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AnJie.ERP.WebApp.Areas.MerchantModule.Controllers
{
    /// <summary>
    /// 商户管理控制器
    /// </summary>
    public class MerchantController : PublicController<MerchantEntity>
    {
        private readonly MerchantBLL _merchantBll = new MerchantBLL();

        /// <summary>
        /// 返回 商户、分类Json树
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWarehouseMerchantTree()
        {
            DataTable dt = _merchantBll.GetWarehouseMerchantTree();
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string NodeId = row["NodeId"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "ParentId='" + NodeId + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = NodeId;
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

        /// <summary>
        /// 返回 商户、分类Json树
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMerchantWarehouseTree()
        {
            DataTable dt = _merchantBll.GetMerchantWarehouseTree();
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string NodeId = row["NodeId"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "ParentId='" + NodeId + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = NodeId;
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
                    TreeList.Add(tree);
                }
            }
            return Content(TreeList.TreeToJson());
        }

        /// <summary>
        /// 商户管理返回树Json
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            List<MerchantEntity> list = _merchantBll.GetList();
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            foreach (MerchantEntity item in list)
            {
                bool hasChildren = false;

                TreeJsonEntity tree = new TreeJsonEntity();
                tree.id = item.MerchantId;
                tree.text = item.FullName;
                tree.value = item.MerchantId;
                tree.Attribute = "Code";
                tree.AttributeValue = item.Code;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.parentId = "0";
                tree.img = "/Content/Images/Icon16/account_balances.png";
                TreeList.Add(tree);
            }
            return Content(TreeList.TreeToJson());
        }

        /// <summary>
        /// 商户管理返回列表Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GridListJson()
        {
            List<MerchantEntity> ListData = _merchantBll.GetList();
            var JsonData = new
            {
                rows = ListData
            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ListJson()
        {
            List<MerchantEntity> ListData = _merchantBll.GetList();
            return Content(ListData.ToJson());
        }

        /// <summary>
        /// 【商户管理】删除数据
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DeleteMerchant(string KeyValue)
        {
            try
            {
                var Message = "删除失败。";
                int IsOk = 0;
                int DepartmentCount = DataFactory.Database().FindCount<MerchantMallEntity>("MerchantId", KeyValue);
                if (DepartmentCount == 0)
                {
                    IsOk = Repositoryfactory.Repository().Delete(KeyValue);
                    if (IsOk > 0)
                    {
                        Message = "删除成功。";
                    }
                }
                else
                {
                    Message = "商户内有店铺，不能删除。";
                }
                WriteLog(IsOk, KeyValue, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 【商户管理】表单赋值
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SetMerchantForm(string KeyValue)
        {
            MerchantEntity entity = Repositoryfactory.Repository().FindEntity(KeyValue);
            string strJson = entity.ToJson();
            //自定义
            strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
            return Content(strJson);
        }

        /// <summary>
        /// 【商户管理】提交表单
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="BuildFormJson">自定义表单</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SubmitMerchantForm(MerchantEntity entity, string KeyValue, string BuildFormJson)
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
                    MerchantEntity Oldentity = Repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    IsOk = database.Update(entity, isOpenTrans);
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);
                }
                else
                {
                    entity.Create();
                    IsOk = database.Insert(entity, isOpenTrans);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
                }
                Base_FormAttributeBll.Instance.SaveBuildForm(BuildFormJson, entity.MerchantId, ModuleId, isOpenTrans);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult MerchantWarehouse()
        {
            return View();
        }

        /// <summary>
        /// 加载商户仓库列表
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public ActionResult MerchantWarehouseList(string merchantId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = _merchantBll.MerchantWarehouseList(merchantId);
            foreach (DataRow dr in dt.Rows)
            {
                string strchecked = "";
                if (!string.IsNullOrEmpty(dr["RelationId"].ToString()))//判断是否选中
                {
                    strchecked = "selected";
                }
                sb.Append("<li title=\"" + dr["WarehouseName"] + "(" + dr["WarehouseCode"] + ")" + "\" class=\"" + strchecked + "\">");
                sb.Append("<a id=\"" + dr["WarehouseId"] + "\"><img src=\"../../Content/Images/Icon16/role.png \">" + dr["WarehouseName"] + "</a><i></i>");
                sb.Append("</li>");
            }
            return Content(sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public ActionResult MerchantWarehouseSubmit(string merchantId, string objectId)
        {
            try
            {
                string[] array = objectId.Split(',');
                int IsOk = _merchantBll.SetMerchantWarehouse(merchantId, array);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }
    }
}