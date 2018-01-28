using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Data.Common;
using AnJie.ERP.DataAccess;
using AnJie.ERP.ViewModel.MerchantModule;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 商户运费结算模板
    /// </summary>
    public class MerchantShipTemplateBLL : RepositoryFactory<MerchantShipTemplateEntity>
    {
        /// <summary>
        /// 获取商户列表
        /// </summary>
        /// <returns></returns>
        public List<MerchantShipTemplateViewModel> GetTemplateList(string warehouseId, string merchantId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT A.*, B.FullName AS MerchantName, C.WarehouseName, D.ShipTypeName
                                FROM[Merchant_ShipTemplate] A
                                INNER JOIN dbo.Merchant B ON A.MerchantId = B.MerchantId
                                INNER JOIN dbo.Warehouse C ON A.WarehouseId = C.WarehouseId
                                INNER JOIN dbo.ShipType D ON A.ShipTypeId = D.ShipTypeId
                                WHERE 1 = 1");
            List<DbParameter> parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND A.WarehouseId = @WarehouseId ");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }
            if (!string.IsNullOrEmpty(merchantId))
            {
                strSql.Append(" AND A.MerchantId = @MerchantId ");
                parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            }
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( A.WarehouseId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");

                //strSql.Append(" AND ( A.MerchantId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                //strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                //strSql.Append(" ) )");
            }

            strSql.Append(" ORDER BY A.SortCode ASC");

            return DataFactory.Database().FindListBySql<MerchantShipTemplateViewModel>(strSql.ToString(), parameter.ToArray());
        }

        public List<MerchantShipTemplateItemViewModel> GetTemplateItemList(string templateId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();

            strSql.Append(@"SELECT A.*, B.FullName AS ProvinceName, C.FullName AS CityName,
                                D.FullName AS CountyName
                            FROM Merchant_ShipTemplateItem A
                            LEFT JOIN Base_ProvinceCity B ON A.ProvinceId = B.Code
                            LEFT JOIN Base_ProvinceCity C ON A.CityId = C.Code
                            LEFT JOIN Base_ProvinceCity D ON A.CountyId = D.Code  WHERE 1=1");
            strSql.Append(" AND A.TemplateId = @TemplateId");
            parameter.Add(DbFactory.CreateDbParameter("@TemplateId", templateId));
            return DataFactory.Database().FindListBySql<MerchantShipTemplateItemViewModel>(strSql.ToString(), parameter.ToArray());
        }
    }
}