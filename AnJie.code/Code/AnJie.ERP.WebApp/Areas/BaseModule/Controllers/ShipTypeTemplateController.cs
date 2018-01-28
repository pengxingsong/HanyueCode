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
    /// �浥ģ��������
    /// </summary>
    public class ShipTypeTemplateController : PublicController<ShipTypeTemplateEntity>
    {

        private readonly ShipTypeTemplateBLL _templateBLL = new ShipTypeTemplateBLL();

        /// <summary>
        /// ��������ر��Json
        /// </summary>
        /// <param name="shipTypeId">������ʽID</param>
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
        /// ����������б�Json
        /// </summary>
        /// <param name="shipTypeId">������ʽId</param>
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

        #region ����ͼ�ϴ�

        /// <summary>
        /// ����ͼ�ϴ�
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadImage()
        {
            return View();
        }

        /// <summary>
        /// �ύ�ϴ�
        /// </summary>
        /// <param name="templateId">ģ������</param>
        /// <param name="filedata">��������</param>
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
                        Message = "�����浥����Ҫ�ϴ�����ͼ"
                    };
                    return Content(result.ToJson());
                }
                //û���ļ��ϴ���ֱ�ӷ���
                if (string.IsNullOrEmpty(filedata?.FileName) || filedata.ContentLength == 0)
                {
                    return HttpNotFound();
                }

                //��ȡ�ļ������ļ���(��������·��)
                //�ļ����·����ʽ��/Resource/ShipTypeTemplate/{TemplateId}.{��׺��}
                string fileEextension = Path.GetExtension(filedata.FileName);
                if (fileEextension != ".jpg")
                {
                    var result = new
                    {
                        Status = 0,
                        Message = "�ļ����Ͳ���ȷ"
                    };
                    return Content(result.ToJson());
                }

                string virtualPath = string.Format("~/Resource/ShipTypeTemplate/{0}{1}", templateId, fileEextension);
                string fullFileName = this.Server.MapPath(virtualPath);
                //�����ļ��У������ļ�
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
                    Message = "�ϴ��ɹ�",
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
        /// ģ�����
        /// </summary>
        /// <returns></returns>
        public ActionResult Design()
        {
            return View();
        }

        /// <summary>
        /// ��ӡԤ��
        /// </summary>
        /// <returns></returns>
        public ActionResult Preview()
        {
            return View();
        }

        /// <summary>
        /// ���ݴ�ӡ��
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintItemTree(string templateId)
        {
            var senderItems = new Dictionary<string, string>
            {
                {"$SenderName", "����������"},
                {"$SenderPhone", "�����˵绰"},
                {"$SenderCellPhone", "�������ֻ���"},
                {"$MallName", "��������"},
                {"$SenderProvince", "������-ʡ"},
                {"$SenderCity", "������-��"},
                {"$SenderContry", "������-��"},
                {"$SenderAddress", "�����˵�ַ"},
                {"$SenderZipCode", "�������ʱ�"}
            };

            var receiverItems = new Dictionary<string, string>
            {
                {"$ReceiverName", "�ռ�������"},
                {"$ReceiverPhone", "�ռ��˵绰"},
                {"$ReceiverCellPhone", "�ռ����ֻ���"},
                {"$ReceiverProvince", "�ռ���-ʡ"},
                {"$ReceiverCity", "�ռ���-��"},
                {"$ReceiverContry", "�ռ���-��"},
                {"$ReceiverAddress", "�ռ��˵�ַ"},
                {"$ReceiverZipCode", "�ռ����ʱ�"}
            };

            var orderItems = new Dictionary<string, string>
            {
                {"$OrderNo", "�������"},
                {"$OrderItmes", "��Ʒ����"},
                {"$Remark", "�ɼ���ע"},
                {"$Select", "��"},
                {"$CustomContent", "�Զ�������"}
            };

            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();

            TreeJsonEntity senderNode = new TreeJsonEntity
            {
                id = "Sender",
                text = "��������Ϣ",
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
                text = "�ռ�����Ϣ",
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
                text = "������Ϣ",
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
                var Message = "����ģ��ɹ�";
                int IsOk = 1;
                _templateBLL.UpdateTemplateContent(templateId, templateContent);
                WriteLog(IsOk, templateId, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, templateId, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
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
                sb.AppendFormat("LODOP.PRINT_INITA(0, 0, \"{0}mm\", \"{1}mm\", \"�����浥��ӡ{2}\");\r\n", template.Width, template.Height, template.TemplateId);
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
                    template.TemplateContent = string.Format("LODOP.PRINT_INITA(0,0,\"{0}mm\",\"{1}mm\",\"�����浥��ӡ{2}\");\r\n", template.Width, template.Height, template.TemplateId) + template.TemplateContent;
                }
            }
            return Content(template.ToJson());
        }
    }
}