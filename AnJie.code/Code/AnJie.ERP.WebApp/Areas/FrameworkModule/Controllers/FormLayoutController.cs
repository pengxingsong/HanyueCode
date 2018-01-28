using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnJie.ERP.WebApp.Areas.FrameworkModule.Controllers
{
    /// <summary>
    /// 表单附加属性 控制器
    /// </summary>
    public class FormLayoutController : PublicController<Base_FormAttribute>
    {
        private readonly BaseModuleBll _baseModulebll = new BaseModuleBll();
        private readonly Base_FormAttributeBll _baseFormattributebll = new Base_FormAttributeBll();

        /// <summary>
        /// 【系统表单】模块目录
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            List<Base_Module> list = _baseModulebll.GetList();
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();
            foreach (Base_Module item in list)
            {
                string moduleId = item.ModuleId;
                bool hasChildren = false;
                List<Base_Module> childnode = list.FindAll(t => t.ParentId == moduleId);
                if (childnode.Count > 0)
                {
                    hasChildren = true;
                }
                else
                {
                    if (item.Category == "目录")
                    {
                        continue;
                    }
                }
                if (item.Category == "页面")
                    if (item.AllowForm != 1)
                        continue;
                TreeJsonEntity tree = new TreeJsonEntity();
                tree.id = moduleId;
                tree.text = item.FullName;
                tree.value = moduleId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.img = item.Icon != null ? "/Content/Images/Icon16/" + item.Icon : item.Icon;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }

        /// <summary>
        /// 表单设计器
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult Layout()
        {
            string moduleId = Request["ModuleId"];
            string strhtml = _baseFormattributebll.CreateBuildFormTable(2, moduleId);
            ViewBag.BuildFormTable = strhtml;
            return View();
        }
    }
}