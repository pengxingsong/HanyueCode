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
    /// ������ʽ����
    /// </summary>
    public class ShipTypeBLL : RepositoryFactory<ShipTypeEntity>
    {
        /// <summary>
        /// ������־
        /// </summary>
        private static LogHelper _log = LogFactory.GetLogger("ShipTypeBLL");
        
        /// <summary>
        /// ��ȡ������ʽ�б�
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public List<ShipTypeEntity> GetPageList(ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM ShipType WHERE 1=1 ");
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        /// <summary>
        /// ��ȡ������ʽ�б�
        /// </summary>
        /// <returns></returns>
        public List<ShipTypeEntity> GetList()
        {
            return Repository().FindList();
        }

        /// <summary>
        /// ��ȡ������ʽ�����б�
        /// </summary>
        /// <param name="shipTypeId">������ʽ����</param>
        /// <returns></returns>
        public List<ShipTypeConfigEntity> GetShipTypeConfigList(string shipTypeId)
        {
            return DataFactory.Database().FindList<ShipTypeConfigEntity>("ShipTypeId", shipTypeId);
        }        

        /// <summary>
        /// �ύ������ʽ�����������༭��
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="entity">�ӿڶ���</param>
        /// <param name="parameterJson">�ӿڲ���</param>
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
                //��Ӳ���
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