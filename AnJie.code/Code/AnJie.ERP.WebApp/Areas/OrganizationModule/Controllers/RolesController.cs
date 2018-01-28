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

namespace AnJie.ERP.WebApp.Areas.OrganizationModule.Controllers
{
    /// <summary>
    /// ��ɫ���������
    /// </summary>
    public class RolesController : PublicController<Base_Roles>
    {
        readonly BaseRolesBll _baseRolesbll = new BaseRolesBll();

        /// <summary>
        /// ����ɫ���������б�JONS
        /// </summary>
        /// <param name="companyId">��˾ID</param>
        /// <param name="jqgridparam">JqGrid������</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(string companyId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable listData = _baseRolesbll.GetPageList(companyId, ref jqgridparam);
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
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }
    }
}