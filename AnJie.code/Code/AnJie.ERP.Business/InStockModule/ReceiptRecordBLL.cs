using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using AnJie.ERP.DataAccess;
using AnJie.ERP.ViewModel.InStockModule;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// Receipt_Record
    /// </summary>
    public class ReceiptRecordBLL : RepositoryFactory<ReceiptRecordEntity>
    {

        /// <summary>
        /// 收货记录明细列表
        /// </summary>
        /// <param name="receiptId">订单主键</param>
        /// <returns></returns>
        public List<ReceiptRecordEntity> GetReceiptRecordList(string receiptId)
        {
            var strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM    Receipt_Record
                            WHERE   ReceiptId = @ReceiptId And Status = 0");
            parameter.Add(DbFactory.CreateDbParameter("@ReceiptId", receiptId));
            return DataFactory.Database().FindListBySql<ReceiptRecordEntity>(strSql.ToString(), parameter.ToArray());
        }

    }
}