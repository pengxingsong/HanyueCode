using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AnJie.ERP.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// 仓库信息控制器
    /// </summary>
    public class WarehouseController : PublicController<WarehouseEntity>
    {
        private readonly WarehouseBLL _warehouseBLL = new WarehouseBLL();

        private readonly WarehouseLocationBLL _warehouseLocationBLL = new WarehouseLocationBLL();

        /// <summary>
        /// 管理返回表格Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GridListJson(string companyId)
        {
            var listData = _warehouseBLL.GetList(companyId);
            var jsonData = new
            {
                rows = listData
            };
            return Content(jsonData.ToJson());
        }

        public ActionResult GetList()
        {
            var listData = _warehouseBLL.GetList();
            return Content(listData.ToJson());
        }

        /// <summary>
        /// 返回仓库列表树
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            var list = _warehouseBLL.GetList();
            var treeList = new List<TreeJsonEntity>();

            var tree = new TreeJsonEntity();
            tree.id = "1";
            tree.text = "全部";
            tree.value = "";
            tree.parentId = "0";
            tree.isexpand = true;
            tree.complete = true;
            tree.hasChildren = true;
            tree.img = "/Content/Images/Icon16/basket_shopping.png";
            treeList.Add(tree);

            foreach (WarehouseEntity entity in list)
            {
                var node = new TreeJsonEntity
                {
                    id = entity.WarehouseId,
                    text = entity.WarehouseName,
                    value = entity.WarehouseId,
                    parentId = "1",
                    isexpand = true,
                    complete = true,
                    hasChildren = false,
                    img = "/Content/Images/Icon16/basket_shopping.png"
                };

                treeList.Add(node);
            }

            return Content(treeList.TreeToJson());
        }

        /// <summary>
        /// 设置仓库默认收货储位
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult SetLocation()
        {
            return View();
        }

        /// <summary>
        /// 设置仓库默认收货储位
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        public ActionResult SubmitSetLocation(string warehouseId, string locationId)
        {
            try
            {
                var entity = Repositoryfactory.Repository().FindEntity("warehouseId", warehouseId);
                if (entity == null)
                {
                    throw new Exception("仓库信息不存在");
                }

                var location = _warehouseLocationBLL.GetLocation(warehouseId, locationId);
                if (location == null || location.IsEnable != (int)YesNoStatus.Yes)
                {
                    throw new Exception("储位信息无效");
                }

                entity.Modify(entity.WarehouseId);
                entity.ReceiptLocationId = location.LocationId;
                bool flag = _warehouseBLL.UpdateReceiptLocationId(entity);

                if (!flag)
                {
                    throw new Exception("设置默认收货储位失败");
                }

                WriteLog(1, warehouseId, "设置默认收货储位成功");
                return Content(new JsonMessage { Success = true, Code = "1", Message = "设置默认收货储位成功" }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, warehouseId, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}