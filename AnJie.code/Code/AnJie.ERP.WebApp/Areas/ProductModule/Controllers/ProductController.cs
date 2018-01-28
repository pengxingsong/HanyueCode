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
    /// 商品信息表控制器
    /// </summary>
    public class ProductController : PublicController<ProductEntity>
    {
        private readonly ProductBLL _productBLL = new ProductBLL();
        private readonly ProductMerchantIdBLL _productMerchantIdBLL = new ProductMerchantIdBLL();

        private readonly ProductService _productService = new ProductService();

        /// <summary>
        /// 查询前面50条商品信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult AutoComplete(string keywords)
        {
            DataTable ListData = _productBLL.OptionUserList(keywords);
            return Content(ListData.ToJson());
        }

        /// <summary>
        /// 返回商品列表JSON
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <param name="categoryId">分类ID</param>
        /// <param name="jqgridparam">表格参数</param>
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
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 搜索商家商品
        /// </summary>
        /// <param name="merchantId">商户Id</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public ActionResult GetMerchantProductList(string merchantId, string keyword)
        {
            var listData = _productMerchantIdBLL.GetProductMerchantIdList(20, string.Empty, HttpUtility.UrlDecode(keyword));
            var errMsg = string.Empty;
            var returnList = listData;
            if (listData.Count() == 0)
            {
                errMsg = "查询不到商品";
            }
            else
            {
                var selectList = listData.Where(i => i.MerchantId == HttpUtility.UrlDecode(merchantId)).ToList();
                returnList = selectList;
                if (selectList.Count() == 0)
                {
                    errMsg = string.Format("商品[{0}]不是当前商户所有，请检查！", HttpUtility.UrlDecode(keyword));
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
        /// 提交表单
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="product">商品信息</param>
        /// <param name="buildFormJson">自定义表单</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitProductForm(string keyValue, ProductEntity product, string buildFormJson)
        {
            string moduleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string message = keyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(keyValue))
                {
                    if (keyValue == ManageProvider.Provider.Current().UserId)
                    {
                        throw new Exception("无权限编辑本人信息");
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
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 获取商品信息对象返回JSON
        /// </summary>
        /// <param name="keyValue">主键值</param>
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

        #region 商品导入
        /// <summary>
        /// 商品导入弹出框页面
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ImportProduct()
        {
            return View();
        }

        /// <summary>
        /// 商品导入
        /// </summary>
        /// <returns></returns>
        public ActionResult SubmitImportProduct()
        {
            bool isSuccess = false;//导入状态
            string errMessage;
            var dataResult = new DataTable();//导入错误记录表
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["filePath"];//获取上传的文件
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
                                throw new Exception("商品导入失败：" + errMessage);
                            }
                        }
                        else
                        {
                            throw new Exception("商品导入失败：文件格式不正确");
                        }
                    }
                    else
                    {
                        throw new Exception("商品导入失败：文件格式不正确");
                    }
                }
                else
                {
                    throw new Exception("请选择上传文件");
                }
            }
            catch (Exception ex)
            {
                BaseSysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "异常错误：" + ex.Message);
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
        /// 加载商品商户列表
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
                if (!string.IsNullOrEmpty(dr["RelationId"].ToString()))//判断是否选中
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
                return Content(new JsonMessage { Success = true, Code = isOk.ToString(), Message = "操作成功。" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 搜索商家商品(根据商户查询)
        /// </summary>
        /// <param name="merchantId">商户Id</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public ActionResult GetProductMerchantIdList(string merchantId, string keyword)
        {
            var listData = _productMerchantIdBLL.GetProductMerchantIdList(20,string.Empty, HttpUtility.UrlDecode(keyword));
            var errMsg = string.Empty;
            var returnList = listData;
            if (listData.Count() == 0)
            {
                errMsg = "查询不到商品";
            }
            else
            {
                var selectList = listData.Where(i => i.MerchantId == HttpUtility.UrlDecode(merchantId)).ToList();
                returnList = selectList;
                if (selectList.Count() == 0)
                {
                    errMsg = string.Format("商品[{0}]不是当前商户所有，请检查！", HttpUtility.UrlDecode(keyword));
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
        /// 商户返回列表Json
        /// </summary>
        /// <returns></returns>
        public ActionResult MerchantListJson(string productId)
        {
            DataTable listData = _productBLL.ProductMerchantList(productId);
            return Content(listData.ToJson());
        }

        /// <summary>
        /// 仓库返回列表Json
        /// </summary>
        /// <returns></returns>
        public ActionResult WarehouseListJson(string productId, string merchantId)
        {
            DataTable listData = _productBLL.ProductWarehouseList(productId, merchantId);
            return Content(listData.ToJson());
        }

        /// <summary>
        /// 库位返回列表Json
        /// </summary>
        /// <returns></returns>
        public ActionResult ZoneListJson(string productId, string merchantId, string warehouseId)
        {
            DataTable listData = _productBLL.ProductZoneList(productId, merchantId, warehouseId);
            return Content(listData.ToJson());
        }

        /// <summary>
        /// 库位返回列表Json
        /// </summary>
        /// <returns></returns>
        public ActionResult LocationListJson(string productId, string merchantId, string warehouseId, string zoneId)
        {
            DataTable listData = _productBLL.ProductLocationList(productId, merchantId, warehouseId, zoneId);
            return Content(listData.ToJson());
        }

        /// <summary>
        /// 商品储位列表Json
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
                BaseSysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 新增商品配置
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
                string message = "新增成功。";

                bool flag = _productBLL.IsExistProductLocation(productLocation.ProductId, productLocation.MerchantId, productLocation.WarehouseId, productLocation.ZoneId);
                if (flag)
                {
                    throw new Exception("同一个商户+仓库+库区只能存在一个默认储位，请删除已存在储位后再新增！");
                }

                productLocation.Create();
                database.Insert(productLocation, isOpenTrans);
                database.Commit();

                return Content(new JsonMessage { Success = true, Code = "1", Message = message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        public ActionResult DeleteLocation(string productLocationID)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string message = "删除成功。";
                ProductLocationEntity productLoacation = database.FindEntity<ProductLocationEntity>("ProductLocationID", productLocationID);
                if (productLoacation == null)
                {
                    throw new Exception("获取储位数据失败，请刷新重试！");
                }
                database.Delete<ProductLocationEntity>("ProductLocationID", productLocationID, isOpenTrans);

                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}