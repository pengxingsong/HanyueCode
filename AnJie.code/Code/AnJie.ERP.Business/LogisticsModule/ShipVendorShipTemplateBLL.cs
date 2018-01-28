using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using AnJie.ERP.ViewModel.LogisticsModule;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 配送商运费结算模板
    /// </summary>
    public class ShipVendorShipTemplateBLL : RepositoryFactory<ShipVendorShipTemplateEntity>
    {
        /// <summary>
        /// 获取配送商列表
        /// </summary>
        /// <returns></returns>
        public List<ShipVendorShipTemplateViewModel> GetTemplateList(string warehouseId, string shipVendorId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT A.*, B.FullName AS ShipVendorName, C.WarehouseName, D.ShipTypeName
                                FROM [ShipVendor_ShipTemplate] A
                                INNER JOIN dbo.ShipVendor B ON A.ShipVendorId = B.ShipVendorId
                                INNER JOIN dbo.Warehouse C ON A.WarehouseId = C.WarehouseId
                                INNER JOIN dbo.ShipType D ON A.ShipTypeId = D.ShipTypeId
                                WHERE 1 = 1");
            List<DbParameter> parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND A.WarehouseId = @WarehouseId ");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }
            if (!string.IsNullOrEmpty(shipVendorId))
            {
                strSql.Append(" AND A.ShipVendorId = @ShipVendorId ");
                parameter.Add(DbFactory.CreateDbParameter("@ShipVendorId", shipVendorId));
            }
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(" AND ( A.WarehouseId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    strSql.Append(" ) )");
            //}

            strSql.Append(" ORDER BY A.SortCode ASC");

            return DataFactory.Database().FindListBySql<ShipVendorShipTemplateViewModel>(strSql.ToString(), parameter.ToArray());
        }

        public List<ShipVendorShipTemplateItemViewModel> GetTemplateItemList(string templateId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT A.*, B.FullName AS ProvinceName, C.FullName AS CityName,
                                D.FullName AS CountyName
                            FROM ShipVendor_ShipTemplateItem A
                            LEFT JOIN Base_ProvinceCity B ON A.ProvinceId = B.Code
                            LEFT JOIN Base_ProvinceCity C ON A.CityId = C.Code
                            LEFT JOIN Base_ProvinceCity D ON A.CountyId = D.Code  WHERE 1=1");
            strSql.Append(" AND A.TemplateId = @TemplateId");
            parameter.Add(DbFactory.CreateDbParameter("@TemplateId", templateId));
            return DataFactory.Database().FindListBySql<ShipVendorShipTemplateItemViewModel>(strSql.ToString(), parameter.ToArray());
        }
    }
}