using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Data;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 配送商管理表
    /// </summary>
    public class ShipVendorBLL : RepositoryFactory<ShipVendorEntity>
    {
        /// <summary>
        /// 获取配送商列表
        /// </summary>
        /// <returns></returns>
        public List<ShipVendorEntity> GetList()
        {
            StringBuilder whereSql = new StringBuilder();
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    WhereSql.Append(" AND ( ShipVendorId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    WhereSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    WhereSql.Append(" ) )");
            //}
            whereSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindList(whereSql.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetShipVendorWarehouseTree()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( 
                                        SELECT
                                            ShipVendorId AS NodeId, Code, FullName AS NodeName, '0' AS ParentId,
                                            SortCode, 'ShipVendor' AS Sort
                                        FROM
                                            dbo.ShipVendor
                                        UNION
                                        SELECT
                                            w.WarehouseId AS NodeId, w.WarehouseCode AS Code,
                                            w.WarehouseName AS NodeName, m.ShipVendorId AS ParentId, w.SortCode,
                                            'Warehouse' AS Sort
                                        FROM
                                            dbo.ShipVendor m
                                        CROSS JOIN dbo.Warehouse w 
                                    ) T WHERE 1=1 ");
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(" AND ( NodeId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    strSql.Append(" ) )");
            //}
            strSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString());
        }
    }
}