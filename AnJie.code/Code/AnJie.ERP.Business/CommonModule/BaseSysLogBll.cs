using AnJie.ERP.DataAccess;
using AnJie.ERP.Entity;
using AnJie.ERP.Repository;
using AnJie.ERP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Threading;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 系统日志表
    /// </summary>
    public class BaseSysLogBll : RepositoryFactory<Base_SysLog>
    {
        #region 静态实例化

        private static BaseSysLogBll item;

        public static BaseSysLogBll Instance
        {
            get
            {
                if (item == null)
                {
                    item = new BaseSysLogBll();
                }
                return item;
            }
        }

        #endregion

        public Base_SysLog SysLog = new Base_SysLog();

        #region 写入操作日志

        /// <summary>
        /// 写入作业日志
        /// </summary>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="OperationType">操作类型</param>
        /// <param name="Status">状态</param>
        /// <param name="Remark">操作说明</param>
        /// <returns></returns>
        public void WriteLog(string ObjectId, OperationType OperationType, string Status, string Remark = "")
        {
            SysLog.SysLogId = CommonHelper.GetGuid;
            SysLog.ObjectId = ObjectId;
            SysLog.LogType = CommonHelper.GetString((int) OperationType);
            if (ManageProvider.Provider.IsOverdue())
            {
                SysLog.IPAddress = ManageProvider.Provider.Current().IPAddress;
                SysLog.IPAddressName = ManageProvider.Provider.Current().IPAddressName;
                SysLog.CompanyId = ManageProvider.Provider.Current().CompanyId;
                SysLog.DepartmentId = ManageProvider.Provider.Current().DepartmentId;
                SysLog.CreateUserId = ManageProvider.Provider.Current().UserId;
                SysLog.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            SysLog.ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            SysLog.Remark = Remark;
            SysLog.Status = Status;
            ThreadPool.QueueUserWorkItem(new WaitCallback(WriteLogUsu), SysLog); //放入异步执行
        }

        private void WriteLogUsu(object obSysLog)
        {
            Base_SysLog VSysLog = (Base_SysLog) obSysLog;
            DataFactory.Database().Insert(VSysLog);
        }

        /// <summary>
        /// 写入作业日志（新增操作）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="OperationType">操作类型</param>
        /// <param name="Status">状态</param>
        /// <param name="Remark">操作说明</param>
        /// <returns></returns>
        public void WriteLog<T>(T entity, OperationType OperationType, string Status, string Remark = "")
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                SysLog.SysLogId = CommonHelper.GetGuid;
                SysLog.ObjectId = DatabaseCommon.GetKeyFieldValue(entity).ToString();
                SysLog.LogType = CommonHelper.GetString((int) OperationType);
                SysLog.IPAddress = ManageProvider.Provider.Current().IPAddress;
                SysLog.IPAddressName = ManageProvider.Provider.Current().IPAddressName;
                SysLog.CompanyId = ManageProvider.Provider.Current().CompanyId;
                SysLog.DepartmentId = ManageProvider.Provider.Current().DepartmentId;
                SysLog.CreateUserId = ManageProvider.Provider.Current().UserId;
                SysLog.CreateUserName = ManageProvider.Provider.Current().UserName;
                SysLog.ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
                if (Remark == "")
                {
                    SysLog.Remark = DatabaseCommon.GetClassName<T>();
                }
                SysLog.Remark = Remark;
                SysLog.Status = Status;
                database.Insert(SysLog, isOpenTrans);
                //添加日志详细信息
                Type objTye = typeof(T);
                foreach (PropertyInfo pi in objTye.GetProperties())
                {
                    object value = pi.GetValue(entity, null);
                    if (value != null && value.ToString() != "&nbsp;" && value.ToString() != "")
                    {

                        Base_SysLogDetail syslogdetail = new Base_SysLogDetail();
                        syslogdetail.SysLogDetailId = CommonHelper.GetGuid;
                        syslogdetail.SysLogId = SysLog.SysLogId;
                        syslogdetail.PropertyField = pi.Name;
                        syslogdetail.PropertyName = DatabaseCommon.GetFieldText(pi);
                        syslogdetail.NewValue = "" + value + "";
                        database.Insert(syslogdetail, isOpenTrans);
                    }
                }
                database.Commit();
            }
            catch
            {
                database.Rollback();
            }
        }

        /// <summary>
        /// 写入作业日志（更新操作）
        /// </summary>
        /// <param name="oldObj">旧实体对象</param>
        /// <param name="newObj">新实体对象</param>
        /// <param name="OperationType">操作类型</param>
        /// <param name="Status">状态</param>
        /// <param name="Remark">操作说明</param>
        /// <returns></returns>
        public void WriteLog<T>(T oldObj, T newObj, OperationType OperationType, string Status, string Remark = "")
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                SysLog.SysLogId = CommonHelper.GetGuid;
                SysLog.ObjectId = DatabaseCommon.GetKeyFieldValue(newObj).ToString();
                SysLog.LogType = CommonHelper.GetString((int) OperationType);
                SysLog.IPAddress = ManageProvider.Provider.Current().IPAddress;
                SysLog.IPAddressName = ManageProvider.Provider.Current().IPAddressName;
                SysLog.CompanyId = ManageProvider.Provider.Current().CompanyId;
                SysLog.DepartmentId = ManageProvider.Provider.Current().DepartmentId;
                SysLog.CreateUserId = ManageProvider.Provider.Current().UserId;
                SysLog.CreateUserName = ManageProvider.Provider.Current().UserName;
                SysLog.ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
                if (Remark == "")
                {
                    SysLog.Remark = DatabaseCommon.GetClassName<T>();
                }
                SysLog.Remark = Remark;
                SysLog.Status = Status;
                database.Insert(SysLog, isOpenTrans);
                //添加日志详细信息
                Type objTye = typeof(T);
                foreach (PropertyInfo pi in objTye.GetProperties())
                {
                    object oldVal = pi.GetValue(oldObj, null);
                    object newVal = pi.GetValue(newObj, null);
                    if (!Equals(oldVal, newVal))
                    {
                        if (oldVal != null && oldVal.ToString() != "&nbsp;" && oldVal.ToString() != "" && newVal != null &&
                            newVal.ToString() != "&nbsp;" && newVal.ToString() != "")
                        {
                            Base_SysLogDetail syslogdetail = new Base_SysLogDetail();
                            syslogdetail.SysLogDetailId = CommonHelper.GetGuid;
                            syslogdetail.SysLogId = SysLog.SysLogId;
                            syslogdetail.PropertyField = pi.Name;
                            syslogdetail.PropertyName = DatabaseCommon.GetFieldText(pi);
                            syslogdetail.NewValue = "" + newVal + "";
                            syslogdetail.OldValue = "" + oldVal + "";
                            database.Insert(syslogdetail, isOpenTrans);
                        }
                    }
                }
                database.Commit();
            }
            catch
            {
                database.Rollback();
            }
        }

        /// <summary>
        /// 写入作业日志（删除操作）
        /// </summary>
        /// <param name="oldObj">旧实体对象</param>
        /// <param name="KeyValue">对象主键</param>
        /// <param name="Status">状态</param>
        /// <param name="Remark">操作说明</param>
        public void WriteLog<T>(string[] KeyValue, string Status, string Remark = "") where T : new()
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                foreach (var item in KeyValue)
                {
                    T Oldentity = database.FindEntity<T>(item.ToString());
                    SysLog.SysLogId = CommonHelper.GetGuid;
                    SysLog.ObjectId = item;
                    SysLog.LogType = CommonHelper.GetString((int) OperationType.Delete);
                    SysLog.IPAddress = ManageProvider.Provider.Current().IPAddress;
                    SysLog.IPAddressName = ManageProvider.Provider.Current().IPAddressName;
                    SysLog.CompanyId = ManageProvider.Provider.Current().CompanyId;
                    SysLog.DepartmentId = ManageProvider.Provider.Current().DepartmentId;
                    SysLog.CreateUserId = ManageProvider.Provider.Current().UserId;
                    SysLog.CreateUserName = ManageProvider.Provider.Current().UserName;
                    SysLog.ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
                    if (Remark == "")
                    {
                        SysLog.Remark = DatabaseCommon.GetClassName<T>();
                    }
                    SysLog.Remark = Remark;
                    SysLog.Status = Status;
                    database.Insert(SysLog, isOpenTrans);
                    //添加日志详细信息
                    Type objTye = typeof(T);
                    foreach (PropertyInfo pi in objTye.GetProperties())
                    {
                        object value = pi.GetValue(Oldentity, null);
                        if (value != null && value.ToString() != "&nbsp;" && value.ToString() != "")
                        {
                            Base_SysLogDetail syslogdetail = new Base_SysLogDetail();
                            syslogdetail.SysLogDetailId = CommonHelper.GetGuid;
                            syslogdetail.SysLogId = SysLog.SysLogId;
                            syslogdetail.PropertyField = pi.Name;
                            syslogdetail.PropertyName = DatabaseCommon.GetFieldText(pi);
                            syslogdetail.NewValue = "" + value + "";
                            database.Insert(syslogdetail, isOpenTrans);
                        }
                    }
                }
                database.Commit();
            }
            catch
            {
                database.Rollback();
            }
        }

        #endregion

        /// <summary>
        /// 清空操作日志
        /// </summary>
        /// <param name="keepTime"></param>
        /// <returns></returns>
        public int RemoveLog(string keepTime)
        {
            StringBuilder strSql = new StringBuilder();
            DateTime createDate = DateTime.Now;
            if (keepTime == "7") //保留近一周
            {
                createDate = DateTime.Now.AddDays(-7);
            }
            else if (keepTime == "1") //保留近一个月
            {
                createDate = DateTime.Now.AddMonths(-1);
            }
            else if (keepTime == "3") //保留近三个月
            {
                createDate = DateTime.Now.AddMonths(-3);
            }
            if (keepTime == "0") //不保留，全部删除
            {
                strSql.Append("DELETE FROM Base_SysLog");
                return DataFactory.Database().ExecuteBySql(strSql);
            }
            else
            {
                strSql.Append("DELETE FROM Base_SysLog WHERE 1=1 ");
                strSql.Append("AND CreateDate <= @CreateDate");
                var parameter = new List<DbParameter> {DbFactory.CreateDbParameter("@CreateDate", createDate)};
                return DataFactory.Database().ExecuteBySql(strSql, parameter.ToArray());
            }
        }

        /// <summary>
        /// 获取系统日志列表
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <param name="parameterJson">搜索条件</param>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public List<Base_SysLog> GetPageList(string moduleId, string parameterJson, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    l.SysLogId, l.ObjectId, l.LogType, l.IPAddress,
                                                l.IPAddressName, c.FullName AS CompanyId,
                                                d.FullName AS DepartmentId, l.CreateDate, l.CreateUserId,
                                                l.CreateUserName, m.FullName AS ModuleId, l.Remark,
                                                l.Status
                                      FROM      Base_SysLog l
                                                LEFT JOIN Base_Department d ON d.DepartmentId = l.DepartmentId
                                                LEFT JOIN Base_Company c ON c.CompanyId = l.CompanyId
                                                LEFT JOIN Base_Module m ON m.ModuleId = l.ModuleId
                                    ) A
                            WHERE   1 = 1");
            if (!string.IsNullOrEmpty(moduleId))
            {
                strSql.Append(" AND ModuleId = @ModuleId");
                parameter.Add(DbFactory.CreateDbParameter("@ModuleId", moduleId));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        /// <summary>
        /// 获取系统日志明细列表
        /// </summary>
        /// <param name="sysLogId">系统日志主键</param>
        /// <returns></returns>
        public List<Base_SysLogDetail> GetSysLogDetailList(string sysLogId)
        {
            string whereSql = " AND SysLogId = @SysLogId Order By CreateDate ASC";
            List<DbParameter> parameter = new List<DbParameter> {DbFactory.CreateDbParameter("@SysLogId", sysLogId)};
            return DataFactory.Database().FindList<Base_SysLogDetail>(whereSql, parameter.ToArray());
        }
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 登陆
        /// </summary>
        Login = 0,

        /// <summary>
        /// 新增
        /// </summary>
        Add = 1,

        /// <summary>
        /// 修改
        /// </summary>
        Update = 2,

        /// <summary>
        /// 删除
        /// </summary>
        Delete = 3,

        /// <summary>
        /// 其他
        /// </summary>
        Other = 4,

        /// <summary>
        /// 访问
        /// </summary>
        Visit = 5,

        /// <summary>
        /// 离开
        /// </summary>
        Leave = 6,

        /// <summary>
        /// 查询
        /// </summary>
        Query = 7,

        /// <summary>
        /// 安全退出
        /// </summary>
        Exit = 8,
    }
}