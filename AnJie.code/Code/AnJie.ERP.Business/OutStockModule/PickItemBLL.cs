using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Data.Common;
using AnJie.ERP.DataAccess;
using AnJie.ERP.ViewModel.OutStockModule;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// Pick_Item
    /// </summary>
    public class PickItemBLL : RepositoryFactory<PickItemEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="pickNo"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool UpdateOrderPickNo(string orderNo, string pickNo, DbTransaction isOpenTrans)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Pick_Item] SET PickNo = @PickNo, Status = @Status
                            WHERE OrderNo = @OrderNo AND Status = @CurrentStatus AND PickNo IS NULL");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@PickNo", pickNo),
                DbFactory.CreateDbParameter("@Status", (int) PickItemStatus.Picking),
                DbFactory.CreateDbParameter("@CurrentStatus", (int) PickItemStatus.Initial),
                DbFactory.CreateDbParameter("@OrderNo", orderNo)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray(), isOpenTrans) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="pickNo"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public bool CancelUpdateOrderPickNo(string orderNo, string pickNo, DbTransaction isOpenTrans)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Pick_Item] SET PickNo = NULL, Status = @Status
                            WHERE OrderNo = @OrderNo AND PickNo = @PickNo AND Status = @CurrentStatus ");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@PickNo", pickNo),
                DbFactory.CreateDbParameter("@Status", (int) PickItemStatus.Initial),
                DbFactory.CreateDbParameter("@CurrentStatus", (int) PickItemStatus.Picking),
                DbFactory.CreateDbParameter("@OrderNo", orderNo)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray(), isOpenTrans) > 0;
        }

        /// <summary>
        /// 拣货单明细
        /// </summary>
        /// <param name="pickNo">拣货单号</param>
        /// <returns></returns>
        public List<PickItemViewModel> GetPickItemListByPickNo(string pickNo)
        {
            var strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
            strSql.Append(@"SELECT PK.*, P.ProductName
                            FROM Pick_Item PK
                            INNER JOIN dbo.Product P ON PK.ProductId = P.ProductId
                            WHERE 1 = 1");
            strSql.Append(" AND PK.PickNo = @PickNo");
            parameter.Add(DbFactory.CreateDbParameter("@PickNo", pickNo));
            return DataFactory.Database().FindListBySql<PickItemViewModel>(strSql.ToString(), parameter.ToArray());
        }

        public List<PickItemViewModel> GetPickItemListByOrderNo(string orderNo)
        {
            var strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
            strSql.Append(@"SELECT PK.*, P.ProductName
                            FROM Pick_Item PK
                            INNER JOIN dbo.Product P ON PK.ProductId = P.ProductId
                            WHERE 1 = 1");
            strSql.Append(" AND PK.OrderNo = @OrderNo");
            parameter.Add(DbFactory.CreateDbParameter("@OrderNo", orderNo));
            return DataFactory.Database().FindListBySql<PickItemViewModel>(strSql.ToString(), parameter.ToArray());
        }
    }
}