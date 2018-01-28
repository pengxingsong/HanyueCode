using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using AnJie.ERP.ViewModel.InStockModule;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 收货单主表
    /// </summary>
    public class ReceiptBLL : RepositoryFactory<ReceiptEntity>
    {
        /// <summary>
        /// 收货单列表
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime">订单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <param name="warehouseId"></param>
        /// <param name="merchantId"></param>
        /// <param name="receiptNo"></param>
        /// <returns></returns>
        public List<ReceiptViewModel> GetReceiptList(string warehouseId, string merchantId, string receiptNo,
            string startTime, string endTime, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT R.*, W.WarehouseName,
                                M.FullName AS MerchantName
                            FROM [Receipt] R
                            INNER JOIN dbo.Warehouse W ON R.WarehouseId = W.WarehouseId
                            INNER JOIN dbo.Merchant M ON R.MerchantId = M.MerchantId WHERE 1=1");
            if (!string.IsNullOrEmpty(warehouseId))
            {
                strSql.Append(" AND R.WarehouseId = @WarehouseId ");
                parameter.Add(DbFactory.CreateDbParameter("@WarehouseId", warehouseId));
            }
            if (!string.IsNullOrEmpty(merchantId))
            {
                strSql.Append(" AND R.MerchantId = @MerchantId ");
                parameter.Add(DbFactory.CreateDbParameter("@MerchantId", merchantId));
            }
            if (!string.IsNullOrEmpty(receiptNo))
            {
                strSql.Append(" AND R.ReceiptNo = @ReceiptNo ");
                parameter.Add(DbFactory.CreateDbParameter("@ReceiptNo", receiptNo));
            }

            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                strSql.Append(" AND R.ReceiptDate Between @StartTime AND @EndTime ");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", CommonHelper.GetDateTime(startTime + " 00:00")));
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", CommonHelper.GetDateTime(endTime + " 23:59")));
            }
            return new Repository<ReceiptViewModel>().FindListPageBySql(strSql.ToString(), parameter.ToArray(),
                ref jqgridparam);
        }

        /// <summary>
        /// 收货单明细列表
        /// </summary>
        /// <param name="receiptId">订单主键</param>
        /// <returns></returns>
        public List<ReceiptItemViewModel> GetReceiptItemList(string receiptId)
        {
            var strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  item.* ,
                                    product.Specification ,
                                    product.BaseUnit ,
                                    product.BarCode
                            FROM    Receipt_Item item
                                    LEFT JOIN dbo.Product product ON item.ProductId = product.ProductId");
                                        strSql.Append(" Where ReceiptId = @ReceiptId");
            parameter.Add(DbFactory.CreateDbParameter("@ReceiptId", receiptId));
            return DataFactory.Database().FindListBySql<ReceiptItemViewModel>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 审核收货单
        /// </summary>
        /// <param name="receipt"></param>
        /// <returns></returns>
        public int Audit(ReceiptEntity receipt)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.Receipt
                            SET Status = 1, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE ReceiptId = @ReceiptId
                                AND Status = 0");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyUserId", receipt.ModifyUserId),
                DbFactory.CreateDbParameter("@ModifyUserName", receipt.ModifyUserName),
                DbFactory.CreateDbParameter("@ModifyDate", receipt.ModifyDate),
                DbFactory.CreateDbParameter("@ReceiptId", receipt.ReceiptId)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiptNo"></param>
        /// <returns></returns>
        public ReceiptViewModel GetReceiptByReceiptNo(string receiptNo)
        {
            var strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
            strSql.Append(@"SELECT R.*, W.WarehouseName,
                                M.FullName AS MerchantName
                            FROM [Receipt] R
                            INNER JOIN dbo.Warehouse W ON R.WarehouseId = W.WarehouseId
                            INNER JOIN dbo.Merchant M ON R.MerchantId = M.MerchantId WHERE 1=1");
            strSql.Append(" AND R.ReceiptNo = @ReceiptNo ");
            parameter.Add(DbFactory.CreateDbParameter("@ReceiptNo", receiptNo));

            return new Repository<ReceiptViewModel>().FindEntityBySql(strSql.ToString(), parameter.ToArray());
        }

        public int UpdateReceiptStatus(ReceiptEntity receipt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.Receipt
                            SET Status = @Status, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE ReceiptId = @ReceiptId");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyUserId", receipt.ModifyUserId),
                DbFactory.CreateDbParameter("@ModifyUserName", receipt.ModifyUserName),
                DbFactory.CreateDbParameter("@ModifyDate", receipt.ModifyDate),
                DbFactory.CreateDbParameter("@ReceiptId", receipt.ReceiptId),
                DbFactory.CreateDbParameter("@Status", receipt.Status)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receipt"></param>
        /// <returns></returns>
        public int CancelAudit(ReceiptEntity receipt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.Receipt
                            SET Status = 0, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE ReceiptId = @ReceiptId
                                AND Status = 1");
            List<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyUserId", receipt.ModifyUserId),
                DbFactory.CreateDbParameter("@ModifyUserName", receipt.ModifyUserName),
                DbFactory.CreateDbParameter("@ModifyDate", receipt.ModifyDate),
                DbFactory.CreateDbParameter("@ReceiptId", receipt.ReceiptId)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray());
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="receipt"></param>
        /// <returns></returns>
        public int Invalid(ReceiptEntity receipt)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.Receipt
                            SET Status = -1, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE ReceiptId = @ReceiptId
                                AND Status = 0");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyUserId", receipt.ModifyUserId),
                DbFactory.CreateDbParameter("@ModifyUserName", receipt.ModifyUserName),
                DbFactory.CreateDbParameter("@ModifyDate", receipt.ModifyDate),
                DbFactory.CreateDbParameter("@ReceiptId", receipt.ReceiptId)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray());
        }

        /// <summary>
        /// 收货单挂起
        /// </summary>
        /// <param name="receipt"></param>
        /// <returns></returns>
        public bool UpdateLockedStatus(ReceiptEntity receipt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.Receipt
                            SET IsLocked = @IsLocked, ModifyUserId = @ModifyUserId,
                                ModifyUserName = @ModifyUserName, ModifyDate = @ModifyDate
                            WHERE ReceiptId = @ReceiptId");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@ModifyUserId", receipt.ModifyUserId),
                DbFactory.CreateDbParameter("@ModifyUserName", receipt.ModifyUserName),
                DbFactory.CreateDbParameter("@ModifyDate", receipt.ModifyDate),
                DbFactory.CreateDbParameter("@IsLocked", receipt.IsLocked),
                DbFactory.CreateDbParameter("@ReceiptId", receipt.ReceiptId)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray()) > 0;
        }
    }
}