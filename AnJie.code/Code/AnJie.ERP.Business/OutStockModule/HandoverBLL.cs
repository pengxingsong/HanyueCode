using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using AnJie.ERP.DataAccess;
using System;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 交接单主表
    /// </summary>
    public class HandoverBLL : RepositoryFactory<HandoverEntity>
    {
        /// <summary>
        /// 获取未关闭的交接单信息
        /// </summary>
        /// <param name="shipTypeId"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public HandoverEntity GetUnPrintHandoverByShipType(string shipTypeId, string createUserId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM Handover WHERE ShipTypeId = @ShipTypeId And CreateUserId=@CreateUserId And IsPrinted = 0");
            parameter.Add(DbFactory.CreateDbParameter("@ShipTypeId", shipTypeId));
            parameter.Add(DbFactory.CreateDbParameter("@CreateUserId", createUserId));
            return DataFactory.Database().FindEntityBySql<HandoverEntity>(strSql.ToString(), parameter.ToArray());
        }

        public HandoverEntity GetEntity(string handoverId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM Handover WHERE HandoverId = @HandoverId");
            parameter.Add(DbFactory.CreateDbParameter("@HandoverId", handoverId));
            return DataFactory.Database().FindEntityBySql<HandoverEntity>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 交接单明细
        /// </summary>
        /// <param name="handoverId"></param>
        /// <returns></returns>
        public List<HandoverItemEntity> GetHandOverItemList(string handoverId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM Handover_Item WHERE HandoverId = @HandoverId");
            parameter.Add(DbFactory.CreateDbParameter("@HandoverId", handoverId));
            return DataFactory.Database().FindListBySql<HandoverItemEntity>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressNum"></param>
        /// <returns></returns>
        public HandoverItemEntity GetHandOverItem(string expressNum)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM Handover_Item WHERE ExpressNum=@ExpressNum");
            parameter.Add(DbFactory.CreateDbParameter("@ExpressNum", expressNum));
            return DataFactory.Database().FindEntityBySql<HandoverItemEntity>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 是否已扫描
        /// </summary>
        /// <param name="expressNum"></param>
        /// <returns></returns>
        public bool IsExist(string expressNum)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT Count(1) FROM Handover_Item WHERE ExpressNum=@ExpressNum");
            parameter.Add(DbFactory.CreateDbParameter("@ExpressNum", expressNum));
            return DataFactory.Database().ExecuteBySql(strSql, parameter.ToArray()) > 0;
        }

        /// <summary>
        /// 取消扫描
        /// </summary>
        /// <param name="handoverId"></param>
        /// <param name="expressNum"></param>
        /// <returns></returns>
        public bool CancelItem(string handoverId, string expressNum)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"DELETE item
                            FROM    Handover_Item item
                                    INNER JOIN dbo.Handover mater ON mater.HandoverId = item.HandoverId
                            WHERE   mater.IsPrinted = 0
                                    AND item.HandoverId = @HandoverId
                                    AND item.ExpressNum = @ExpressNum");
            parameter.Add(DbFactory.CreateDbParameter("@HandoverId", handoverId));
            parameter.Add(DbFactory.CreateDbParameter("@ExpressNum", expressNum));
            return DataFactory.Database().ExecuteBySql(strSql, parameter.ToArray()) > 0;
        }

        /// <summary>
        /// 关闭交接单
        /// </summary>
        /// <param name="handover"></param>
        /// <returns></returns>
        public bool CloseHandover(HandoverEntity handover)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE dbo.Handover
                            SET IsPrinted = 1, PrintTime = GetDate()
                            WHERE HandoverId = @HandoverId");
            var parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter("@HandoverId", handover.HandoverId)
            };
            return Repository().ExecuteBySql(strSql, parameter.ToArray()) > 0;
        }
    }
}