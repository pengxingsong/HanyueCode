using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnJie.ERP.Business
{
    public class ForeignSysApiMgrBLL : RepositoryFactory<ForeignSysApiEntity>
    {
        /// <summary>
        /// 调试日志
        /// </summary>
        private static LogHelper _log = LogFactory.GetLogger("ForeignSysApiMgrBLL");


        #region 数据处理

        /// <summary>
        /// 数据表名称
        /// </summary>
        private readonly string _dbTableName = "Base_ForeignSysApi";

        /// <summary>
        /// 默认数据返回字段
        /// </summary>
        private readonly string _returnFields = "ID,PlatformID,PlatformName,ApiMethod,Tag,IsCollectFee,IsAuth,Description,IsDisabled,IsDeleted,CreateDate,CreateUserName,ModifyDate,ModifyUserName ";

        /// <summary>
        /// 生成查询sql语句
        /// </summary>
        ///  /// <param name="whereSql">条件语句,开头需加AND 或 OR判段词</param>
        /// <param name="returnFields">返回字段,以','隔开.</param>
        /// <returns></returns>
        private string GetSelectSqlStr(string whereSql, string returnFields)
        {
            var sqlStr = string.Format("SELECT {0} FROM {1} WHERE 1=1 {2}", string.IsNullOrWhiteSpace(returnFields) ? _returnFields : returnFields, _dbTableName, whereSql);
            return sqlStr;
        }

        #endregion


        public List<ForeignSysApiEntity> GetList_ByPlatformID(string platformID)
        {
            StringBuilder strWhereSql = new StringBuilder();
            strWhereSql.AppendFormat(" AND PlatformID=@PlatformID");
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@PlatformID", platformID));
            var result = Repository().FindListBySql(GetSelectSqlStr(strWhereSql.ToString(), ""), parameter.ToArray());
            return result;
        }


        public bool SaveEntity(List<ForeignSysApiEntity> list)
        {
            try
            {
                if (list != null && list.Count() > 0)
                {
                    var insetDataList = list.Where(ent => string.IsNullOrWhiteSpace(ent.ID)).ToList();
                    var updateDataList = list.Where(ent => !string.IsNullOrWhiteSpace(ent.ID)).ToList();
                    insetDataList.ForEach((ent) =>
                    {
                        ent.Create();
                    });
                    updateDataList.ForEach((ent) =>
                    {
                        ent.Modify(ent.ID);
                    });
                    if (insetDataList != null && insetDataList.Count > 0)
                        Repository().Insert(insetDataList);
                    if (updateDataList != null && updateDataList.Count > 0)
                        Repository().Update(updateDataList);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



    }
}
