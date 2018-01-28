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
    /// ϵͳ��־��
    /// </summary>
    public class BaseSysLogBll : RepositoryFactory<Base_SysLog>
    {
        #region ��̬ʵ����

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

        #region д�������־

        /// <summary>
        /// д����ҵ��־
        /// </summary>
        /// <param name="ObjectId">��������</param>
        /// <param name="OperationType">��������</param>
        /// <param name="Status">״̬</param>
        /// <param name="Remark">����˵��</param>
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
            ThreadPool.QueueUserWorkItem(new WaitCallback(WriteLogUsu), SysLog); //�����첽ִ��
        }

        private void WriteLogUsu(object obSysLog)
        {
            Base_SysLog VSysLog = (Base_SysLog) obSysLog;
            DataFactory.Database().Insert(VSysLog);
        }

        /// <summary>
        /// д����ҵ��־������������
        /// </summary>
        /// <param name="entity">ʵ�����</param>
        /// <param name="OperationType">��������</param>
        /// <param name="Status">״̬</param>
        /// <param name="Remark">����˵��</param>
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
                //�����־��ϸ��Ϣ
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
        /// д����ҵ��־�����²�����
        /// </summary>
        /// <param name="oldObj">��ʵ�����</param>
        /// <param name="newObj">��ʵ�����</param>
        /// <param name="OperationType">��������</param>
        /// <param name="Status">״̬</param>
        /// <param name="Remark">����˵��</param>
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
                //�����־��ϸ��Ϣ
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
        /// д����ҵ��־��ɾ��������
        /// </summary>
        /// <param name="oldObj">��ʵ�����</param>
        /// <param name="KeyValue">��������</param>
        /// <param name="Status">״̬</param>
        /// <param name="Remark">����˵��</param>
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
                    //�����־��ϸ��Ϣ
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
        /// ��ղ�����־
        /// </summary>
        /// <param name="keepTime"></param>
        /// <returns></returns>
        public int RemoveLog(string keepTime)
        {
            StringBuilder strSql = new StringBuilder();
            DateTime createDate = DateTime.Now;
            if (keepTime == "7") //������һ��
            {
                createDate = DateTime.Now.AddDays(-7);
            }
            else if (keepTime == "1") //������һ����
            {
                createDate = DateTime.Now.AddMonths(-1);
            }
            else if (keepTime == "3") //������������
            {
                createDate = DateTime.Now.AddMonths(-3);
            }
            if (keepTime == "0") //��������ȫ��ɾ��
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
        /// ��ȡϵͳ��־�б�
        /// </summary>
        /// <param name="moduleId">ģ��ID</param>
        /// <param name="parameterJson">��������</param>
        /// <param name="jqgridparam">��ҳ����</param>
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
        /// ��ȡϵͳ��־��ϸ�б�
        /// </summary>
        /// <param name="sysLogId">ϵͳ��־����</param>
        /// <returns></returns>
        public List<Base_SysLogDetail> GetSysLogDetailList(string sysLogId)
        {
            string whereSql = " AND SysLogId = @SysLogId Order By CreateDate ASC";
            List<DbParameter> parameter = new List<DbParameter> {DbFactory.CreateDbParameter("@SysLogId", sysLogId)};
            return DataFactory.Database().FindList<Base_SysLogDetail>(whereSql, parameter.ToArray());
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// ��½
        /// </summary>
        Login = 0,

        /// <summary>
        /// ����
        /// </summary>
        Add = 1,

        /// <summary>
        /// �޸�
        /// </summary>
        Update = 2,

        /// <summary>
        /// ɾ��
        /// </summary>
        Delete = 3,

        /// <summary>
        /// ����
        /// </summary>
        Other = 4,

        /// <summary>
        /// ����
        /// </summary>
        Visit = 5,

        /// <summary>
        /// �뿪
        /// </summary>
        Leave = 6,

        /// <summary>
        /// ��ѯ
        /// </summary>
        Query = 7,

        /// <summary>
        /// ��ȫ�˳�
        /// </summary>
        Exit = 8,
    }
}