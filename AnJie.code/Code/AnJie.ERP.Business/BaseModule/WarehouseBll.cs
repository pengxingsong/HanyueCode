using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Data;
using System.Data.Common;
using AnJie.ERP.DataAccess;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// ≤÷ø‚–≈œ¢
    /// </summary>
    public class WarehouseBLL : RepositoryFactory<WarehouseEntity>
    {
        public WarehouseEntity GetWarehouse(string warehouseId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                                FROM    dbo.Warehouse
                                WHERE   WarehouseId = @WarehouseId");
            parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            return DataFactory.Database().FindEntityBySql<WarehouseEntity>(strSql.ToString(), parameter.ToArray());
        }

        public List<WarehouseEntity> GetList(string companyId = null)
        {
            StringBuilder whereSql = new StringBuilder();
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                whereSql.Append(" AND ( WarehouseId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                whereSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                whereSql.Append(" ) )");
            }
            var parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(companyId))
            {
                whereSql.Append(" AND CompanyId = @CompanyId");
                parameter.Add(DbFactory.CreateDbParameter("@CompanyId", companyId));
            }
            whereSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindList(whereSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateReceiptLocationId(WarehouseEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.Warehouse
                            SET ReceiptLocationId = @ReceiptLocationId, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE WarehouseId = @WarehouseId");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyUserId", entity.ModifyUserId),
                DbFactory.CreateDbParameter("@ModifyUserName", entity.ModifyUserName),
                DbFactory.CreateDbParameter("@ModifyDate", entity.ModifyDate),
                DbFactory.CreateDbParameter("@ReceiptLocationId", entity.ReceiptLocationId),
                DbFactory.CreateDbParameter("@WarehouseId", entity.WarehouseId)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray()) > 0;
        }
    }
}