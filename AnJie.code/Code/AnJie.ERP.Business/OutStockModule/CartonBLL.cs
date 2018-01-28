using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using AnJie.ERP.DataAccess;
using AnJie.ERP.ViewModel.OrderModule;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 包裹
    /// </summary>
    public class CartonBLL : RepositoryFactory<CartonEntity>
    {
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="orderNo">订单号</param>
        /// <param name="orderStatus"></param>
        /// <param name="startTime">订单开始时间</param>
        /// <param name="endTime">订单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public List<SaleOrderViewModel> GetCartonList(string warehouseId, string merchantId, string orderNo, OrderStatus? orderStatus, string startTime, string endTime, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT SO.*, W.WarehouseName, M.FullName AS MerchantName, MM.MallName,
                                S.ShipTypeName
                            FROM SaleOrder SO
                            LEFT JOIN dbo.Warehouse W ON SO.WarehouseId = W.WarehouseId
                            LEFT JOIN dbo.Merchant M ON SO.MerchantId = M.MerchantId
                            LEFT JOIN dbo.Merchant_Mall MM ON SO.MerchantMallId = MM.MallId
                            LEFT JOIN dbo.ShipType S ON SO.ShipTypeId = S.ShipTypeId WHERE 1=1");
            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND SO.WarehouseId = @WarehouseId ");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }

            if (orderStatus.HasValue)
            {
                strSql.Append(" AND SO.Status = @Status ");
                parameter.Add(DbFactory.CreateDbParameter("@Status", (int)orderStatus.Value));
            }

            if (!string.IsNullOrEmpty(merchantId))
            {
                strSql.Append(" AND SO.MerchantId = @MerchantId ");
                parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            }
            if (!string.IsNullOrEmpty(orderNo))
            {
                strSql.Append(" AND SO.orderNo = @orderNo ");
                parameter.Add(DbFactory.CreateDbParameter("@orderNo", orderNo));
            }

            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                strSql.Append(" AND SO.OrderDate Between @startTime AND @endTime ");
                parameter.Add(DbFactory.CreateDbParameter("@startTime", CommonHelper.GetDateTime(startTime + " 00:00")));
                parameter.Add(DbFactory.CreateDbParameter("@endTime", CommonHelper.GetDateTime(endTime + " 23:59")));
            }
            return new Repository<SaleOrderViewModel>().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

    }
}