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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AnJie.ERP.Service;

namespace AnJie.ERP.WebApp.Areas.ProductModule.Controllers
{
    /// <summary>
    /// ��Ʒ��Ϣ�������
    /// </summary>
    public class ProductController : PublicController<ProductEntity>
    {
        private readonly ProductBLL _productBLL = new ProductBLL();
        private readonly ProductMerchantIdBLL _productMerchantIdBLL = new ProductMerchantIdBLL();

        private readonly ProductService _productService = new ProductService();

        /// <summary>
        /// ��ѯǰ��50����Ʒ��Ϣ������JSON��
        /// </summary>
        /// <param name="keywords">��ѯ�ؼ���</param>
        /// <returns></returns>
        public ActionResult AutoComplete(string keywords)
        {
            DataTable ListData = _productBLL.OptionUserList(keywords);
            return Content(ListData.ToJson());
        }

        /// <summary>
        /// ������Ʒ�б�JSON
        /// </summary>
        /// <param name="keywords">��ѯ�ؼ���</param>
        /// <param name="categoryId">����ID</param>
        /// <param name="jqgridparam">������</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(string keywords, string categoryId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable listData = _productBLL.GetPageList(keywords, categoryId, ref jqgridparam);
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

        /// <summary>
        /// �����̼���Ʒ
        /// </summary>
        /// <param name="merchantId">�̻�Id</param>
        /// <param name="keyword">�ؼ���</param>
        /// <returns></returns>
        public ActionResult GetMerchantProductList(string merchantId, string keyword)
        {
            var listData = _productMerchantIdBLL.GetProductMerchantIdList(20, string.Empty, HttpUtility.UrlDecode(keyword));
            var errMsg = string.Empty;
            var returnList = listData;
            if (listData.Count() == 0)
            {
                errMsg = "��ѯ������Ʒ";
            }
            else
            {
                var selectList = listData.Where(i => i.MerchantId == HttpUtility.UrlDecode(merchantId)).ToList();
                returnList = selectList;
                if (selectList.Count() == 0)
                {
                    errMsg = string.Format("��Ʒ[{0}]���ǵ�ǰ�̻����У����飡", HttpUtility.UrlDecode(keyword));
                }
            }

            var jsonData = new
            {
                errMsg = errMsg,
                rows = returnList,
            };
            return Content(jsonData.ToJson());
        }

        /// <summary>
        /// �ύ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="product">��Ʒ��Ϣ</param>
        /// <param name="buildFormJson">�Զ����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitProductForm(string keyValue, ProductEntity product, string buildFormJson)
        {
            string moduleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string message = keyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                if (!string.IsNullOrEmpty(keyValue))
                {
                    if (keyValue == ManageProvider.Provider.Current().UserId)
                    {
                        throw new Exception("��Ȩ�ޱ༭������Ϣ");
                    }
                    product.Modify(keyValue);
                    database.Update(product, isOpenTrans);
                }
                else
                {
                    product.Create();
                    product.SortCode = CommonHelper.GetInt(BaseFactory.BaseHelper().GetSortCode<ProductEntity>("SortCode"));
                    database.Insert(product, isOpenTrans);
                    BaseDataScopePermissionBLL.Instance.AddScopeDefault(moduleId, ManageProvider.Provider.Current().UserId, product.ProductId, isOpenTrans);
                }
                Base_FormAttributeBll.Instance.SaveBuildForm(buildFormJson, product.ProductId, moduleId, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// ��ȡ��Ʒ��Ϣ���󷵻�JSON
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetProductForm(string keyValue)
        {
            ProductEntity product = DataFactory.Database().FindEntity<ProductEntity>(keyValue);
            if (product == null)
            {
                return Content("");
            }
            string strJson = product.ToJson();
            strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(keyValue));
            return Content(strJson);
        }

        #region ��Ʒ����
        /// <summary>
        /// ��Ʒ���뵯����ҳ��
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ImportProduct()
        {
            return View();
        }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        public ActionResult SubmitImportProduct()
        {
            bool isSuccess = false;//����״̬
            string errMessage;
            var dataResult = new DataTable();//��������¼��
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["filePath"];//��ȡ�ϴ����ļ�
                if (file != null && file.FileName != "")
                {
                    string fullname = file.FileName;
                    var extension = System.IO.Path.GetExtension(fullname);
                    if (extension != null)
                    {
                        string fileType = extension.ToLower();
                        if (fileType == ".xls" || fileType == ".xlsx")
                        {
                            string fileId = Guid.NewGuid().ToString();
                            string filename = fileId + fileType;

                            bool flag = UploadHelper.FileUpload(file, Server.MapPath("~/UploadFile/ImportProduct/"),
                                filename, out errMessage);
                            if (flag)
                            {
                                ProductImportFileEntity importFile = new ProductImportFileEntity();
                                importFile.Create();
                                importFile.FileId = fileId;
                                importFile.FileName = fullname;

                                DataTable dt = ImportExcel.ImportExcelFile(Server.MapPath("~/UploadFile/ImportProduct/") + filename);
                                flag = _productService.ImportProduct(importFile, dt, out dataResult, out errMessage);
                                if (flag)
                                {
                                    isSuccess = true;
                                }
                            }
                            else
                            {
                                throw new Exception("��Ʒ����ʧ�ܣ�" + errMessage);
                            }
                        }
                        else
                        {
                            throw new Exception("��Ʒ����ʧ�ܣ��ļ���ʽ����ȷ");
                        }
                    }
                    else
                    {
                        throw new Exception("��Ʒ����ʧ�ܣ��ļ���ʽ����ȷ");
                    }
                }
                else
                {
                    throw new Exception("��ѡ���ϴ��ļ�");
                }
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "�쳣����" + ex.Message);
                errMessage = ex.Message;
                isSuccess = false;
            }

            if (dataResult.Rows.Count > 0)
            {
                isSuccess = false;
            }
            var data = new
            {
                status = isSuccess ? "true" : "false",
                result = dataResult,
                message = errMessage
            };
            return Content(data.ToJson());
        }
        #endregion

        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ProductMerchant()
        {
            return View();
        }

        /// <summary>
        /// ������Ʒ�̻��б�
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult ProductMerchantList(string productId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = _productBLL.ProductMerchantList(productId);
            foreach (DataRow dr in dt.Rows)
            {
                string strchecked = "";
                if (!string.IsNullOrEmpty(dr["RelationId"].ToString()))//�ж��Ƿ�ѡ��
                {
                    strchecked = "selected";
                }
                sb.Append("<li title=\"" + dr["MerchantName"] + "(" + dr["MerchantCode"] + ")" + "\" class=\"" + strchecked + "\">");
                sb.Append("<a id=\"" + dr["MerchantId"] + "\"><img src=\"../../Content/Images/Icon16/role.png \">" + dr["MerchantName"] + "</a><i></i>");
                sb.Append("</li>");
            }
            return Content(sb.ToString());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public ActionResult ProductMerchantSubmit(string productId, string objectId)
        {
            try
            {
                string[] array = objectId.Split(',');
                int isOk = _productBLL.SetProductMerchant(productId, array);
                return Content(new JsonMessage { Success = true, Code = isOk.ToString(), Message = "�����ɹ���" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�����" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// �����̼���Ʒ(�����̻���ѯ)
        /// </summary>
        /// <param name="merchantId">�̻�Id</param>
        /// <param name="keyword">�ؼ���</param>
        /// <returns></returns>
        public ActionResult GetProductMerchantIdList(string merchantId, string keyword)
        {
            var listData = _productMerchantIdBLL.GetProductMerchantIdList(20,string.Empty, HttpUtility.UrlDecode(keyword));
            var errMsg = string.Empty;
            var returnList = listData;
            if (listData.Count() == 0)
            {
                errMsg = "��ѯ������Ʒ";
            }
            else
            {
                var selectList = listData.Where(i => i.MerchantId == HttpUtility.UrlDecode(merchantId)).ToList();
                returnList = selectList;
                if (selectList.Count() == 0)
                {
                    errMsg = string.Format("��Ʒ[{0}]���ǵ�ǰ�̻����У����飡", HttpUtility.UrlDecode(keyword));
                }
            }

            var jsonData = new
            {
                errMsg = errMsg,
                rows = returnList,
            };
            return Content(jsonData.ToJson());
        }

        /// <summary>
        /// �̻������б�Json
        /// </summary>
        /// <returns></returns>
        public ActionResult MerchantListJson(string productId)
        {
            DataTable listData = _productBLL.ProductMerchantList(productId);
            return Content(listData.ToJson());
        }

        /// <summary>
        /// �ֿⷵ���б�Json
        /// </summary>
        /// <returns></returns>
        public ActionResult WarehouseListJson(string productId, string merchantId)
        {
            DataTable listData = _productBLL.ProductWarehouseList(productId, merchantId);
            return Content(listData.ToJson());
        }

        /// <summary>
        /// ��λ�����б�Json
        /// </summary>
        /// <returns></returns>
        public ActionResult ZoneListJson(string productId, string merchantId, string warehouseId)
        {
            DataTable listData = _productBLL.ProductZoneList(productId, merchantId, warehouseId);
            return Content(listData.ToJson());
        }

        /// <summary>
        /// ��λ�����б�Json
        /// </summary>
        /// <returns></returns>
        public ActionResult LocationListJson(string productId, string merchantId, string warehouseId, string zoneId)
        {
            DataTable listData = _productBLL.ProductLocationList(productId, merchantId, warehouseId, zoneId);
            return Content(listData.ToJson());
        }

        /// <summary>
        /// ��Ʒ��λ�б�Json
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="merchantId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="zoneId"></param>
        /// <param name="locationId"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetProductLocationPageList(string productId, string merchantId, string warehouseId, string zoneId, string locationId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable listData = _productBLL.GetProductLocationPageList(productId, merchantId, warehouseId, zoneId, locationId, ref jqgridparam);
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

        /// <summary>
        /// ������Ʒ����
        /// </summary>
        /// <param name="productLocation"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveAddLocation(ProductLocationEntity productLocation)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string message = "�����ɹ���";

                bool flag = _productBLL.IsExistProductLocation(productLocation.ProductId, productLocation.MerchantId, productLocation.WarehouseId, productLocation.ZoneId);
                if (flag)
                {
                    throw new Exception("ͬһ���̻�+�ֿ�+����ֻ�ܴ���һ��Ĭ�ϴ�λ����ɾ���Ѵ��ڴ�λ����������");
                }

                productLocation.Create();
                database.Insert(productLocation, isOpenTrans);
                database.Commit();

                return Content(new JsonMessage { Success = true, Code = "1", Message = message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }

        public ActionResult DeleteLocation(string productLocationID)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string message = "ɾ���ɹ���";
                ProductLocationEntity productLoacation = database.FindEntity<ProductLocationEntity>("ProductLocationID", productLocationID);
                if (productLoacation == null)
                {
                    throw new Exception("��ȡ��λ����ʧ�ܣ���ˢ�����ԣ�");
                }
                database.Delete<ProductLocationEntity>("ProductLocationID", productLocationID, isOpenTrans);

                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
    }
}