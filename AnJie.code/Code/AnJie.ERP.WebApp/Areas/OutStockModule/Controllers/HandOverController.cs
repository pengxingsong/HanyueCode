using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Service;
using AnJie.ERP.Utilities;

namespace AnJie.ERP.WebApp.Areas.OutStockModule.Controllers
{
    public class HandOverController : Controller
    {
        private readonly HandoverBLL _handoverBll = new HandoverBLL();

        private readonly HandoverService _handoverService = new HandoverService();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取交接单明细
        /// </summary>
        /// <param name="shipTypeId">物流方式</param>
        /// <returns></returns>
        public ActionResult GetHandOverItemList(string shipTypeId)
        {
            try
            {
                List<HandoverItemEntity> items;
                string createUserId = ManageProvider.Provider.Current().UserId;
                HandoverEntity handover = _handoverBll.GetUnPrintHandoverByShipType(shipTypeId, createUserId);
                if (handover != null)
                {
                    items = _handoverBll.GetHandOverItemList(handover.HandoverId);
                }
                else
                {
                    items = new List<HandoverItemEntity>();
                }
                var jsonData = new
                {
                    rows = items
                };
                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipTypeId"></param>
        /// <param name="expressNum"></param>
        /// <returns></returns>
        public ActionResult ScanExpressNum(string shipTypeId, string expressNum)
        {
            try
            {
                string createUserId = ManageProvider.Provider.Current().UserId;
                var handoverService = new HandoverService();
                string message;
                bool flag = handoverService.ScanExpressNum(shipTypeId, expressNum, createUserId, out message);
                if (!flag)
                {
                    return Content(new JsonMessage {Success = true, Code = "1", Message = message}.ToString());
                }
                return Content(new JsonMessage {Success = true, Code = "2", Message = message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage {Success = false, Code = "-1", Message = "操作失败：" + ex.Message}.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipTypeId"></param>
        /// <returns></returns>
        public ActionResult CloseHandover(string shipTypeId)
        {
            try
            {
                string createUserId = ManageProvider.Provider.Current().UserId;
                var handoverService = new HandoverService();
                string message;
                bool flag = handoverService.CloseHandover(shipTypeId, createUserId, out message);
                if (!flag)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = message }.ToString());
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectPrinter()
        {
            return View();
        }

        /// <summary>
        /// 打印物流单
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintHandover()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handoverId"></param>
        /// <returns></returns>
        public ActionResult GetPrintContent(string handoverId)
        {
            var itemList = _handoverBll.GetHandOverItemList(handoverId);
            var handoverEntity = _handoverBll.GetEntity(handoverId);
            string templateContent = _handoverService.GetPrintTemplateContent(handoverEntity, itemList);
           

            StringBuilder sb = new StringBuilder();

            sb.Append("LODOP.PRINT_INIT(\"快递交接单\");");
            sb.Append("LODOP.ADD_PRINT_RECT(70, 27, 634, 242, 0, 1);");
            sb.Append("LODOP.ADD_PRINT_TEXT(29, 236, 279, 38, \"交接明细\");");
            sb.Append("LODOP.SET_PRINT_STYLEA(2, \"FontSize\", 18);");
            sb.Append("LODOP.SET_PRINT_STYLEA(2, \"Bold\", 1);");
            //sb.Append(string.Format("LODOP.ADD_PRINT_HTM(88, 40, 321, 185, \"{0}\");", templateContent.Replace("\"","'")));
            sb.Append(string.Format("LODOP.ADD_PRINT_HTM(88, 40, 321, 185, \"{0}\");", "测试模板内容"));
            var jsonData = new
            {
                TemplateContent = sb.ToString()
            };
            return Content(jsonData.ToJson());
        }
    }
}