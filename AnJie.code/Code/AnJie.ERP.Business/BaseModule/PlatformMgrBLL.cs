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
    public  class PlatformMgrBLL: RepositoryFactory<PlatformEntity>
    {
        /// <summary>
        /// 调试日志
        /// </summary>
        private static LogHelper _log = LogFactory.GetLogger("PlatformBLL");


        #region 数据处理

        /// <summary>
        /// 数据表名称
        /// </summary>
        private readonly string _dbTableName = "Base_Platform";

        /// <summary>
        /// 默认数据返回字段
        /// </summary>
        private readonly string _returnFields = "ID,Name,Code,Remark,ApiUrl,SandboxApiUrl,ApiUrlType,IsDisabled,IsDeleted,CreateDate,CreateUserName,ModifyDate,ModifyUserName ";

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


        /// <summary>
        /// 获取平台列表
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<PlatformEntity> GetPageList(ref JqGridParam jqgridparam)
        {
            var strSql = GetSelectSqlStr(" AND IsDeleted=@IsDeleted ", "");
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@IsDeleted", 0));
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        /// <summary>
        /// 提交平台表单(新增，编辑)
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="parameterJson"></param>
        /// <returns></returns>
        public int SubmitPlatformForm(string keyValue, PlatformEntity entity, string parameterJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
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

        /// <summary>
        /// 检查平台Code唯一
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int CheckCodeUnique(string code)
        {
            return Repository().FindCount("Code",code);
        }




    }
}
