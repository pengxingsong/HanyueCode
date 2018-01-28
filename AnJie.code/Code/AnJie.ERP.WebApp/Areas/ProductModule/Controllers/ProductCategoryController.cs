using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AnJie.ERP.WebApp.Areas.ProductModule.Controllers
{
    /// <summary>
    /// 商品分类管理控制器
    /// </summary>
    public class ProductCategoryController : PublicController<ProductCategoryEntity>
    {
        readonly ProductCategoryBLL _productCategoryBLL = new ProductCategoryBLL();

        /// <summary>
        /// 返回 商户、分类Json树
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            DataTable dt = _productCategoryBLL.GetTree();
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();

            TreeJsonEntity rootNode = new TreeJsonEntity();
            rootNode.id = "0";
            rootNode.text = "全部";
            rootNode.value = "0";
            rootNode.parentId = "-1";
            rootNode.isexpand = true;
            rootNode.complete = true;
            rootNode.hasChildren = true;
            rootNode.img = "/Content/Images/Icon16/chart_organisation.png";

            treeList.Add(rootNode);

            if (!DataHelper.IsExistRows(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string categoryId = row["CategoryId"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "ParentId='" + categoryId + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = categoryId;
                    tree.text = row["CategoryName"].ToString();
                    tree.value = row["Code"].ToString();
                    tree.parentId = row["ParentId"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.img = "/Content/Images/Icon16/chart_organisation.png";

                    treeList.Add(tree);
                }
            }
            return Content(treeList.TreeToJson("-1"));
        }

        /// <summary>
        /// 分类管理返回表格Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GridListJson()
        {
            DataTable listData = _productCategoryBLL.GetList();
            var jsonData = new
            {
                rows = listData,
            };
            return Content(jsonData.ToJson());
        }

        /// <summary>
        /// 根据商户Id获取分类列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CategoryTreeJson()
        {
            var listData = _productCategoryBLL.GetList();
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (DataRow item in listData.Rows)
            {
                sb.Append("{");
                sb.Append("\"id\":\"" + item["CategoryId"] + "\",");
                sb.Append("\"text\":\"" + item["CategoryName"] + "\",");
                sb.Append("\"value\":\"" + item["CategoryId"] + "\",");
                sb.Append("\"img\":\"/Content/Images/Icon16/chart_organisation.png\",");
                sb.Append("\"isexpand\":true,");
                sb.Append("\"hasChildren\":false");
                sb.Append("},");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return Content(sb.ToString());
        }

        /// <summary>
        /// 分类管理返回列表Json
        /// </summary>
        /// <returns></returns>
        public ActionResult ListJson()
        {
            DataTable listData = _productCategoryBLL.GetList();
            return Content(listData.ToJson());
        }

        /// <summary>
        /// 分类管理删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DeleteCategory(string keyValue)
        {
            try
            {
                var Message = "删除失败。";
                int IsOk = 0;
                int productCount = DataFactory.Database().FindCount<ProductEntity>("CategoryId", keyValue);
                if (productCount == 0)
                {
                    IsOk = Repositoryfactory.Repository().Delete(keyValue);
                    if (IsOk > 0)
                    {
                        Message = "删除成功。";
                    }
                }
                else
                {
                    Message = "该分类内有商品，不能删除。";
                }
                WriteLog(IsOk, keyValue, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, keyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}