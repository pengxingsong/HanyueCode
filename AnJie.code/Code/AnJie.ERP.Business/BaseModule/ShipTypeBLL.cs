using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System;
using System.Data;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 物流方式管理
    /// </summary>
    public class ShipTypeBLL : RepositoryFactory<ShipTypeEntity>
    {
        /// <summary>
        /// 调试日志
        /// </summary>
        private static LogHelper _log = LogFactory.GetLogger("ShipTypeBLL");
        
        /// <summary>
        /// 获取物流方式列表
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<ShipTypeEntity> GetPageList(ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM ShipType WHERE 1=1 ");
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        /// <summary>
        /// 获取物流方式列表
        /// </summary>
        /// <returns></returns>
        public List<ShipTypeEntity> GetList()
        {
            return Repository().FindList();
        }

        /// <summary>
        /// 获取物流方式参数列表
        /// </summary>
        /// <param name="shipTypeId">物流方式主键</param>
        /// <returns></returns>
        public List<ShipTypeConfigEntity> GetShipTypeConfigList(string shipTypeId)
        {
            return DataFactory.Database().FindList<ShipTypeConfigEntity>("ShipTypeId", shipTypeId);
        }        

        /// <summary>
        /// 提交物流方式表单（新增、编辑）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">接口对象</param>
        /// <param name="parameterJson">接口参数</param>
        /// <returns></returns>
        public int SubmitShipTypeForm(string keyValue, ShipTypeEntity entity, string parameterJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    database.Delete<ShipTypeConfigEntity>("ShipTypeId", keyValue, isOpenTrans);
                    entity.Modify(keyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                }
                //添加参数
                List<ShipTypeConfigEntity> configList = parameterJson.JonsToList<ShipTypeConfigEntity>();
                foreach (ShipTypeConfigEntity config in configList)
                {
                    if (!string.IsNullOrEmpty(config.ConfigField))
                    {
                        config.Create();
                        config.ShipTypeId = entity.ShipTypeId;
                        database.Insert(config, isOpenTrans);
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

        public DataTable SearchShipTypeList(string keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@"SELECT
                                    ShipTypeId, Code, ShipTypeName
                                FROM
                                    dbo.ShipType
                                WHERE
                                    Enabled = 1
                                    AND Code LIKE @keyword
                                    OR ShipTypeName LIKE @keyword ");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            else
            {
                strSql.Append(@"SELECT
                                    ShipTypeId, Code, ShipTypeName
                                FROM
                                    dbo.ShipType
                                WHERE
                                    Enabled = 1");
            }
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}