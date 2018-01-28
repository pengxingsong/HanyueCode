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
    /// 物流方式管理控制器
    /// </summary>
    public class ShipTypeController : PublicController<ShipTypeEntity>
    {
        private readonly ShipTypeBLL _shipTypeBLL = new ShipTypeBLL();

        /// <summary>
        /// 返回配送列表列表JSON
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
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
        /// 返回 物流方式Json树
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            List<ShipTypeEntity> list = _shipTypeBLL.GetList();
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();

            TreeJsonEntity tree = new TreeJsonEntity();
            tree.id = "1";
            tree.text = "全部";
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
        /// 返回物流方式参数列表JSON
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
        /// 提交物流方式表单（新增、编辑）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">物流方式对象</param>
        /// <param name="parameterJson">物流方式参数</param>
        /// <returns></returns>
        public ActionResult SubmitShipTypeForm(string keyValue, ShipTypeEntity entity, string parameterJson)
        {
            try
            {
                string message = keyValue == "" ? "新增成功。" : "编辑成功。";
                int isOk = _shipTypeBLL.SubmitShipTypeForm(keyValue, entity, parameterJson);
                return Content(new JsonMessage { Success = true, Code = isOk.ToString(), Message = message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 查询前面50条用户信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult AutoComplete(string keywords)
        {
            DataTable listData = _shipTypeBLL.SearchShipTypeList(keywords);
            return Content(listData.ToJson());
        }
    }
}