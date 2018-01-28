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
    /// 储位信息
    /// </summary>
    public class WarehouseLocationBLL : RepositoryFactory<WarehouseLocationEntity>
    {
        /// <summary>
        /// 根据仓库ID获取储位列表
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns></returns>
        public DataTable GetList(string warehouseId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  L.LocationId, L.WarehouseId, L.Code, L.AreaCode, L.PutZone,
                                    L.AllocZone, L.CCZone, L.HighValue, L.LastCCDate, L.Alsle, L.Bay,
                                    L.Floor, L.Length, L.Width, L.Height, L.Cube, L.Weight, L.PickingSEQ,
                                    L.CycleCountSEQ, L.PutAwaySEQ, L.LocationClass, L.LocationType,
                                    L.MovementType, L.CommingleSKU, L.CommingleLOT, L.IsLostLPN,
                                    L.IsEnable, L.IsCheckCapacity, L.Comments, W.WarehouseName
                            FROM    dbo.Warehouse_Location L
                                    INNER JOIN Warehouse W ON W.WarehouseId = L.WarehouseId
                            WHERE   1 = 1");
            var parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND W.WarehouseId = @WarehouseId");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }
            strSql.Append(" ORDER BY Code ASC");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        public WarehouseLocationEntity GetLocationByCode(string warehouseId, string locationCode)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM dbo.Warehouse_Location WHERE 1=1 ");
            strSql.Append(" AND WarehouseId = @WarehouseId");
            strSql.Append(" AND Code = @Code");

            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@WarehouseId", warehouseId),
                DbFactory.CreateDbParameter("@Code", locationCode)
            };

            WarehouseLocationEntity location = Repository().FindEntityBySql(strSql.ToString(), parameter.ToArray());
            if (string.IsNullOrEmpty(location?.LocationId)) 
            {
                return null;
            }
            return location;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public WarehouseLocationEntity GetLocation(string warehouseId, string locationId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM dbo.Warehouse_Location WHERE 1=1 ");
            strSql.Append(" AND WarehouseId = @WarehouseId");
            strSql.Append(" AND LocationId = @LocationId");

            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@WarehouseId", warehouseId),
                DbFactory.CreateDbParameter("@LocationId", locationId)
            };

            WarehouseLocationEntity location = Repository().FindEntityBySql(strSql.ToString(), parameter.ToArray());
            if (string.IsNullOrEmpty(location?.LocationId))
            {
                return null;
            }
            return location;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public List<WarehouseLocationEntity> GetListByWarehouseId(string warehouseId)
        {
            return DataFactory.Database().FindList<WarehouseLocationEntity>("WarehouseId", warehouseId);
        }
    }
}