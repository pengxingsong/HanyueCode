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

namespace AnJie.ERP.WebApp.Areas.MerchantModule.Controllers
{
    /// <summary>
    /// 商户店铺管理控制器
    /// </summary>
    public class MerchantMallController : PublicController<MerchantMallEntity>
    {
        private readonly MerchantMallBLL _mallBll = new MerchantMallBLL();

        #region 视图

        //商户店铺列表
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult MerchantMallListView()
        {
            return View();
        }

        #endregion

        /// <summary>
        /// 返回 商户、店铺Json树
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            DataTable dt = _mallBll.GetTree();
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string MallId = row["MallId"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "MallId='" + MallId + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = MallId;
                    tree.text = row["MallName"].ToString();
                    tree.value = row["Code"].ToString();
                    tree.parentId = row["ParentId"].ToString();
                    tree.Attribute = "Type";
                    tree.AttributeValue = row["Sort"].ToString();
                    tree.AttributeA = "MerchantId";
                    tree.AttributeValueA = row["MerchantId"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    if (row["ParentId"].ToString() == "0")
                    {
                        tree.img = "/Content/Images/Icon16/basket_shopping.png";
                    }
                    else if (row["sort"].ToString() == "Merchant")
                    {
                        tree.img = "/Content/Images/Icon16/basket_shopping.png";
                    }
                    else if (row["sort"].ToString() == "Mall")
                    {
                        tree.img = "/Content/Images/Icon16/chart_organisation.png";
                    }
                    TreeList.Add(tree);
                }
            }
            return Content(TreeList.TreeToJson());
        }

        /// <summary>
        /// 分类管理返回表格Json
        /// </summary>
        /// <param name="MerchantId">商户ID</param>
        /// <returns></returns>
        public ActionResult GridListJson(string MerchantId)
        {
            DataTable listData = _mallBll.GetList(MerchantId);
            var jsonData = new
            {
                rows = listData,
            };
            return Content(jsonData.ToJson());
        }

        /// <summary>
        /// 根据商户Id获取店铺列表
        /// </summary>
        /// <param name="MerchantId">商户Id</param>
        /// <returns></returns>
        public ActionResult MallTreeJson(string MerchantId)
        {
            DataTable listData = _mallBll.GetList(MerchantId);
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (DataRow item in listData.Rows)
            {
                sb.Append("{");
                sb.Append("\"id\":\"" + item["MallId"] + "\",");
                sb.Append("\"text\":\"" + item["MallName"] + "\",");
                sb.Append("\"value\":\"" + item["MallId"] + "\",");
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
        /// 店铺管理返回列表Json
        /// </summary>
        /// <param name="MerchantId">商户Id</param>
        /// <returns></returns>
        public ActionResult ListJson(string MerchantId)
        {
            var listData = _mallBll.GetList(MerchantId);
            return Content(listData.ToJson());
        }

        /// <summary>
        /// 分类管理删除数据
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DeleteMall(string KeyValue)
        {
            try
            {
                var Message = "删除失败。";
                int IsOk = 0;
                int UserCount = DataFactory.Database().FindCount<SaleOrderEntity>("MerchantMallId", KeyValue);
                if (UserCount == 0)
                {
                    IsOk = Repositoryfactory.Repository().Delete(KeyValue);
                    if (IsOk > 0)
                    {
                        Message = "删除成功。";
                    }
                }
                else
                {
                    Message = "该店铺下已有订单，不能删除。";
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

        //商户店铺查询
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult GetMerchantMallAll()
        {
            try
            {
                var result = _mallBll.GetListAll();
                if(result!=null && result.Rows.Count > 0)
                    return Content(result.ToJson());
                return Content(new JsonMessage { Success = true, Code = "0", Message = "未查询到结果!" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

    }
}