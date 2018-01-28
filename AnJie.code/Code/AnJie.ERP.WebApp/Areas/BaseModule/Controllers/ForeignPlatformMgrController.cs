using AnJie.ERP.Entity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AnJie.ERP.Business;
using AnJie.ERP.Utilities;
using System.Diagnostics;

namespace AnJie.ERP.WebApp.Areas.BaseModule.Controllers
{
    public class ForeignPlatformMgrController : PublicController<PlatformEntity>
    {
        //
        // GET: /BaseModule/ForeignPlatformMgr/

        #region BLL

        private readonly PlatformMgrBLL _platformMgrBLL = new PlatformMgrBLL();

        private readonly ForeignSysApiMgrBLL _foreignSysApiMgrBLL = new ForeignSysApiMgrBLL();

        private readonly PlatformMerchantsAuthBLL _platformMerchantsAuthBLL = new PlatformMerchantsAuthBLL();

        #endregion

        #region 视图

        //平台列表界面视图
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult PlatformIndexView()
        {
            return View();
        }

        //平台信息编辑视图
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult PlatformFormView()
        {
            return View();
        }

        //平台api设置视图
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult PlatformAPISettingFormView()
        {
            return View();
        }

        //平台商户授权视图
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult PlatformMerchantsAuthFormView()
        {
            return View();
        }

        #endregion

        #region 业务处理

        #region 三方平台设置--业务处理

        /// <summary>
        /// 返回配送列表列表JSON
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult GridPageListJson(JqGridParam jqgridparam)
        {
            Stopwatch watch = CommonHelper.TimerStart();
            List<PlatformEntity> listData = _platformMgrBLL.GetPageList(ref jqgridparam);
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
        /// 保存平台数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="parameterJson"></param>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult SubmitPlatformForm(string keyValue, PlatformEntity entity, string parameterJson)
        {
            try
            {
                string message = keyValue == "" ? "新增成功。" : "编辑成功。";
                int isOk = _platformMgrBLL.SubmitPlatformForm(keyValue, entity, parameterJson);
                return Content(new JsonMessage { Success = true, Code = isOk.ToString(), Message = message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 检查平台Code唯一性
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult CheckCodeUnique(string code)
        {
            try
            {
                int result = _platformMgrBLL.CheckCodeUnique(code);
                bool IsSuccess = result <= 0;
                return Content(new JsonMessage { Success = IsSuccess, Code = IsSuccess ? "1" : "0", Message = !IsSuccess ? "平台编码已存在" : "" }.ToString());

            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        #endregion

        #region 三方平台API设置--业务处理

        /// <summary>
        /// 保存平台api设置数据
        /// </summary>
        /// <param name="platformID"></param>
        /// <param name="platformName"></param>
        /// <param name="dataJson"></param>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult SaveForeignSysApiDataList(string platformID, string platformName, string dataJson)
        {
            try
            {
                var list = dataJson.JonsToList<ForeignSysApiEntity>();
                list.ForEach((ent) =>
                {
                    ent.PlatformID = platformID;
                    ent.PlatformName = platformName;
                });
                var result = _foreignSysApiMgrBLL.SaveEntity(list);
                return Content(new JsonMessage { Success = result, Code = result ? "1" : "0", Message = result ? "保存成功!" : "保存失败!" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 根据平台ID获取设置的api列表数据
        /// </summary>
        /// <param name="platformID"></param>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult GetForeignSysApiDataList(string platformID)
        {
            try
            {
                var jsonData = new
                {
                    rows = _foreignSysApiMgrBLL.GetList_ByPlatformID(platformID),
                };
                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }


        #endregion

        #region 三方平添商户授权设置--业务处理

        /// <summary>
        /// 保存平台商户授权设置数据
        /// </summary>
        /// <param name="platformID"></param>
        /// <param name="platformName"></param>
        /// <param name="dataJson"></param>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult SavePlatformMerchantsAuthDataList(string platformID, string platformName, string dataJson)
        {
            try
            {
                var list = dataJson.JonsToList<PlatformMerchantsAuthEntity>();
                list.ForEach((ent) =>
                {
                    ent.PlatformID = platformID;
                    ent.PlatformName = platformName;
                });
                var result = _platformMerchantsAuthBLL.SaveEntity(list);
                return Content(new JsonMessage { Success = result, Code = result ? "1" : "0", Message = result ? "保存成功!" : "保存失败!" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        /// <summary>
        /// 根据平台ID获取平台商户授权列表数据
        /// </summary>
        /// <param name="platformID"></param>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult GetPlatformMerchantsAuthDataList(string platformID)
        {
            try
            {
                var jsonData = new
                {
                    rows = _platformMerchantsAuthBLL.GetList_ByPlatformID(platformID),
                };
                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #endregion

    }
}
