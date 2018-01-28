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
    /// ��Ʒ������������
    /// </summary>
    public class ProductCategoryController : PublicController<ProductCategoryEntity>
    {
        readonly ProductCategoryBLL _productCategoryBLL = new ProductCategoryBLL();

        /// <summary>
        /// ���� �̻�������Json��
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            DataTable dt = _productCategoryBLL.GetTree();
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();

            TreeJsonEntity rootNode = new TreeJsonEntity();
            rootNode.id = "0";
            rootNode.text = "ȫ��";
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
        /// ��������ر��Json
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
        /// �����̻�Id��ȡ�����б�
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
        /// ����������б�Json
        /// </summary>
        /// <returns></returns>
        public ActionResult ListJson()
        {
            DataTable listData = _productCategoryBLL.GetList();
            return Content(listData.ToJson());
        }

        /// <summary>
        /// �������ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DeleteCategory(string keyValue)
        {
            try
            {
                var Message = "ɾ��ʧ�ܡ�";
                int IsOk = 0;
                int productCount = DataFactory.Database().FindCount<ProductEntity>("CategoryId", keyValue);
                if (productCount == 0)
                {
                    IsOk = Repositoryfactory.Repository().Delete(keyValue);
                    if (IsOk > 0)
                    {
                        Message = "ɾ���ɹ���";
                    }
                }
                else
                {
                    Message = "�÷���������Ʒ������ɾ����";
                }
                WriteLog(IsOk, keyValue, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, keyValue, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
    }
}