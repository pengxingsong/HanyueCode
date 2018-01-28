using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 商品信息管理
    /// </summary>
    public class ProductBLL : RepositoryFactory<ProductEntity>
    {
        /// <summary>
        /// 选择用户列表
        /// </summary>
        /// <param name="keyword">模块查询</param>
        /// <returns></returns>
        public DataTable OptionUserList(string keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@"SELECT TOP 50 * FROM ( SELECT    
                                        u.UserId ,
                                        u.Account ,
                                        u.code,
                                        u.RealName ,
                                        u.DepartmentId ,
                                        d.FullName AS DepartmentName,
                                        u.Gender
                                FROM    Base_User u
                                LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                                WHERE   u.RealName LIKE @keyword
                                        OR u.Account LIKE @keyword
                                        OR u.Code LIKE @keyword
                                        OR u.Spell LIKE @keyword
                                        OR u.UserId IN (
                                        SELECT  u.UserId
                                        FROM    Base_User u
                                                INNER JOIN Base_ObjectUserRelation oc ON u.UserId = oc.UserId
                                                INNER JOIN dbo.Base_Company c ON c.CompanyId = oc.ObjectId
                                        WHERE   c.FullName LIKE @keyword
                                        UNION
                                        SELECT  u.UserId
                                        FROM    Base_User u
                                                INNER JOIN Base_ObjectUserRelation od ON u.UserId = od.UserId
                                                INNER JOIN Base_Department d ON d.DepartmentId = od.ObjectId
                                        WHERE   d.FullName LIKE @keyword
                                        UNION
                                        SELECT  u.UserId
                                        FROM    Base_User u
                                                INNER JOIN Base_ObjectUserRelation oro ON u.UserId = oro.UserId
                                                INNER JOIN Base_Roles r ON r.RoleId = oro.ObjectId
                                        WHERE   r.FullName LIKE @keyword)
                            ) a WHERE 1 = 1");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            else
            {
                strSql.Append(@"SELECT TOP 50
                                        u.UserId ,
                                        u.Account ,
                                        u.code ,
                                        u.RealName ,
                                        u.DepartmentId ,
                                        d.FullName AS DepartmentName ,
                                        u.Gender
                                FROM    Base_User u
                                        LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                                WHERE   1 = 1");
            }
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(" AND ( UserId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    strSql.Append(" ) )");
            //}
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public bool IsExistProductCode(string productCode)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT COUNT(1) FROM Product WHERE Code = @ProductCode");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ProductCode", productCode)
            };
            return DataFactory.Database().FindCountBySql(strSql.ToString(), parameter.ToArray()) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductEntity GetProduct(string productId)
        {
            return Repository().FindEntity(productId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ProductEntity GetProductByCode(string code)
        {
            return Repository().FindEntity("Code", code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topNum"></param>
        /// <param name="merchantId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<ProductEntity> GetMerchantProductList(int topNum, string merchantId, string keyword)
        {
            List<DbParameter> parameter = new List<DbParameter>();

            StringBuilder strSql = new StringBuilder();
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@" AND (Code LIKE @keyword
                                    OR ProductName LIKE @keyword
                                    OR BriefName LIKE @keyword
                                    OR BarCode LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            //if (!string.IsNullOrEmpty(MerchantId))
            //{
            //    strSql.Append(" AND MerchantId = @MerchantId");
            //    parameter.Add(DbFactory.CreateDbParameter("@MerchantId", MerchantId));
            //}
            strSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindListTop(topNum, strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 获取商品列表列表
        /// </summary>
        /// <param name="keyword">查询关键词</param>
        /// <param name="categoryId">商品分类ID</param>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public DataTable GetPageList(string keyword, string categoryId, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT p.ProductId, p.CategoryId, p.Code, p.ProductName, p.BriefName,
                                    p.Weight, p.Volume, p.Specification, p.Brand, p.BarCode, p.Price,
                                    p.BaseUnit, p.IsLotControl, p.IsForcedScan, p.Remark, p.Enabled,
                                    p.SortCode, p.CreateDate, p.ModifyDate, c.CategoryName
                             FROM   Product p
                                    LEFT JOIN dbo.Product_Category c ON c.CategoryId = p.CategoryId
                             WHERE  1 = 1");
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.AppendFormat(@" AND (p.Code LIKE '{0}'
                                    OR p.ProductName LIKE '{0}'
                                    OR p.BriefName LIKE '{0}'
                                    OR p.BarCode LIKE '{0}')", '%' + keyword + '%');
            }
            if (!string.IsNullOrEmpty(categoryId) && categoryId != "0")
            {
                strSql.Append(" AND p.CategoryId = @CategoryId");
                parameter.Add(DbFactory.CreateDbParameter("@CategoryId", categoryId));
            }
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(" AND ( MerchantId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    strSql.Append(" ) )");
            //}
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public DataTable ProductMerchantList(string productId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  A.MerchantId ,
                                    A.Code AS MerchantCode ,
                                    A.FullName AS MerchantName ,
                                    B.RelationId
                            FROM    dbo.Merchant A
                                    LEFT JOIN dbo.Product_Merchant B ON A.MerchantId = B.MerchantId
                                                                        AND B.ProductId = @ProductId
                           ");
            parameter.Add(DbFactory.CreateDbParameter("@ProductId", productId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="arrayObjectId"></param>
        /// <returns></returns>
        public int SetProductMerchant(string productId, string[] arrayObjectId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                StringBuilder sbDelete = new StringBuilder("DELETE FROM Product_Merchant WHERE ProductId = @ProductId");
                var parameter = new List<DbParameter>
                {
                    DbFactory.CreateDbParameter("@ProductId", productId)
                };
                database.ExecuteBySql(sbDelete, parameter.ToArray(), isOpenTrans);
                foreach (string item in arrayObjectId)
                {
                    if (item.Length > 0)
                    {
                        ProductMerchantEntity entity = new ProductMerchantEntity();
                        entity.RelationId = CommonHelper.GetGuid;
                        entity.MerchantId = item;
                        entity.ProductId = productId;
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

        /// <summary>
        /// 根据商品、商户获取仓库信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public DataTable ProductWarehouseList(string productId,string merchantId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"
                SELECT  C.WarehouseId,C.WarehouseCode,C.WarehouseName
                FROM    [dbo].[Product_Merchant] AS A
                JOIN [dbo].[Merchant_Warehouse] AS B ON A.MerchantId=B.MerchantId
                JOIN [dbo].[Warehouse] AS C ON C.WarehouseId=B.WarehouseId
                WHERE   A.ProductId = @ProductId 
	                AND A.MerchantId = @MerchantId
                GROUP BY C.WarehouseId,C.WarehouseCode,C.WarehouseName;
                           ");
            parameter.Add(DbFactory.CreateDbParameter("@ProductId", productId));
            parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 根据商品、商户、仓库获取库位信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="merchantId"></param>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public DataTable ProductZoneList(string productId, string merchantId, string warehouseId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"
                SELECT  C.ZoneId ,C.ZoneType,(CASE C.ZoneType WHEN 1 THEN '拣货' WHEN 2 THEN '待处理' WHEN 3 THEN '不良品' ELSE '' END) AS ZoneTypeName,C.ZoneCode ,C.ZoneName
                FROM    [dbo].[Product_Merchant] AS A
                        JOIN [dbo].[Merchant_Warehouse] AS B ON A.MerchantId = B.MerchantId
                        JOIN [dbo].[Warehouse_Zone] AS C ON C.WarehouseId = B.WarehouseId
                WHERE   A.ProductId = @ProductId
                        AND B.WarehouseId = @WarehouseId
		                AND A.MerchantId = @MerchantId
                GROUP BY C.ZoneId ,C.ZoneType,C.ZoneCode ,C.ZoneName;
                           ");
            parameter.Add(DbFactory.CreateDbParameter("@ProductId", productId));
            parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 根据商品、商户、仓库、库位获取库位信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="merchantId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        public DataTable ProductLocationList(string productId, string merchantId, string warehouseId, string zoneId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"
                SELECT  D.LocationId , D.Code
                FROM    [dbo].[Product_Merchant] AS A
                        JOIN [dbo].[Merchant_Warehouse] AS B ON A.MerchantId = B.MerchantId
                        JOIN [dbo].[Warehouse_Zone] AS C ON C.WarehouseId = B.WarehouseId
                        JOIN [dbo].[Warehouse_Location] AS D ON C.ZoneId = D.ZoneId
                WHERE   A.ProductId = @ProductId
                        AND B.WarehouseId = @WarehouseId
                        AND A.MerchantId = @MerchantId
                        AND C.ZoneId = @ZoneId
                GROUP BY D.LocationId , D.Code;
                           ");
            parameter.Add(DbFactory.CreateDbParameter("@ProductId", productId));
            parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            parameter.Add(DbFactory.CreateDbParameter("@ZoneId", zoneId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 商品储位配置列表
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="merchantId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="zoneId"></param>
        /// <param name="locationId"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public DataTable GetProductLocationPageList(string productId, string merchantId, string warehouseId, string zoneId,string locationId, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();

            strSql.Append(@"
                SELECT  A.* ,
                        B.WarehouseCode + '-' + B.WarehouseName AS WarehouseName ,
                        C.Code + '-' + C.FullName AS MerchantName ,
                        ( CASE D.ZoneType
                            WHEN 1 THEN '拣货'
                            WHEN 2 THEN '待处理'
                            WHEN 3 THEN '不良品'
                            ELSE ''
                          END ) + '-' + D.ZoneCode + '-' + D.ZoneName AS ZoneName ,
                        E.Code
                FROM    [dbo].[Product_Location] AS A
                        JOIN dbo.Warehouse AS B ON A.WarehouseId = B.WarehouseId
                        JOIN dbo.Merchant AS C ON A.MerchantId = C.MerchantId
                        JOIN dbo.Warehouse_Zone AS D ON D.ZoneId = A.ZoneId
                        JOIN [dbo].[Warehouse_Location] AS E ON E.LocationId = A.LocationId
                WHERE   A.ProductId = @ProductId ");
            parameter.Add(DbFactory.CreateDbParameter("@ProductId", productId));
            if (!string.IsNullOrWhiteSpace(merchantId))
            {
                strSql.Append(" AND A.MerchantId = @MerchantId ");
                parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            }
            if (!string.IsNullOrWhiteSpace(warehouseId))
            {
                strSql.Append(" AND A.WarehouseId = @WarehouseId ");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }
            if (!string.IsNullOrWhiteSpace(zoneId))
            {
                strSql.Append(" AND A.ZoneId = @ZoneId ");
                parameter.Add(DbFactory.CreateDbParameter("@ZoneId", zoneId));
            }
            if (!string.IsNullOrWhiteSpace(locationId))
            {
                strSql.Append(" AND A.LocationId = @LocationId ");
                parameter.Add(DbFactory.CreateDbParameter("@LocationId", locationId));
            }
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        /// <summary>
        /// 判断商品、商户、仓库、库区是否已存在对应储位，只能存一条
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="merchantId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        public bool IsExistProductLocation(string productId, string merchantId, string warehouseId, string zoneId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"
                SELECT  COUNT(1)
                FROM    Product_Location
                WHERE   ProductId = @ProductId
                        AND MerchantId = @MerchantId
                        AND WarehouseId = @WarehouseId
                        AND ZoneId = @ZoneId");

            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ProductId", productId),
                DbFactory.CreateDbParameter("@MerchantId", merchantId),
                DbFactory.CreateDbParameter("@WarehouseId", warehouseId),
                DbFactory.CreateDbParameter("@ZoneId", zoneId)
            };
            return DataFactory.Database().FindCountBySql(strSql.ToString(), parameter.ToArray()) > 0;
        }
    }
}