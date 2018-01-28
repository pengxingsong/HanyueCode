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
    /// 作业区域
    /// </summary>
    public class WarehouseAreaBLL : RepositoryFactory<WarehouseAreaEntity>
    {
        /// <summary>
        /// 根据仓库ID获取作业区域列表
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns></returns>
        public DataTable GetList(string warehouseId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  A.AreaId ,
                                    A.WarehouseId ,
                                    A.AreaCode ,
                                    A.AreaName ,
                                    A.Description ,
                                    A.Status ,
                                    A.SortCode ,
                                    W.WarehouseName
                            FROM    Warehouse_Area A
                                    INNER JOIN Warehouse W ON W.WarehouseId = A.WarehouseId
                            WHERE   1 = 1 ");
            var parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND A.WarehouseId = @WarehouseId");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }
            strSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}