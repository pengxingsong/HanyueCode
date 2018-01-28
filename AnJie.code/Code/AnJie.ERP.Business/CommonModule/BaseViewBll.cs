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
    /// 视图设置表
    /// </summary>
    public class BaseViewBll : RepositoryFactory<Base_View>
    {
        /// <summary>
        /// 根据模块Id获取视图列表
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public List<Base_View> GetViewList(string moduleId)
        {
            StringBuilder whereSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            whereSql.Append(" AND ModuleId = @ModuleId order by sortcode asc");
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", moduleId));
            return Repository().FindList(whereSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 视图提交（新增、编辑、删除）
        /// </summary>
        /// <param name="keyValue">判断新增、修改</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="viewJson">视图Json</param>
        /// <param name="viewWhereJson">视图条件Json</param>
        /// <returns></returns>
        public int SubmitForm(string keyValue, string moduleId, string viewJson, string viewWhereJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                List<Base_View> viewList = viewJson.JonsToList<Base_View>();
                List<Base_ViewWhere> viewWhereList = viewWhereJson.JonsToList<Base_ViewWhere>();
                if (!string.IsNullOrEmpty(keyValue))
                {
                    database.Delete<Base_View>("ModuleId", moduleId, isOpenTrans);
                    database.Delete<Base_ViewWhere>("ModuleId", moduleId, isOpenTrans);
                }
                foreach (Base_View baseView in viewList)
                {
                    if (string.IsNullOrEmpty(baseView.ViewId))
                        baseView.ViewId = CommonHelper.GetGuid;
                    baseView.ModuleId = moduleId;
                    baseView.ParentId = "0";
                    database.Insert(baseView, isOpenTrans);
                }
                foreach (Base_ViewWhere baseViewwhere in viewWhereList)
                {
                    if (string.IsNullOrEmpty(baseViewwhere.ViewWhereId))
                        baseViewwhere.ViewWhereId = CommonHelper.GetGuid;
                    baseViewwhere.ModuleId = moduleId;
                    database.Insert(baseViewwhere, isOpenTrans);
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