using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using AnJie.ERP.DataAccess;
using AnJie.ERP.ViewModel.OrderModule;
using AnJie.ERP.ViewModel.OutStockModule;
using System;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// Pick_Master
    /// </summary>
    public class PickMasterBLL : RepositoryFactory<PickMasterEntity>
    {
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="pickNo">拣货单号</param>
        /// <param name="startTime">订单开始时间</param>
        /// <param name="endTime">订单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public List<PickMasterViewModel> GetPickMasterList(string warehouseId, string pickNo, string startTime,
            string endTime, JqGridParam jqgridparam)
        {
            var strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
            strSql.Append(@"SELECT PM.*, W.WarehouseName
                            FROM dbo.Pick_Master PM
                            LEFT JOIN dbo.Warehouse W ON PM.WarehouseId = W.WarehouseId
                            WHERE 1 = 1");
            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND PM.WarehouseId = @WarehouseId ");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }

            if (!string.IsNullOrEmpty(pickNo))
            {
                strSql.Append(" AND PM.PickNo = @PickNo ");
                parameter.Add(DbFactory.CreateDbParameter("@PickNo", pickNo));
            }

            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                strSql.Append(" AND PM.CreateDate Between @StartTime AND @EndTime ");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", CommonHelper.GetDateTime(startTime + " 00:00")));
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", CommonHelper.GetDateTime(endTime + " 23:59")));
            }
            return new Repository<PickMasterViewModel>().FindListPageBySql(strSql.ToString(), parameter.ToArray(),
                ref jqgridparam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pickStauts"></param>
        /// <param name="isOpenTrans"></param>
        /// <param name="pickMaster"></param>
        /// <returns></returns>
        public bool UpdateStatus(PickMasterEntity pickMaster, PickMasterStatus pickStauts, DbTransaction isOpenTrans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.Pick_Master
                            SET Status = @Status, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE PickId = @PickId
                                AND Status <= @CurrentStatus");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyUserId", pickMaster.ModifyUserId),
                DbFactory.CreateDbParameter("@ModifyUserName", pickMaster.ModifyUserName),
                DbFactory.CreateDbParameter("@ModifyDate", pickMaster.ModifyDate),
                DbFactory.CreateDbParameter("@Status", pickMaster.Status),
                DbFactory.CreateDbParameter("@CurrentStatus", pickStauts),
                DbFactory.CreateDbParameter("@PickId", pickMaster.PickId)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray(), isOpenTrans) > 0;
        }
    }
}