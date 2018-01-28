using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 定义通用的功能接口
    /// </summary>
    public interface IBaseManager
    {
        #region 获取最大编号+1
        /// <summary>
        /// 获取最大编号
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        object GetSortCode<T>(string fieldName) where T : new();
        #endregion

        #region 验证对象值不能重复
        /// <summary>
        /// 验证对象值不能重复
        /// </summary>
        /// <param name="tablename">实体类</param>
        /// <param name="fieldname">属性字段</param>
        /// <param name="fieldvalue">属性字段值</param>
        /// <param name="keyfield">主键字段</param>
        /// <param name="keyvalue">主键字段值</param>
        /// <returns></returns>
        bool FieldExist(string tablename, string fieldname, string fieldvalue, string keyfield, string keyvalue);
        #endregion
    }
}
