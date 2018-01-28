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
    /// 批量打印主表
    /// </summary>
    public class PrintBatchBLL : RepositoryFactory<PrintBatchEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public List<PrintBatchEntity> GetList(string createUserId)
        {
            var strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM [Print_Batch] WHERE CreateUserId=@CreateUserId ORDER BY CreateDate DESC");
            parameter.Add(DbFactory.CreateDbParameter("@CreateUserId", createUserId));
            return DataFactory.Database().FindListBySql<PrintBatchEntity>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public List<PrintBatchItemViewModel> GetBatchItemList(string batchId)
        {
            var strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  batchItem.* , so.ExpressNum
                            FROM    dbo.SaleOrder so
                                    INNER JOIN dbo.Print_BatchItem batchItem ON batchItem.OrderNo = so.OrderNo
                            WHERE   batchItem.BatchId = @BatchId");
            parameter.Add(DbFactory.CreateDbParameter("@BatchId", batchId));

            return DataFactory.Database().FindListBySql<PrintBatchItemViewModel>(strSql.ToString(), parameter.ToArray());
        }
    }
}