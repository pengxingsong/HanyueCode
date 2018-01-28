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

namespace AnJie.ERP.WebApp.Areas.OrganizationModule.Controllers
{
    /// <summary>
    /// 权限操作控制器
    /// </summary>
    public class PermissionController : Controller
    {
        private readonly BaseModulePermissionBll _baseModulepermissionbll = new BaseModulePermissionBll();
        private readonly BaseButtonPermissionBll _baseButtonpermissionbll = new BaseButtonPermissionBll();
        private readonly BaseViewPermissionBll _baseViewpermissionbll = new BaseViewPermissionBll();
        private readonly BaseObjectUserRelationBll _baseObjectuserrelationbll = new BaseObjectUserRelationBll();
        private readonly BaseModuleBll _baseModulebll = new BaseModuleBll();
        private readonly BaseDataScopePermissionBLL _baseDatascopepermissionbll = new BaseDataScopePermissionBLL();

        #region 分配权限

        /// <summary>
        /// 分配权限
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult AllotPermission()
        {
            return View();
        }

        /// <summary>
        /// 加载模块 返回树JSON
        /// </summary>
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// <returns></returns>
        public ActionResult ModuleTree(string objectId, string category)
        {
            DataTable dt = _baseModulepermissionbll.GetList(objectId, category);
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dt))
            {
                foreach (DataRow item in dt.Rows)
                {
                    string moduleId = item["moduleid"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "ParentId = '" + moduleId + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    var tree = new TreeJsonEntity
                    {
                        id = item["moduleid"].ToString(),
                        text = item["fullname"].ToString(),
                        value = item["moduleid"].ToString(),
                        checkstate = item["objectid"].ToString() != "" ? 1 : 0,
                        showcheck = true,
                        isexpand = true,
                        complete = true,
                        hasChildren = hasChildren,
                        parentId = item["parentid"].ToString(),
                        img =
                            item["icon"].ToString() != string.Empty
                                ? "/Content/Images/Icon16/" + item["icon"].ToString()
                                : item["icon"].ToString()
                    };
                    treeList.Add(tree);
                }
            }
            return Content(treeList.TreeToJson());
        }

        /// <summary>
        /// 加载按钮
        /// </summary>
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门</param>
        /// <returns></returns>
        public ActionResult ButtoneList(string objectId, string category)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = _baseButtonpermissionbll.GetList(objectId, category);
            foreach (DataRow dr in dt.Rows)
            {
                string strchecked = "";
                if (!string.IsNullOrEmpty(dr["objectid"].ToString())) //判断是否选中
                {
                    strchecked = "selected";
                }
                sb.Append("<li title=\"" + dr["fullname"] + "\" moduleid=\"" + dr["moduleid"] +
                          "\" style='display:none;' class=\"" + strchecked + "\">");
                sb.Append("<a class=\"disabled Category_" + dr["category"] + "\" id=\"" + dr["buttonid"] + "|" +
                          dr["moduleid"] + "\"><img src=\"../../Content/Images/Icon16/" + dr["icon"] + "\">" +
                          dr["fullname"] + "</a><i></i>");
                sb.Append("</li>");
            }
            return Content(sb.ToString());
        }

        /// <summary>
        /// 加载视图
        /// </summary>
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// <returns></returns>
        public ActionResult ViewList(string objectId, string category)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = _baseViewpermissionbll.GetList(objectId, category);
            foreach (DataRow dr in dt.Rows)
            {
                string strchecked = "";
                if (!string.IsNullOrEmpty(dr["objectid"].ToString())) //判断是否选中
                {
                    strchecked = "selected";
                }
                sb.Append("<li title=\"" + dr["showname"] + "\" moduleid=\"" + dr["moduleid"] +
                          "\" style='display:none;' class=\"" + strchecked + "\">");
                sb.Append("<a class=\"disabled\" id=\"" + dr["viewid"] + "|" + dr["moduleid"] +
                          "\"><img src=\"../../Content/Images/Icon16/tag_blue.png\">" + dr["showname"] + "</a><i></i>");
                sb.Append("</li>");
            }
            return Content(sb.ToString());
        }

        /// <summary>
        /// 权限授权提交事件
        /// </summary>
        /// <param name="moduleId">访问权限值</param>
        /// <param name="moduleButtonId">操作权限值</param>
        /// <param name="viewDetailId">视图权限值</param>
        /// <param name="objectId">对象ID</param>
        /// <param name="category">分类</param>
        /// <returns></returns>
        public ActionResult AuthorizedSubmit(string moduleId, string moduleButtonId, string viewDetailId,
            string objectId, string category)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                var parameter = new List<DbParameter>
                {
                    DbFactory.CreateDbParameter("@ObjectId", objectId),
                    DbFactory.CreateDbParameter("@Category", category)
                };

                #region 访问

                string[] arrayModuleId = moduleId.Split(',');
                StringBuilder sbDeleteModule =
                    new StringBuilder(
                        "delete from Base_ModulePermission where ObjectId = @ObjectId AND Category=@Category");
                database.ExecuteBySql(sbDeleteModule, parameter.ToArray(), isOpenTrans);
                int index = 1;
                foreach (var item in arrayModuleId)
                {
                    if (item.Length > 0)
                    {
                        var entity = new Base_ModulePermission
                        {
                            ModulePermissionId = CommonHelper.GetGuid,
                            ObjectId = objectId,
                            Category = category,
                            ModuleId = item,
                            SortCode = index
                        };
                        entity.Create();
                        database.Insert(entity, isOpenTrans);
                        index++;
                    }
                }

                #endregion

                #region 操作

                string[] arrayModuleButtonId = moduleButtonId.Split(',');
                StringBuilder sbDeleteButton =
                    new StringBuilder(
                        "delete from Base_ButtonPermission where ObjectId = @ObjectId AND Category=@Category");
                database.ExecuteBySql(sbDeleteButton, parameter.ToArray(), isOpenTrans);
                index = 1;
                foreach (var item in arrayModuleButtonId)
                {
                    if (item.Length > 0)
                    {
                        string[] stritem = item.Split('|');
                        Base_ButtonPermission entity = new Base_ButtonPermission();
                        entity.ButtonPermissionId = CommonHelper.GetGuid;
                        entity.ObjectId = objectId;
                        entity.Category = category;
                        entity.ModuleButtonId = stritem[0];
                        entity.ModuleId = stritem[0];
                        entity.SortCode = index;
                        entity.Create();
                        database.Insert(entity, isOpenTrans);
                        index++;
                    }
                }

                #endregion

                #region 视图

                string[] arrayViewDetailId = viewDetailId.Split(',');
                StringBuilder sbDeleteView =
                    new StringBuilder(
                        "delete from Base_ViewPermission where ObjectId = @ObjectId AND Category=@Category");
                database.ExecuteBySql(sbDeleteView, parameter.ToArray(), isOpenTrans);
                index = 1;
                foreach (var item in arrayViewDetailId)
                {
                    if (item.Length > 0)
                    {
                        string[] stritem = item.Split('|');
                        Base_ViewPermission entity = new Base_ViewPermission
                        {
                            ViewPermissionId = CommonHelper.GetGuid,
                            ObjectId = objectId,
                            Category = category,
                            ViewId = stritem[0],
                            ModuleId = stritem[1],
                            SortCode = index
                        };
                        entity.Create();
                        database.Insert(entity, isOpenTrans);
                        index++;
                    }
                }

                #endregion

                database.Commit();
                return Content(new JsonMessage {Success = true, Code = "1", Message = "操作成功。"}.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return
                    Content(new JsonMessage {Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message}.ToString());
            }
        }

        #endregion

        #region 分配用户

        /// <summary>
        /// 根据部门Id加载用户视图
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult AllotUser()
        {
            return View();
        }

        /// <summary>
        /// 根据公司Id/部门Id加载用户视图
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult AllotMember()
        {
            return View();
        }

        /// <summary>
        /// 根据公司Id加载用户列表
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="departmentId">部门ID</param>
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// <returns></returns>
        public ActionResult MemberList(string companyId, string departmentId, string objectId, string category)
        {
            StringBuilder sb = new StringBuilder();
            DataTable listData = _baseObjectuserrelationbll.GetList(companyId, departmentId, objectId, category);
            if (listData != null && listData.Rows.Count != 0)
            {
                foreach (DataRow item in listData.Rows)
                {
                    string genderimg = "user_female.png";
                    if (item["Gender"].ToString() == "男")
                    {
                        genderimg = "user_green.png";
                    }
                    string strchecked = "";
                    if (!string.IsNullOrEmpty(item["objectid"].ToString())) //判断是否选中
                    {
                        strchecked = "selected";
                    }
                    sb.Append("<li class=\"" + item["departmentid"] + " " + strchecked + "\">");
                    sb.Append("<a class=\"a_" + strchecked + "\" id=\"" + item["userid"] + "\" title='工号：" +
                              item["code"] + "\r\n账户：" + item["account"] + "'><img src=\"/Content/Images/Icon16/" +
                              genderimg + "\">" + item["realname"] + "</a><i></i>");
                    sb.Append("</li>");
                }
            }
            return Content(sb.ToString());
        }

        /// <summary>
        /// 添加用户 - 提交事件
        /// </summary>
        /// <param name="userId">选中用户ID: 1,2,3,4,5,6</param>
        /// <param name="objectId"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public ActionResult AuthorizedMember(string userId, string objectId, string category)
        {
            try
            {
                string[] array = userId.Split(',');
                int IsOk = _baseObjectuserrelationbll.BatchAddMember(array, objectId, category);
                return Content(new JsonMessage {Success = true, Code = IsOk.ToString(), Message = "操作成功。"}.ToString());
            }
            catch (Exception ex)
            {
                return
                    Content(new JsonMessage {Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message}.ToString());
            }
        }

        #endregion

        #region 分配用户 批量

        /// <summary>
        /// 分配用户 批量
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult AllotMemberBatch()
        {
            return View();
        }

        #endregion

        #region 数据范围

        /// <summary>
        /// 数据范围
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ScopePermission()
        {
            return View();
        }

        /// <summary>
        /// 加载授权项目
        /// </summary>
        /// <returns></returns>
        public ActionResult ScopeAuthorizedProject()
        {
            StringBuilder sbHtml = new StringBuilder();
            List<Base_Module> list = _baseModulebll.GetList().FindAll(t => t.DataScope == 1);
            int index = 0;
            string leftselected = "class=\"leftselected\"";
            foreach (Base_Module entity in list)
            {
                if (index > 0)
                    leftselected = "";
                sbHtml.Append("<li>");
                sbHtml.Append("    <div ModuleId=\"" + entity.ModuleId + "\"  " + leftselected + ">");
                sbHtml.Append("        <img src=\"../../Content/Images/Icon16/" + entity.Icon + "\"><span>" +
                              entity.FullName + "</span>");
                sbHtml.Append("    </div>");
                sbHtml.Append("</li>");
                index++;
            }
            return Content(sbHtml.ToString());
            //StringBuilder sbJson = new StringBuilder();
            //List<Base_Module> list = base_modulebll.GetList().FindAll(t => t.DataScope == 1);
            //if (list.Count > 0 )
            //{
            //    foreach (Base_Module entity in list)
            //    {
            //        sbJson.Append("{");
            //        sbJson.Append("\"id\":\"" + entity.ModuleId + "\",");
            //        sbJson.Append("\"text\":\"" + entity.FullName + "\",");
            //        sbJson.Append("\"value\":\"" + entity.Code + "\",");
            //        sbJson.Append("\"isexpand\":true,");
            //        sbJson.Append("\"img\":\"/Content/Images/Icon16/" + entity.Icon + "\",");
            //        sbJson.Append("\"hasChildren\":false");
            //        sbJson.Append("},");
            //    }
            //    sbJson = sbJson.Remove(sbJson.Length - 1, 1);
            //}
            //StringBuilder strJson = new StringBuilder();
            //strJson.Append("[{");
            //strJson.Append("\"id\":\"0\",");
            //strJson.Append("\"text\":\"授权项目\",");
            //strJson.Append("\"value\":\"0\",");
            //strJson.Append("\"isexpand\":true,");
            //strJson.Append("\"img\":\"/Content/Images/Icon16/change_password.png\",");
            //strJson.Append("\"hasChildren\":true,");
            //strJson.Append("\"ChildNodes\":[" + sbJson + "]");
            //strJson.Append("}]");
            //return Content(strJson.ToString());
        }

        /// <summary>
        /// 授权提交事件
        /// </summary>
        /// <param name="scopeType">范围类型:1-显示设置；2-条件设置</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="resourceId">对什么资源Id</param>
        /// <param name="objectId">对象ID</param>
        /// <param name="category">分类</param>
        /// <returns></returns>
        public ActionResult ScopeAuthorizedSubmit(string scopeType, string moduleId, string resourceId, string objectId,
            string category)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                Hashtable htDelete = new Hashtable();
                htDelete["ModuleId"] = moduleId;
                htDelete["ObjectId"] = objectId;
                htDelete["Category"] = category;
                database.Delete("Base_DataScopePermission", htDelete, isOpenTrans);
                string[] arrayResourceId = resourceId.Split(',');
                int index = 1;
                foreach (var item in arrayResourceId)
                {
                    if (item.Length > 0)
                    {
                        Base_DataScopePermission entity = new Base_DataScopePermission();
                        entity.DataScopePermissionId = CommonHelper.GetGuid;
                        entity.ObjectId = objectId;
                        entity.Category = category;
                        entity.ModuleId = moduleId;
                        entity.ResourceId = item;
                        entity.ScopeType = scopeType;
                        entity.SortCode = index;
                        entity.Create();
                        database.Insert(entity, isOpenTrans);
                        index++;
                    }
                }
                database.Commit();
                return Content(new JsonMessage {Success = true, Code = "1", Message = "操作成功。"}.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message}.ToString());
            }
        }

        #region 公司管理

        /// <summary>
        /// 加载公司
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// </summary>
        /// <returns></returns>
        public ActionResult ScopeCompanyList(string objectId, string category)
        {
            string ModuleId = "b29cabd8-ffb6-4d34-9d08-ee1dba2b5b6b";
            DataTable dataList = _baseDatascopepermissionbll.GetScopeCompanyList(ModuleId, objectId, category);
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dataList))
            {
                foreach (DataRow item in dataList.Rows)
                {
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dataList,
                        "parentid = '" + item["companyid"].ToString() + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = item["companyid"].ToString();
                    tree.text = item["fullname"].ToString();
                    tree.value = item["code"].ToString();
                    tree.checkstate = item["objectid"].ToString() != "" ? 1 : 0;
                    tree.showcheck = true;
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.parentId = item["parentid"].ToString();
                    if (item["parentid"].ToString() == "0")
                    {
                        tree.img = "/Content/Images/Icon16/molecule.png";
                    }
                    else
                    {
                        tree.img = "/Content/Images/Icon16/hostname.png";
                    }
                    treeList.Add(tree);
                }
            }
            else
            {
                TreeJsonEntity tree = new TreeJsonEntity();
                tree.id = "";
                tree.text = "<span style='color:red'>没有找到您要的相关数据...</span>";
                tree.value = "";
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = false;
                tree.parentId = "0";
                tree.img = "/Content/Images/Icon32/database_red.png";
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }

        #endregion

        #region 部门管理

        /// <summary>
        /// 加载部门
        /// <param name="ObjectId">对象主键</param>
        /// <param name="Category">对象分类:1-部门2-角色</param>
        /// </summary>
        /// <returns></returns>
        public ActionResult ScopeDepartmentList(string ObjectId, string Category)
        {
            string ModuleId = "e84c0fca-d912-4f5c-a25e-d5765e33b0d2";
            DataTable dataList = _baseDatascopepermissionbll.GetScopeDepartmentList(ModuleId, ObjectId, Category);
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dataList))
            {
                foreach (DataRow row in dataList.Rows)
                {
                    TreeJsonEntity tree = new TreeJsonEntity();
                    string departmentId = row["departmentid"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dataList, "parentid='" + departmentId + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    else
                    {
                        if (row["sort"].ToString() == "Company")
                        {
                            continue;
                        }
                    }
                    if (row["parentid"].ToString() != "0")
                    {
                        tree.checkstate = row["objectid"].ToString() != "" ? 1 : 0;
                        tree.showcheck = true;
                    }
                    tree.id = departmentId;
                    tree.text = row["fullname"].ToString();
                    tree.value = row["code"].ToString();
                    tree.parentId = row["parentid"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    if (row["parentid"].ToString() == "0")
                    {
                        tree.img = "/Content/Images/Icon16/molecule.png";
                    }
                    else if (row["sort"].ToString() == "Company")
                    {
                        tree.img = "/Content/Images/Icon16/hostname.png";
                    }
                    else if (row["sort"].ToString() == "Department")
                    {
                        tree.img = "/Content/Images/Icon16/chart_organisation.png";
                    }
                    treeList.Add(tree);
                }
            }
            return Content(treeList.TreeToJson());
        }

        #endregion


        #region 仓库管理

        /// <summary>
        /// 加载部门
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// </summary>
        /// <returns></returns>
        public ActionResult ScopeWarehouseList(string objectId, string category)
        {
            string moduleId = "f19b29b4-3edc-4d17-adc9-165c03020fa9";
            DataTable dataList = _baseDatascopepermissionbll.GetScopeWarehouseList(moduleId, objectId, category);
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dataList))
            {
                foreach (DataRow row in dataList.Rows)
                {
                    TreeJsonEntity tree = new TreeJsonEntity();
                    string warehouseId = row["WarehouseId"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dataList, "parentid='" + warehouseId + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    else
                    {
                        if (row["sort"].ToString() == "Company")
                        {
                            continue;
                        }
                    }
                    if (row["parentid"].ToString() != "0")
                    {
                        tree.checkstate = row["objectid"].ToString() != "" ? 1 : 0;
                        tree.showcheck = true;
                    }
                    tree.id = warehouseId;
                    tree.text = row["fullname"].ToString();
                    tree.value = row["code"].ToString();
                    tree.parentId = row["parentid"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    if (row["parentid"].ToString() == "0")
                    {
                        tree.img = "/Content/Images/Icon16/molecule.png";
                    }
                    else if (row["sort"].ToString() == "Company")
                    {
                        tree.img = "/Content/Images/Icon16/hostname.png";
                    }
                    else if (row["sort"].ToString() == "Department")
                    {
                        tree.img = "/Content/Images/Icon16/chart_organisation.png";
                    }
                    treeList.Add(tree);
                }
            }
            return Content(treeList.TreeToJson());
        }

        #endregion

        #region 角色管理

        /// <summary>
        /// 加载角色
        /// <param name="objectId">对象主键</param>
        /// <param name="category">对象分类:1-部门2-角色</param>
        /// </summary>
        /// <returns></returns>
        public ActionResult ScopeRoleList(string objectId, string category)
        {
            string moduleId = "cef74b80-24a5-4d77-9ede-bbbc75cdb431";
            DataTable dataList = _baseDatascopepermissionbll.GetScopeRoleList(moduleId, objectId, category);
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dataList))
            {
                foreach (DataRow row in dataList.Rows)
                {
                    TreeJsonEntity tree = new TreeJsonEntity();
                    string roleId = row["roleid"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dataList, "parentid='" + roleId + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    else
                    {
                        if (row["sort"].ToString() == "Company")
                        {
                            continue;
                        }
                    }
                    if (row["parentid"].ToString() != "0")
                    {
                        tree.checkstate = row["objectid"].ToString() != "" ? 1 : 0;
                        tree.showcheck = true;
                    }
                    tree.id = roleId;
                    tree.text = row["fullname"].ToString();
                    tree.value = row["code"].ToString();
                    tree.parentId = row["parentid"].ToString();
                    tree.Attribute = "Type";
                    tree.AttributeValue = row["sort"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    if (row["parentid"].ToString() == "0")
                    {
                        tree.img = "/Content/Images/Icon16/molecule.png";
                    }
                    else if (row["sort"].ToString() == "Company")
                    {
                        tree.img = "/Content/Images/Icon16/hostname.png";
                    }
                    else if (row["sort"].ToString() == "Roles")
                    {
                        tree.img = "/Content/Images/Icon16/role.png";
                    }
                    treeList.Add(tree);
                }
            }
            return Content(treeList.TreeToJson());
        }

        #endregion

        #region 用户管理

        /// <summary>
        /// 加载岗位
        /// <param name="ObjectId">对象主键</param>
        /// <param name="Category">对象分类:1-部门2-角色</param>
        /// </summary>
        /// <returns></returns>
        public ActionResult ScopeUserList(string ObjectId, string Category)
        {
            string moduleId = "58e86c4c-8022-4d30-95d5-b3d0eedcc878";
            DataTable dataList = _baseDatascopepermissionbll.GetScopeUserList(moduleId, ObjectId, Category);
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dataList))
            {
                foreach (DataRow row in dataList.Rows)
                {
                    TreeJsonEntity tree = new TreeJsonEntity();
                    string postId = row["id"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dataList, "parentid='" + postId + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    else
                    {
                        if (row["sort"].ToString() == "Company" || row["sort"].ToString() == "Department")
                        {
                            continue;
                        }
                    }
                    if (row["parentid"].ToString() != "0")
                    {
                        tree.checkstate = row["objectid"].ToString() != "" ? 1 : 0;
                        tree.showcheck = true;
                    }
                    tree.id = postId;
                    tree.text = row["fullname"].ToString();
                    tree.value = row["code"].ToString();
                    tree.parentId = row["parentid"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    if (row["parentid"].ToString() == "0")
                    {
                        tree.img = "/Content/Images/Icon16/molecule.png";
                    }
                    else if (row["sort"].ToString() == "Company")
                    {
                        tree.img = "/Content/Images/Icon16/hostname.png";
                    }
                    else if (row["sort"].ToString() == "Department")
                    {
                        tree.img = "/Content/Images/Icon16/chart_organisation.png";
                    }
                    else if (row["sort"].ToString() == "User")
                    {
                        if (row["gender"].ToString() == "男")
                        {
                            tree.img = "/Content/Images/Icon16/user_green.png";
                        }
                        else if (row["gender"].ToString() == "女")
                        {
                            tree.img = "/Content/Images/Icon16/user_female.png";
                        }
                    }
                    treeList.Add(tree);
                }
            }
            return Content(treeList.TreeToJson());
        }

        #endregion

        #endregion


        #region 查看拥有权限

        /// <summary>
        /// 查看拥有成员 返回树JSON
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public ActionResult LookObjectUserList(string objectId)
        {
            StringBuilder sb = new StringBuilder();
            List<Base_User> listData = _baseObjectuserrelationbll.GetUserList(objectId);
            if (listData.Count > 0)
            {
                foreach (Base_User item in listData)
                {
                    string Genderimg = "user_female.png";
                    if (item.Gender.ToString() == "男")
                    {
                        Genderimg = "user_green.png";
                    }
                    sb.Append("<li class=\"selected\">");
                    sb.Append("<a id=\"" + item.UserId + "\" title='工号：" + item.UserId + "\r\n账户：" + item.Account +
                              "'><img src=\"/Content/Images/Icon16/" + Genderimg + "\">" + item.RealName + "</a><i></i>");
                    sb.Append("</li>");
                }
            }
            return Content(sb.ToString());
        }

        /// <summary>
        /// 查看拥有模块权限 返回树JSON
        /// </summary>
        /// <param name="ObjectId">对象主键</param>
        /// <returns></returns>
        public ActionResult LookModulePermission(string ObjectId)
        {
            //如果ObjectId为空。自动获取当前登录用户拥有权限
            if (string.IsNullOrEmpty(ObjectId))
            {
                ObjectId = ManageProvider.Provider.Current().ObjectId;
            }
            DataTable dt = _baseModulepermissionbll.GetModulePermission(ObjectId);
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dt))
            {
                foreach (DataRow item in dt.Rows)
                {
                    string ModuleId = item["moduleid"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "ParentId = '" + ModuleId + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = item["moduleid"].ToString();
                    tree.text = item["fullname"].ToString();
                    tree.value = item["moduleid"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.parentId = item["parentid"].ToString();
                    tree.img = item["icon"].ToString() != null
                        ? "/Content/Images/Icon16/" + item["icon"].ToString()
                        : item["icon"].ToString();
                    TreeList.Add(tree);
                }
            }
            return Content(TreeList.TreeToJson());
        }

        /// <summary>
        /// 查看拥有按钮权限 返回树JSON
        /// </summary>
        /// <param name="ObjectId">对象主键</param>
        /// <returns></returns>
        public ActionResult LookButtonePermission(string ObjectId)
        {
            //如果ObjectId为空。自动获取当前登录用户拥有权限
            if (string.IsNullOrEmpty(ObjectId))
            {
                ObjectId = ManageProvider.Provider.Current().ObjectId;
            }
            DataTable dt = _baseButtonpermissionbll.GetButtonePermission(ObjectId);
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dt))
            {
                foreach (DataRow item in dt.Rows)
                {
                    string text = "";
                    if (item["Sort"].ToString() == "按钮")
                    {
                        if (item["Category"].ToString() == "1")
                        {
                            text = item["fullname"].ToString() + "（工具栏）";
                        }
                        else if (item["Category"].ToString() == "2")
                        {
                            text = item["fullname"].ToString() + "（右击栏）";
                        }
                    }
                    else
                    {
                        text = item["fullname"].ToString();
                    }
                    string id = item["id"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "ParentId = '" + id + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = item["id"].ToString();
                    tree.text = text;
                    tree.value = item["id"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.parentId = item["parentid"].ToString();
                    tree.img = item["icon"].ToString() != string.Empty
                        ? "/Content/Images/Icon16/" + item["icon"].ToString()
                        : item["icon"].ToString();
                    TreeList.Add(tree);
                }
            }
            return Content(TreeList.TreeToJson());
        }

        /// <summary>
        /// 查看拥有视图权限 返回树JSON
        /// </summary>
        /// <param name="objectId">对象主键</param>
        /// <returns></returns>
        public ActionResult LookViewPermission(string objectId)
        {
            //如果ObjectId为空。自动获取当前登录用户拥有权限
            if (string.IsNullOrEmpty(objectId))
            {
                objectId = ManageProvider.Provider.Current().ObjectId;
            }
            DataTable dt = _baseViewpermissionbll.GetViewPermission(objectId);
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();
            if (!DataHelper.IsExistRows(dt))
            {
                foreach (DataRow item in dt.Rows)
                {
                    string id = item["id"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "ParentId = '" + id + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = item["id"].ToString();
                    tree.text = item["fullname"].ToString();
                    tree.value = item["id"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.parentId = item["parentid"].ToString();
                    tree.img = item["icon"].ToString() != ""
                        ? "/Content/Images/Icon16/" + item["icon"].ToString()
                        : "/Content/Images/Icon16/tag_blue.png";
                    treeList.Add(tree);
                }
            }
            return Content(treeList.TreeToJson());
        }

        #endregion
    }
}