using AnJie.ERP.Business;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AnJie.ERP.ViewModel.BaseModule;
using System.Text.RegularExpressions;

namespace AnJie.ERP.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// 面单模板表控制器
    /// </summary>
    public class ShipTypeTemplateController : PublicController<ShipTypeTemplateEntity>
    {

        private readonly ShipTypeTemplateBLL _templateBLL = new ShipTypeTemplateBLL();

        /// <summary>
        /// 分类管理返回表格Json
        /// </summary>
        /// <param name="shipTypeId">物流方式ID</param>
        /// <returns></returns>
        public ActionResult GridListJson(string shipTypeId)
        {
            var listData = _templateBLL.GetList(shipTypeId);
            var jsonData = new
            {
                rows = listData,
            };
            return Content(jsonData.ToJson());
        }

        /// <summary>
        /// 分类管理返回列表Json
        /// </summary>
        /// <param name="shipTypeId">物流方式Id</param>
        /// <returns></returns>
        public ActionResult ListJson(string shipTypeId)
        {
            List<ShipTypeTemplateViewModel> listData = _templateBLL.GetList(shipTypeId);
            return Content(listData.ToJson());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipTypeId"></param>
        /// <returns></returns>
        public ActionResult ValidTemplateList(string shipTypeId)
        {
            List<ShipTypeTemplateViewModel> listData = _templateBLL.GetList(shipTypeId, true);
            return Content(listData.ToJson());
        }

        #region 背景图上传

        /// <summary>
        /// 背景图上传
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadImage()
        {
            return View();
        }

        /// <summary>
        /// 提交上传
        /// </summary>
        /// <param name="templateId">模板主键</param>
        /// <param name="filedata">附件对象</param>
        /// <returns></returns>
        public ActionResult SubmitUploadImage(string templateId, HttpPostedFileBase filedata)
        {
            try
            {
                ShipTypeTemplateEntity template = _templateBLL.GetTemplate(templateId);
                if (template.IsElectronicBill == 1)
                {
                    var result = new
                    {
                        Status = 0,
                        Message = "电子面单不需要上传背景图"
                    };
                    return Content(result.ToJson());
                }
                //没有文件上传，直接返回
                if (string.IsNullOrEmpty(filedata?.FileName) || filedata.ContentLength == 0)
                {
                    return HttpNotFound();
                }

                //获取文件完整文件名(包含绝对路径)
                //文件存放路径格式：/Resource/ShipTypeTemplate/{TemplateId}.{后缀名}
                string fileEextension = Path.GetExtension(filedata.FileName);
                if (fileEextension != ".jpg")
                {
                    var result = new
                    {
                        Status = 0,
                        Message = "文件类型不正确"
                    };
                    return Content(result.ToJson());
                }

                string virtualPath = string.Format("~/Resource/ShipTypeTemplate/{0}{1}", templateId, fileEextension);
                string fullFileName = this.Server.MapPath(virtualPath);
                //创建文件夹，保存文件
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);

                if (System.IO.File.Exists(fullFileName))
                {
                    System.IO.File.Delete(fullFileName);
                }
                if (!System.IO.File.Exists(fullFileName))
                {
                    filedata.SaveAs(fullFileName);
                }
                var jsonData = new
                {
                    Status = 1,
                    Message = "上传成功",
                    FileName = virtualPath
                };
                _templateBLL.UpdateTemplateImage(templateId, string.Format("/Resource/ShipTypeTemplate/{0}{1}", templateId, fileEextension));
                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                var jsonData = new
                {
                    Status = 0,
                    ex.Message
                };
                return Content(jsonData.ToJson());
            }
        }

        #endregion

        /// <summary>
        /// 模板设计
        /// </summary>
        /// <returns></returns>
        public ActionResult Design()
        {
            return View();
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public ActionResult Preview()
        {
            return View();
        }

        /// <summary>
        /// 单据打印项
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintItemTree(string templateId)
        {
            var senderItems = new Dictionary<string, string>
            {
                {"$SenderName", "发件人姓名"},
                {"$SenderPhone", "发件人电话"},
                {"$SenderCellPhone", "发件人手机号"},
                {"$MallName", "店铺名称"},
                {"$SenderProvince", "发件人-省"},
                {"$SenderCity", "发件人-市"},
                {"$SenderContry", "发件人-区"},
                {"$SenderAddress", "发件人地址"},
                {"$SenderZipCode", "发件人邮编"}
            };

            var receiverItems = new Dictionary<string, string>
            {
                {"$ReceiverName", "收件人姓名"},
                {"$ReceiverPhone", "收件人电话"},
                {"$ReceiverCellPhone", "收件人手机号"},
                {"$ReceiverProvince", "收件人-省"},
                {"$ReceiverCity", "收件人-市"},
                {"$ReceiverContry", "收件人-区"},
                {"$ReceiverAddress", "收件人地址"},
                {"$ReceiverZipCode", "收件人邮编"}
            };

            var orderItems = new Dictionary<string, string>
            {
                {"$OrderNo", "订单编号"},
                {"$OrderItmes", "物品数量"},
                {"$Remark", "派件备注"},
                {"$Select", "√"},
                {"$CustomContent", "自定义内容"}
            };

            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();

            TreeJsonEntity senderNode = new TreeJsonEntity
            {
                id = "Sender",
                text = "发件人信息",
                value = "",
                parentId = "0",
                isexpand = true,
                complete = true,
                hasChildren = true,
                showcheck = false,
                img = "/Content/Images/Icon16/basket_shopping.png"
            };
            treeList.Add(senderNode);

            treeList.AddRange(senderItems.Select(entity => new TreeJsonEntity
            {
                id = entity.Key,
                text = entity.Value,
                value = entity.Value,
                parentId = "Sender",
                isexpand = true,
                showcheck = true,
                complete = true,
                hasChildren = false,
                img = "/Content/Images/Icon16/basket_shopping.png"
            }));

            TreeJsonEntity receiverNode = new TreeJsonEntity
            {
                id = "Receiver",
                text = "收件人信息",
                value = "",
                parentId = "0",
                isexpand = true,
                complete = true,
                hasChildren = true,
                showcheck = false,
                img = "/Content/Images/Icon16/basket_shopping.png"
            };
            treeList.Add(receiverNode);

            treeList.AddRange(receiverItems.Select(entity => new TreeJsonEntity
            {
                id = entity.Key,
                text = entity.Value,
                value = entity.Value,
                parentId = "Receiver",
                isexpand = true,
                showcheck = true,
                complete = true,
                hasChildren = false,
                img = "/Content/Images/Icon16/basket_shopping.png"
            }));

            var orderNode = new TreeJsonEntity
            {
                id = "Order",
                text = "订单信息",
                value = "",
                parentId = "0",
                isexpand = true,
                complete = true,
                hasChildren = true,
                showcheck = false,
                img = "/Content/Images/Icon16/basket_shopping.png"
            };
            treeList.Add(orderNode);

            treeList.AddRange(orderItems.Select(entity => new TreeJsonEntity
            {
                id = entity.Key,
                text = entity.Value,
                value = entity.Value,
                parentId = "Order",
                isexpand = true,
                showcheck = true,
                complete = true,
                hasChildren = false,
                img = "/Content/Images/Icon16/basket_shopping.png"
            }));

            ShipTypeTemplateEntity template = _templateBLL.GetTemplate(templateId);
            if (template != null)
            {
                foreach (TreeJsonEntity treeJsonEntity in treeList)
                {
                    if (template.TemplateContent.IndexOf(treeJsonEntity.id, StringComparison.InvariantCultureIgnoreCase) != -1)
                    {
                        treeJsonEntity.checkstate = 1;
                    }
                }
            }
            return Content(treeList.TreeToJson());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="templateContent"></param>
        /// <returns></returns>
        public ActionResult SubmitTemplateContent(string templateId, string templateContent)
        {
            try
            {
                templateContent = HttpUtility.UrlDecode(templateContent);
                var Message = "保存模板成功";
                int IsOk = 1;
                _templateBLL.UpdateTemplateContent(templateId, templateContent);
                WriteLog(IsOk, templateId, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, templateId, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public ActionResult GetTemplate(string templateId)
        {
            ShipTypeTemplateEntity template = _templateBLL.GetTemplate(templateId);
            if (string.IsNullOrWhiteSpace(template.TemplateContent))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("LODOP.PRINT_INITA(0, 0, \"{0}mm\", \"{1}mm\", \"物流面单打印{2}\");\r\n", template.Width, template.Height, template.TemplateId);
                sb.AppendFormat("LODOP.SET_PRINT_PAGESIZE(1, \"{0}mm\", \"{1}mm\", \"{2}\");\r\n", template.Width, template.Height, "");
                sb.AppendLine("LODOP.SET_PRINT_MODE(\"POS_BASEON_PAPER\", true);");
                if (template.IsElectronicBill == 0)
                {
                    sb.AppendFormat("LODOP.ADD_PRINT_SETUP_BKIMG(\"<img border='0' src='{0}'>\");\r\n", template.BackgroundImage);
                }
                sb.AppendFormat("LODOP.SET_SHOW_MODE(\"BKIMG_WIDTH\", \"{0}mm\");\r\n", template.Width);
                sb.AppendFormat("LODOP.SET_SHOW_MODE(\"BKIMG_HEIGHT\",\"{0}mm\");\r\n", template.Height);
                template.TemplateContent = sb.ToString();
            }
            else
            {
                Match match = Regex.Match(template.TemplateContent, @"LODOP\.PRINT_INITA.+?\);\r\n");
                if (match.Success)
                {
                    template.TemplateContent = template.TemplateContent.Replace(match.Groups[0].Value, "");
                    template.TemplateContent = string.Format("LODOP.PRINT_INITA(0,0,\"{0}mm\",\"{1}mm\",\"物流面单打印{2}\");\r\n", template.Width, template.Height, template.TemplateId) + template.TemplateContent;
                }
            }
            return Content(template.ToJson());
        }
    }
}