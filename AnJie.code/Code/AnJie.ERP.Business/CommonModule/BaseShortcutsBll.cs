using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 首页快捷方式
    /// </summary>
    public class BaseShortcutsBll : RepositoryFactory<Base_Shortcuts>
    {
        /// <summary>
        /// 获取首页快捷方式列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public List<Base_Module> GetShortcutList(string userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    Base_Module M
                                    RIGHT JOIN Base_Shortcuts S ON s.ModuleId = M.ModuleId
                            WHERE   S.CreateUserId = @CreateUserId
                            ORDER BY M.SortCode");
            var parameter = new List<DbParameter> {DbFactory.CreateDbParameter("@CreateUserId", userId)};
            return DataFactory.Database().FindListBySql<Base_Module>(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 快捷方式（新增、编辑、删除）
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int SubmitForm(string moduleId, string userId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string[] array = moduleId.Split(',');
                database.Delete<Base_Shortcuts>("CreateUserId", userId, isOpenTrans);
                foreach (string item in array)
                {
                    if (item.Length>0)
                    {
                        Base_Shortcuts entity = new Base_Shortcuts();
                        entity.Create();
                        entity.ModuleId = item;
                        entity.CreateUserId = userId;
                        database.Insert(entity);
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
    }
}