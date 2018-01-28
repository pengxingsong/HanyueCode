using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using AnJie.ERP.Business;
using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;

namespace AnJie.ERP.Service
{
    public class ProductService
    {
        private readonly ProductBLL _productBLL = new ProductBLL();

        /// <summary>
        /// 商品导入
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dataResult"></param>
        /// <param name="errMessage"></param>
        /// <param name="importFile"></param>
        /// <returns></returns>
        public bool ImportProduct(ProductImportFileEntity importFile, DataTable dt, out DataTable dataResult, out string errMessage)
        {
            string userId = ManageProvider.Provider.Current().UserId;

            //构造导入返回结果表
            DataTable resultTable = new DataTable("Result");
            resultTable.Columns.Add("rowid", typeof(System.String)); //行号
            resultTable.Columns.Add("locate", typeof(System.String)); //位置
            resultTable.Columns.Add("reason", typeof(System.String)); //原因
            errMessage = string.Empty;
            bool isSuccess = false;
            if (dt != null && dt.Rows.Count > 0)
            {
                IDatabase database = DataFactory.Database();
                try
                {
                    List<ProductImportItemEntity> lstEntities = new List<ProductImportItemEntity>();
                    foreach (DataRow item in dt.Rows)
                    {
                        var importEntity = new ProductImportItemEntity();
                        importEntity.Create();
                        importEntity.FileId = importFile.FileId;
                        importEntity.ProductCode = item["商品编码"].ToString();
                        importEntity.ProductName = item["商品名称"].ToString();
                        importEntity.BriefName = item["商品简称"].ToString();
                        importEntity.Specification = item["规格"].ToString();
                        importEntity.BaseUnit = item["基本单位"].ToString();
                        importEntity.BarCode = item["商品条码"].ToString();
                        importEntity.Brand = item["品牌"].ToString();

                        string strWeight = item["重量"].ToString();
                        decimal weight = 0;
                        if (decimal.TryParse(strWeight, out weight))
                        {
                            importEntity.Weight = weight;
                        }

                        string strVolume = item["体积"].ToString();
                        decimal volume = 0;
                        if (decimal.TryParse(strVolume, out volume))
                        {
                            importEntity.Volume = volume;
                        }

                        string strLong = item["长"].ToString();
                        decimal productLong = 0;
                        if (decimal.TryParse(strLong, out productLong))
                        {
                            importEntity.Long = productLong;
                        }

                        string strWidth = item["宽"].ToString();
                        decimal width = 0;
                        if (decimal.TryParse(strWidth, out width))
                        {
                            importEntity.Width = width;
                        }

                        string strHeight = item["高"].ToString();
                        decimal height = 0;
                        if (decimal.TryParse(strHeight, out height))
                        {
                            importEntity.Height = height;
                        }

                        string isLotControl = item["批号管控"].ToString();
                        importEntity.IsLotControl = isLotControl == "是" ? 1 : 0;

                        string isForceScan = item["是否强制扫描"].ToString();
                        importEntity.IsForcedScan = isForceScan == "是" ? 1 : 0;

                        importEntity.Remark = item["说明"].ToString();

                        if (string.IsNullOrWhiteSpace(importEntity.ProductCode))
                        {
                            throw new Exception("商品编码不能为空");
                        }
                        if (string.IsNullOrWhiteSpace(importEntity.ProductName))
                        {
                            throw new Exception("商品名称不能为空");
                        }

                        bool flag = _productBLL.IsExistProductCode(importEntity.ProductCode);
                        if (flag)
                        {
                            throw new Exception(string.Format("商品编码{0}已存在，不能重复导入", importEntity.ProductCode));
                        }

                        lstEntities.Add(importEntity);
                    }

                    DbTransaction isOpenTrans = database.BeginTrans();

                    List<ProductEntity> productEntities = new List<ProductEntity>();
                    foreach (ProductImportItemEntity importEntity in lstEntities)
                    {
                        ProductEntity productEntity = new ProductEntity();
                        productEntity.Create();
                        productEntity.ProductId = importEntity.ProductId;
                        productEntity.Code = importEntity.ProductCode;
                        productEntity.ProductName = importEntity.ProductName;
                        productEntity.BriefName = importEntity.BriefName;
                        productEntity.Width = importEntity.Width;
                        productEntity.Volume = importEntity.Volume;
                        productEntity.Specification = importEntity.Specification;
                        productEntity.Brand = importEntity.Brand;
                        productEntity.BarCode = importEntity.BarCode;
                        productEntity.BaseUnit = importEntity.BaseUnit;
                        productEntity.Width = importEntity.Width;
                        productEntity.Height = importEntity.Height;
                        productEntity.Long = importEntity.Long;
                        productEntity.IsLotControl = importEntity.IsLotControl;
                        productEntity.IsForcedScan = importEntity.IsForcedScan;
                        productEntity.Remark = importEntity.Remark;
                        productEntities.Add(productEntity);
                    }

                    database.Insert(importFile);
                    foreach (ProductImportItemEntity orderImportEntity in lstEntities)
                    {
                        database.Insert(orderImportEntity, isOpenTrans);
                    }

                    foreach (ProductEntity orderImportEntity in productEntities)
                    {
                        database.Insert(orderImportEntity, isOpenTrans);
                    }

                    database.Commit();
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    database.Rollback();
                    BaseSysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "异常错误：" + ex.Message);
                    isSuccess = false;
                    errMessage = ex.Message;
                }
            }
            dataResult = resultTable;
            return isSuccess;
        }
    }
}