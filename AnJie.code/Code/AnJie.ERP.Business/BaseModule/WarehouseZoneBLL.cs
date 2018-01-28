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
    /// 库存信息
    /// </summary>
    public class WarehouseZoneBLL : RepositoryFactory<WarehouseZoneEntity>
    {

        /// <summary>
        /// 根据仓库ID获取库区列表
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns></returns>
        public DataTable GetList(string warehouseId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  Z.ZoneId, Z.WarehouseId, Z.ZoneCode, Z.ZoneName, Z.ZoneType, Z.InLoc,
                                    Z.OutLoc, Z.CheckMethod, Z.PickMethod, Z.IsCollect, Z.Status,
                                    Z.Comments, Z.SortCode, W.WarehouseName
                            FROM    Warehouse_Zone Z
                                    INNER JOIN Warehouse W ON W.WarehouseId = Z.WarehouseId
                            WHERE   1 = 1 ");
            var parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND Z.WarehouseId = @WarehouseId");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }
            strSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}