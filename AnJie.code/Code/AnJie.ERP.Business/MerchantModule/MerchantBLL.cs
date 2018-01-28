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
    /// 商户管理
    /// </summary>
    public class MerchantBLL : RepositoryFactory<MerchantEntity>
    {
        /// <summary>
        /// 获取商户列表
        /// </summary>
        /// <returns></returns>
        public List<MerchantEntity> GetList()
        {
            StringBuilder whereSql = new StringBuilder();
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    WhereSql.Append(" AND ( MerchantId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    WhereSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    WhereSql.Append(" ) )");
            //}
            whereSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindList(whereSql.ToString());
        }

        /// <summary>
        /// 获取 仓库、商户 列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetWarehouseMerchantTree()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    (  SELECT WarehouseId AS NodeId, WarehouseCode AS Code,
                                                WarehouseName AS NodeName, '0' AS ParentId, SortCode,
                                                'Warehouse' AS Sort
                                            FROM dbo.Warehouse
                                         UNION
                                         SELECT m.MerchantId AS NodeId, m.Code, m.FullName AS NodeName,
                                                w.WarehouseId AS ParentId, m.SortCode, 'Merchant' AS Sort
                                            FROM dbo.Merchant m
                                            INNER JOIN dbo.Merchant_Warehouse w ON m.MerchantId = w.MerchantId
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

        /// <summary>
        /// 获取 仓库、商户 列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMerchantWarehouseTree()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( 
                                         SELECT MerchantId AS NodeId, Code, FullName AS NodeName, '0' AS ParentId,
                                                SortCode, 'Merchant' AS Sort
                                            FROM dbo.Merchant
                                         UNION
                                         SELECT w.WarehouseId AS NodeId, w.WarehouseCode AS Code,
                                                w.WarehouseName AS NodeName, m.MerchantId AS ParentId, w.SortCode,
                                                'Warehouse' AS Sort
                                            FROM dbo.Merchant m
                                            INNER JOIN dbo.Merchant_Warehouse r ON m.MerchantId = r.MerchantId
                                            INNER JOIN dbo.Warehouse w ON r.WarehouseId = w.WarehouseId
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public DataTable MerchantWarehouseList(string merchantId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  A.WarehouseId ,
                                    A.WarehouseCode ,
                                    A.WarehouseName ,
                                    B.RelationId
                            FROM    Warehouse A
                                    LEFT JOIN Merchant_Warehouse B ON A.WarehouseId = B.WarehouseId
                                                                      AND B.MerchantId = @MerchantId
                           ");
            parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="arrayObjectId"></param>
        /// <returns></returns>
        public int SetMerchantWarehouse(string merchantId, string[] arrayObjectId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                StringBuilder sbDelete = new StringBuilder("DELETE FROM Merchant_Warehouse WHERE MerchantId = @MerchantId");
                var parameter = new List<DbParameter>
                {
                    DbFactory.CreateDbParameter("@MerchantId", merchantId)
                };
                database.ExecuteBySql(sbDelete, parameter.ToArray(), isOpenTrans);
                foreach (string item in arrayObjectId)
                {
                    if (item.Length > 0)
                    {
                        MerchantWarehouseEntity entity = new MerchantWarehouseEntity();
                        entity.RelationId = CommonHelper.GetGuid;
                        entity.MerchantId = merchantId;
                        entity.WarehouseId = item;
                        entity.Create();
                        database.Insert(entity, isOpenTrans);
                    }
                }
                database.Commit();
                return 1;
            }
            catch
            {
                database.Rollback();
                return -1;
            }
        }
    }
}