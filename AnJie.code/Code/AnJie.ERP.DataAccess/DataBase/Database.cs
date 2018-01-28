using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using AnJie.ERP.DataAccess.DbExpand;

namespace AnJie.ERP.DataAccess
{
    /// <summary>
    ///     操作数据库基类
    /// </summary>
    public class Database : IDatabase, IDisposable
    {
        #region SqlBulkCopy大批量数据插入

        /// <summary>
        ///     大批量数据插入
        /// </summary>
        /// <param name="datatable">资料表</param>
        /// <returns></returns>
        public bool BulkInsert(DataTable datatable)
        {
            return false;
        }

        #endregion

        #region 构造函数

        public static string ConnString { get; set; }

        /// <summary>
        ///     构造方法
        /// </summary>
        public Database(string connstring)
        {
            var dbhelper = new DbHelper(connstring);
        }

        /// <summary>
        ///     数据库连接对象
        /// </summary>
        private DbConnection DbConnection { get; set; }

        /// <summary>
        ///     事务对象
        /// </summary>
        private DbTransaction IsOpenTrans { get; set; }

        /// <summary>
        ///     是否已在事务之中
        /// </summary>
        public bool InTransaction { get; set; }

        /// <summary>
        ///     事务开始
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTrans()
        {
            if (!InTransaction)
            {
                DbConnection = DbFactory.CreateDbConnection(DbHelper.ConnectionString);
                if (DbConnection.State == ConnectionState.Closed)
                {
                    DbConnection.Open();
                }
                InTransaction = true;
                IsOpenTrans = DbConnection.BeginTransaction();
            }
            return IsOpenTrans;
        }

        /// <summary>
        ///     提交事务
        /// </summary>
        public void Commit()
        {
            if (InTransaction)
            {
                InTransaction = false;
                IsOpenTrans.Commit();
                Close();
            }
        }

        /// <summary>
        ///     回滚事务
        /// </summary>
        public void Rollback()
        {
            if (InTransaction)
            {
                InTransaction = false;
                IsOpenTrans.Rollback();
                Close();
            }
        }

        /// <summary>
        ///     关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (DbConnection != null)
            {
                DbConnection.Close();
                DbConnection.Dispose();
            }
            if (IsOpenTrans != null)
            {
                IsOpenTrans.Dispose();
            }
            DbConnection = null;
            IsOpenTrans = null;
        }

        /// <summary>
        ///     内存回收
        /// </summary>
        public void Dispose()
        {
            if (DbConnection != null)
            {
                DbConnection.Dispose();
            }
            if (IsOpenTrans != null)
            {
                IsOpenTrans.Dispose();
            }
        }

        #endregion

        #region 执行SQL语句

        /// <summary>
        ///     执行SQL语句
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public int ExecuteBySql(StringBuilder strSql)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString());
        }

        /// <summary>
        ///     执行SQL语句
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int ExecuteBySql(StringBuilder strSql, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString());
        }

        /// <summary>
        ///     执行SQL语句
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public int ExecuteBySql(StringBuilder strSql, DbParameter[] parameters)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        ///     执行SQL语句
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int ExecuteBySql(StringBuilder strSql, DbParameter[] parameters, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion

        #region 执行存储过程

        /// <summary>
        ///     执行存储过程
        /// </summary>
        /// <param name="procName">存储过程</param>
        /// <returns></returns>
        public int ExecuteByProc(string procName)
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, procName);
        }

        /// <summary>
        ///     执行存储过程
        /// </summary>
        /// <param name="procName">存储过程</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int ExecuteByProc(string procName, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.StoredProcedure, procName);
        }

        /// <summary>
        ///     执行存储过程
        /// </summary>
        /// <param name="procName">存储过程</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public int ExecuteByProc(string procName, DbParameter[] parameters)
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, procName, parameters);
        }

        /// <summary>
        ///     执行存储过程
        /// </summary>
        /// <param name="procName">存储过程</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int ExecuteByProc(string procName, DbParameter[] parameters, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.StoredProcedure, procName, parameters);
        }

        #endregion

        #region 插入数据

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="entity">实体类对象</param>
        /// <returns></returns>
        public int Insert<T>(T entity)
        {
            object val = 0;
            var strSql = DatabaseCommon.InsertSql(entity);
            var parameter = DatabaseCommon.GetParameter(entity);
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter);
            return Convert.ToInt32(val);
        }

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="entity">实体类对象</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Insert<T>(T entity, DbTransaction isOpenTrans)
        {
            object val = 0;
            var strSql = DatabaseCommon.InsertSql(entity);
            var parameter = DatabaseCommon.GetParameter(entity);
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter);
            return Convert.ToInt32(val);
        }

        /// <summary>
        ///     批量插入数据
        /// </summary>
        /// <param name="entity">实体类对象</param>
        /// <returns></returns>
        public int Insert<T>(List<T> entity)
        {
            object val = 0;
            var isOpenTrans = BeginTrans();
            try
            {
                foreach (var item in entity)
                {
                    Insert(item, isOpenTrans);
                }
                Commit();
                val = 1;
            }
            catch (Exception ex)
            {
                Rollback();
                Close();
                val = -1;
                throw;
            }
            return Convert.ToInt32(val);
        }

        /// <summary>
        ///     批量插入数据
        /// </summary>
        /// <param name="entity">实体类对象</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Insert<T>(List<T> entity, DbTransaction isOpenTrans)
        {
            object val = 0;
            try
            {
                foreach (var item in entity)
                {
                    Insert(item, isOpenTrans);
                }
                val = 1;
            }
            catch (Exception)
            {
                val = -1;
                throw;
            }
            return Convert.ToInt32(val);
        }

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">哈希表键值</param>
        /// <returns></returns>
        public int Insert(string tableName, Hashtable ht)
        {
            object val = 0;
            var strSql = DatabaseCommon.InsertSql(tableName, ht);
            var parameter = DatabaseCommon.GetParameter(ht);
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter);
            return Convert.ToInt32(val);
        }

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">哈希表键值</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Insert(string tableName, Hashtable ht, DbTransaction isOpenTrans)
        {
            object val = 0;
            var strSql = DatabaseCommon.InsertSql(tableName, ht);
            var parameter = DatabaseCommon.GetParameter(ht);
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter);
            return Convert.ToInt32(val);
        }

        #endregion

        #region 修改数据

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int Update<T>(T entity)
        {
            object val = 0;
            var strSql = DatabaseCommon.UpdateSql(entity);
            var parameter = DatabaseCommon.GetParameter(entity);
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter);
            return Convert.ToInt32(val);
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Update<T>(T entity, DbTransaction isOpenTrans)
        {
            object val = 0;
            var strSql = DatabaseCommon.UpdateSql(entity);
            var parameter = DatabaseCommon.GetParameter(entity);
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter);
            return Convert.ToInt32(val);
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public int Update<T>(string propertyName, string propertyValue)
        {
            object val = 0;
            var strSql = new StringBuilder();
            var sb = new StringBuilder();
            sb.Append("Update ");
            sb.Append(DatabaseCommon.GetTableName<T>());
            sb.Append(" Set ");
            sb.Append(propertyName);
            sb.Append("=");
            sb.Append(DbHelper.DbParmChar + propertyName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue));
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return Convert.ToInt32(val);
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Update<T>(string propertyName, string propertyValue, DbTransaction isOpenTrans)
        {
            object val = 0;
            var strSql = new StringBuilder();
            var sb = new StringBuilder();
            sb.Append("Update ");
            sb.Append(DatabaseCommon.GetTableName<T>());
            sb.Append(" Set ");
            sb.Append(propertyName);
            sb.Append("=");
            sb.Append(DbHelper.DbParmChar + propertyName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue));
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray());
            return Convert.ToInt32(val);
        }

        /// <summary>
        ///     批量修改数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int Update<T>(List<T> entity)
        {
            object val = 0;
            var isOpenTrans = BeginTrans();
            try
            {
                foreach (var item in entity)
                {
                    Update(item, isOpenTrans);
                }
                Commit();
                val = 1;
            }
            catch (Exception ex)
            {
                Rollback();
                Close();
                val = -1;
                throw ex;
            }
            return Convert.ToInt32(val);
        }

        /// <summary>
        ///     批量修改数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Update<T>(List<T> entity, DbTransaction isOpenTrans)
        {
            object val = 0;
            try
            {
                foreach (var item in entity)
                {
                    Update(item, isOpenTrans);
                }
                val = 1;
            }
            catch (Exception ex)
            {
                val = -1;
                throw ex;
            }
            return Convert.ToInt32(val);
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">哈希表键值</param>
        /// <param name="propertyName">主键字段</param>
        /// <returns></returns>
        public int Update(string tableName, Hashtable ht, string propertyName)
        {
            object val = 0;
            var strSql = DatabaseCommon.UpdateSql(tableName, ht, propertyName);
            var parameter = DatabaseCommon.GetParameter(ht);
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter);
            return Convert.ToInt32(val);
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">哈希表键值</param>
        /// <param name="propertyName">主键字段</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Update(string tableName, Hashtable ht, string propertyName, DbTransaction isOpenTrans)
        {
            object val = 0;
            var strSql = DatabaseCommon.UpdateSql(tableName, ht, propertyName);
            var parameter = DatabaseCommon.GetParameter(ht);
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter);
            return Convert.ToInt32(val);
        }

        #endregion

        #region 删除数据

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public int Delete<T>(T entity)
        {
            var strSql = DatabaseCommon.DeleteSql(entity);
            var parameter = DatabaseCommon.GetParameter(entity);
            return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter);
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Delete<T>(T entity, DbTransaction isOpenTrans)
        {
            var strSql = DatabaseCommon.DeleteSql(entity);
            var parameter = DatabaseCommon.GetParameter(entity);
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="propertyValue">主键值</param>
        /// <returns></returns>
        public int Delete<T>(object propertyValue)
        {
            var tableName = DatabaseCommon.GetTableName<T>().ToString(); //获取表名
            var pkName = DatabaseCommon.GetKeyField<T>().ToString(); //获取主键
            var strSql = DatabaseCommon.DeleteSql(tableName, pkName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + pkName, propertyValue));
            return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="propertyValue">主键值</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Delete<T>(object propertyValue, DbTransaction isOpenTrans)
        {
            var tableName = DatabaseCommon.GetTableName<T>().ToString(); //获取表名
            var pkName = DatabaseCommon.GetKeyField<T>().ToString(); //获取主键
            var strSql = DatabaseCommon.DeleteSql(tableName, pkName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + pkName, propertyValue));
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public int Delete<T>(string propertyName, string propertyValue)
        {
            var tableName = DatabaseCommon.GetTableName<T>().ToString(); //获取表名
            var strSql = DatabaseCommon.DeleteSql(tableName, propertyName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue));
            return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Delete<T>(string propertyName, string propertyValue, DbTransaction isOpenTrans)
        {
            var tableName = DatabaseCommon.GetTableName<T>().ToString(); //获取表名
            var strSql = DatabaseCommon.DeleteSql(tableName, propertyName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue));
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public int Delete(string tableName, string propertyName, string propertyValue)
        {
            var strSql = DatabaseCommon.DeleteSql(tableName, propertyName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue));
            return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Delete(string tableName, string propertyName, string propertyValue, DbTransaction isOpenTrans)
        {
            var strSql = DatabaseCommon.DeleteSql(tableName, propertyName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue));
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">键值生成SQL条件</param>
        /// <returns></returns>
        public int Delete(string tableName, Hashtable ht)
        {
            var strSql = DatabaseCommon.DeleteSql(tableName, ht);
            var parameter = DatabaseCommon.GetParameter(ht);
            return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter);
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">键值生成SQL条件</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Delete(string tableName, Hashtable ht, DbTransaction isOpenTrans)
        {
            var strSql = DatabaseCommon.DeleteSql(tableName, ht);
            var parameter = DatabaseCommon.GetParameter(ht);
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter);
        }

        /// <summary>
        ///     批量删除数据
        /// </summary>
        /// <param name="propertyValue">主键值：数组1,2,3,4,5,6.....</param>
        /// <returns></returns>
        public int Delete<T>(object[] propertyValue)
        {
            var tableName = DatabaseCommon.GetTableName<T>().ToString(); //获取表名
            var pkName = DatabaseCommon.GetKeyField<T>().ToString(); //获取主键
            var strSql = new StringBuilder("DELETE FROM " + tableName + " WHERE " + pkName + " IN (");
            try
            {
                IList<DbParameter> parameter = new List<DbParameter>();
                var index = 0;
                var str = DbHelper.DbParmChar + "ID" + index;
                for (var i = 0; i < propertyValue.Length - 1; i++)
                {
                    var obj2 = propertyValue[i];
                    str = DbHelper.DbParmChar + "ID" + index;
                    strSql.Append(str).Append(",");
                    parameter.Add(DbFactory.CreateDbParameter(str, obj2));
                    index++;
                }
                str = DbHelper.DbParmChar + "ID" + index;
                strSql.Append(str);
                parameter.Add(DbFactory.CreateDbParameter(str, propertyValue[index]));
                strSql.Append(")");
                return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray());
                ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     批量删除数据
        /// </summary>
        /// <param name="propertyValue">主键值：数组1,2,3,4,5,6.....</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Delete<T>(object[] propertyValue, DbTransaction isOpenTrans)
        {
            var tableName = DatabaseCommon.GetTableName<T>().ToString(); //获取表名
            var pkName = DatabaseCommon.GetKeyField<T>().ToString(); //获取主键
            var strSql =
                new StringBuilder("DELETE FROM " + tableName + " WHERE " + DbHelper.DbParmChar + pkName + " IN (");
            try
            {
                IList<DbParameter> parameter = new List<DbParameter>();
                var index = 0;
                var str = DbHelper.DbParmChar + "ID" + index;
                for (var i = 0; i < propertyValue.Length - 1; i++)
                {
                    var obj2 = propertyValue[i];
                    str = DbHelper.DbParmChar + "ID" + index;
                    strSql.Append(str).Append(",");
                    parameter.Add(DbFactory.CreateDbParameter(str, obj2));
                    index++;
                }
                str = DbHelper.DbParmChar + "ID" + index;
                strSql.Append(str);
                parameter.Add(DbFactory.CreateDbParameter(str, propertyValue[index]));
                strSql.Append(")");
                return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray());
                ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     批量删除数据
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值：数组1,2,3,4,5,6.....</param>
        /// <returns></returns>
        public int Delete<T>(string propertyName, object[] propertyValue)
        {
            var tableName = DatabaseCommon.GetTableName<T>().ToString(); //获取表名
            var pkName = propertyName;
            var strSql =
                new StringBuilder("DELETE FROM " + tableName + " WHERE " + DbHelper.DbParmChar + pkName + " IN (");
            try
            {
                IList<DbParameter> parameter = new List<DbParameter>();
                var index = 0;
                var str = DbHelper.DbParmChar + "ID" + index;
                for (var i = 0; i < propertyValue.Length - 1; i++)
                {
                    var obj2 = propertyValue[i];
                    str = DbHelper.DbParmChar + "ID" + index;
                    strSql.Append(str).Append(",");
                    parameter.Add(DbFactory.CreateDbParameter(str, obj2));
                    index++;
                }
                str = DbHelper.DbParmChar + "ID" + index;
                strSql.Append(str);
                parameter.Add(DbFactory.CreateDbParameter(str, propertyValue[index]));
                strSql.Append(")");
                return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     批量删除数据
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值：数组1,2,3,4,5,6.....</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Delete<T>(string propertyName, object[] propertyValue, DbTransaction isOpenTrans)
        {
            var tableName = DatabaseCommon.GetTableName<T>().ToString(); //获取表名
            var pkName = propertyName;
            var strSql =
                new StringBuilder("DELETE FROM " + tableName + " WHERE " + DbHelper.DbParmChar + pkName + " IN (");
            try
            {
                IList<DbParameter> parameter = new List<DbParameter>();
                var index = 0;
                var str = DbHelper.DbParmChar + "ID" + index;
                for (var i = 0; i < propertyValue.Length - 1; i++)
                {
                    var obj2 = propertyValue[i];
                    str = DbHelper.DbParmChar + "ID" + index;
                    strSql.Append(str).Append(",");
                    parameter.Add(DbFactory.CreateDbParameter(str, obj2));
                    index++;
                }
                str = DbHelper.DbParmChar + "ID" + index;
                strSql.Append(str);
                parameter.Add(DbFactory.CreateDbParameter(str, propertyValue[index]));
                strSql.Append(")");
                return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray());
                ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     批量删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值：数组1,2,3,4,5,6.....</param>
        /// <returns></returns>
        public int Delete(string tableName, string propertyName, object[] propertyValue)
        {
            var pkName = propertyName;
            var strSql =
                new StringBuilder("DELETE FROM " + tableName + " WHERE " + DbHelper.DbParmChar + pkName + " IN (");
            try
            {
                IList<DbParameter> parameter = new List<DbParameter>();
                var index = 0;
                var str = DbHelper.DbParmChar + "ID" + index;
                for (var i = 0; i < propertyValue.Length - 1; i++)
                {
                    var obj2 = propertyValue[i];
                    str = DbHelper.DbParmChar + "ID" + index;
                    strSql.Append(str).Append(",");
                    parameter.Add(DbFactory.CreateDbParameter(str, obj2));
                    index++;
                }
                str = DbHelper.DbParmChar + "ID" + index;
                strSql.Append(str);
                parameter.Add(DbFactory.CreateDbParameter(str, propertyValue[index]));
                strSql.Append(")");
                return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray());
                ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     批量删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值：数组1,2,3,4,5,6.....</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Delete(string tableName, string propertyName, object[] propertyValue, DbTransaction isOpenTrans)
        {
            var pkName = propertyName;
            var strSql =
                new StringBuilder("DELETE FROM " + tableName + " WHERE " + DbHelper.DbParmChar + pkName + " IN (");
            try
            {
                IList<DbParameter> parameter = new List<DbParameter>();
                var index = 0;
                var str = DbHelper.DbParmChar + "ID" + index;
                for (var i = 0; i < propertyValue.Length - 1; i++)
                {
                    var obj2 = propertyValue[i];
                    str = DbHelper.DbParmChar + "ID" + index;
                    strSql.Append(str).Append(",");
                    parameter.Add(DbFactory.CreateDbParameter(str, obj2));
                    index++;
                }
                str = DbHelper.DbParmChar + "ID" + index;
                strSql.Append(str);
                parameter.Add(DbFactory.CreateDbParameter(str, propertyValue[index]));
                strSql.Append(")");
                return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray());
                ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 查询数据列表、返回List

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="top">显示条数</param>
        /// <returns></returns>
        public List<T> FindListTop<T>(int top) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>(top);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="top">显示条数</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public List<T> FindListTop<T>(int top, string propertyName, string propertyValue) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>(top);
            strSql.Append(" AND " + propertyName + " = " + DbHelper.DbParmChar + propertyName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue));
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="top">显示条数</param>
        /// <param name="whereSql">条件</param>
        /// <returns></returns>
        public List<T> FindListTop<T>(int top, string whereSql) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>(top);
            strSql.Append(whereSql);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="top">显示条数</param>
        /// <param name="whereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public List<T> FindListTop<T>(int top, string whereSql, DbParameter[] parameters) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>(top);
            strSql.Append(whereSql);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <returns></returns>
        public List<T> FindList<T>() where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public List<T> FindList<T>(string propertyName, string propertyValue) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(" AND " + propertyName + " = " + DbHelper.DbParmChar + propertyName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue));
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="whereSql">条件</param>
        /// <returns></returns>
        public List<T> FindList<T>(string whereSql) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(whereSql);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="whereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public List<T> FindList<T>(string whereSql, DbParameter[] parameters) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(whereSql);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public List<T> FindListBySql<T>(string strSql)
        {
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql);
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public List<T> FindListBySql<T>(string strSql, DbParameter[] parameters)
        {
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql, parameters);
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public List<T> FindListPage<T>(string orderField, string orderType, int pageIndex, int pageSize,
            ref int recordCount) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            return SqlServerHelper.GetPageList<T>(strSql.ToString(), orderField, orderType, pageIndex, pageSize,
                ref recordCount);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="whereSql">条件</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public List<T> FindListPage<T>(string whereSql, string orderField, string orderType, int pageIndex, int pageSize,
            ref int recordCount) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(whereSql);
            return SqlServerHelper.GetPageList<T>(strSql.ToString(), orderField, orderType, pageIndex, pageSize,
                ref recordCount);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="whereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public List<T> FindListPage<T>(string whereSql, DbParameter[] parameters, string orderField, string orderType,
            int pageIndex, int pageSize, ref int recordCount) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(whereSql);
            return SqlServerHelper.GetPageList<T>(strSql.ToString(), parameters, orderField, orderType, pageIndex,
                pageSize, ref recordCount);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public List<T> FindListPageBySql<T>(string strSql, string orderField, string orderType, int pageIndex,
            int pageSize, ref int recordCount)
        {
            return SqlServerHelper.GetPageList<T>(strSql, orderField, orderType, pageIndex, pageSize, ref recordCount);
        }

        /// <summary>
        ///     查询数据列表、返回List
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public List<T> FindListPageBySql<T>(string strSql, DbParameter[] parameters, string orderField, string orderType,
            int pageIndex, int pageSize, ref int recordCount)
        {
            return SqlServerHelper.GetPageList<T>(strSql, parameters, orderField, orderType, pageIndex, pageSize,
                ref recordCount);
        }

        #endregion

        #region 查询数据列表、返回DataTable

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="top">显示条数</param>
        /// <returns></returns>
        public DataTable FindTableTop<T>(int top) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>(top);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToDataTable(dr);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="top">显示条数</param>
        /// <param name="whereSql">条件</param>
        /// <returns></returns>
        public DataTable FindTableTop<T>(int top, string whereSql) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>(top);
            strSql.Append(whereSql);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToDataTable(dr);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="top">显示条数</param>
        /// <param name="whereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public DataTable FindTableTop<T>(int top, string whereSql, DbParameter[] parameters) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>(top);
            strSql.Append(whereSql);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToDataTable(dr);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable FindTable<T>() where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToDataTable(dr);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="whereSql">条件</param>
        /// <returns></returns>
        public DataTable FindTable<T>(string whereSql) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(whereSql);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToDataTable(dr);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="whereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public DataTable FindTable<T>(string whereSql, DbParameter[] parameters) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(whereSql);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToDataTable(dr);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public DataTable FindTableBySql(string strSql)
        {
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql);
            return DatabaseReader.ReaderToDataTable(dr);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public DataTable FindTableBySql(string strSql, DbParameter[] parameters)
        {
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql, parameters);
            return DatabaseReader.ReaderToDataTable(dr);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public DataTable FindTablePage<T>(string orderField, string orderType, int pageIndex, int pageSize,
            ref int recordCount) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            return SqlServerHelper.GetPageTable(strSql.ToString(), orderField, orderType, pageIndex, pageSize,
                ref recordCount);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="whereSql">条件</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public DataTable FindTablePage<T>(string whereSql, string orderField, string orderType, int pageIndex,
            int pageSize, ref int recordCount) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(whereSql);
            return SqlServerHelper.GetPageTable(strSql.ToString(), orderField, orderType, pageIndex, pageSize,
                ref recordCount);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="whereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public DataTable FindTablePage<T>(string whereSql, DbParameter[] parameters, string orderField, string orderType,
            int pageIndex, int pageSize, ref int recordCount) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(whereSql);
            return SqlServerHelper.GetPageTable(strSql.ToString(), parameters, orderField, orderType, pageIndex,
                pageSize, ref recordCount);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public DataTable FindTablePageBySql(string strSql, string orderField, string orderType, int pageIndex,
            int pageSize, ref int recordCount)
        {
            return SqlServerHelper.GetPageTable(strSql, orderField, orderType, pageIndex, pageSize, ref recordCount);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public DataTable FindTablePageBySql(string strSql, DbParameter[] parameters, string orderField, string orderType,
            int pageIndex, int pageSize, ref int recordCount)
        {
            return SqlServerHelper.GetPageTable(strSql, parameters, orderField, orderType, pageIndex, pageSize,
                ref recordCount);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="procName">存储过程</param>
        /// <returns></returns>
        public DataTable FindTableByProc(string procName)
        {
            var dr = DbHelper.ExecuteReader(CommandType.StoredProcedure, procName);
            return DatabaseReader.ReaderToDataTable(dr);
        }

        /// <summary>
        ///     查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="procName">存储过程</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public DataTable FindTableByProc(string procName, DbParameter[] parameters)
        {
            var dr = DbHelper.ExecuteReader(CommandType.StoredProcedure, procName, parameters);
            return DatabaseReader.ReaderToDataTable(dr);
        }

        #endregion

        #region 查询数据列表、返回DataSet

        /// <summary>
        ///     查询数据列表、返回DataSet
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public DataSet FindDataSetBySql(string strSql)
        {
            return DbHelper.GetDataSet(CommandType.Text, strSql);
        }

        /// <summary>
        ///     查询数据列表、返回DataSet
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public DataSet FindDataSetBySql(string strSql, DbParameter[] parameters)
        {
            return DbHelper.GetDataSet(CommandType.Text, strSql, parameters);
        }

        /// <summary>
        ///     查询数据列表、返回DataSet
        /// </summary>
        /// <param name="procName">存储过程</param>
        /// <returns></returns>
        public DataSet FindDataSetByProc(string procName)
        {
            return DbHelper.GetDataSet(CommandType.StoredProcedure, procName);
        }

        /// <summary>
        ///     查询数据列表、返回DataSet
        /// </summary>
        /// <param name="procName">存储过程</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public DataSet FindDataSetByProc(string procName, DbParameter[] parameters)
        {
            return DbHelper.GetDataSet(CommandType.StoredProcedure, procName, parameters);
        }

        #endregion

        #region 查询对象、返回实体

        /// <summary>
        ///     查询对象、返回实体
        /// </summary>
        /// <param name="propertyValue">主键值</param>
        /// <returns></returns>
        public T FindEntity<T>(object propertyValue) where T : new()
        {
            var pkName = DatabaseCommon.GetKeyField<T>().ToString(); //获取主键字段
            var strSql = DatabaseCommon.SelectSql<T>(1);
            strSql.Append(" AND ").Append(pkName).Append("=").Append(DbHelper.DbParmChar + pkName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + pkName, propertyValue));
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return DatabaseReader.ReaderToModel<T>(dr);
        }

        /// <summary>
        ///     查询对象、返回实体
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public T FindEntity<T>(string propertyName, object propertyValue) where T : new()
        {
            var pkName = propertyName;
            var strSql = DatabaseCommon.SelectSql<T>(1);
            strSql.Append(" AND ").Append(pkName).Append("=").Append(DbHelper.DbParmChar + pkName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + pkName, propertyValue));
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return DatabaseReader.ReaderToModel<T>(dr);
        }

        /// <summary>
        ///     查询对象、返回实体
        /// </summary>
        /// <param name="whereSql">条件</param>
        /// <returns></returns>
        public T FindEntityByWhere<T>(string whereSql) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>(1);
            strSql.Append(whereSql);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToModel<T>(dr);
        }

        /// <summary>
        ///     查询对象、返回实体
        /// </summary>
        /// <param name="whereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public T FindEntityByWhere<T>(string whereSql, DbParameter[] parameters) where T : new()
        {
            var strSql = DatabaseCommon.SelectSql<T>(1);
            strSql.Append(whereSql);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToModel<T>(dr);
        }

        /// <summary>
        ///     查询对象、返回实体
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public T FindEntityBySql<T>(string strSql)
        {
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql);
            return DatabaseReader.ReaderToModel<T>(dr);
        }

        /// <summary>
        ///     查询对象、返回实体
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public T FindEntityBySql<T>(string strSql, DbParameter[] parameters)
        {
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql, parameters);
            return DatabaseReader.ReaderToModel<T>(dr);
        }

        #endregion

        #region 查询对象、返回哈希表

        /// <summary>
        ///     查询对象、返回哈希表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public Hashtable FindHashtable(string tableName, string propertyName, object propertyValue)
        {
            var strSql = DatabaseCommon.SelectSql(tableName, 1);
            strSql.Append(" AND ").Append(propertyName).Append("=").Append(DbHelper.DbParmChar + propertyName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue));
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return DatabaseReader.ReaderToHashtable(dr);
        }

        /// <summary>
        ///     查询对象、返回哈希表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="whereSql">条件</param>
        /// <returns></returns>
        public Hashtable FindHashtable(string tableName, StringBuilder whereSql)
        {
            var strSql = DatabaseCommon.SelectSql(tableName, 1);
            strSql.Append(whereSql);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToHashtable(dr);
        }

        /// <summary>
        ///     查询对象、返回哈希表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="whereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public Hashtable FindHashtable(string tableName, StringBuilder whereSql, DbParameter[] parameters)
        {
            var strSql = DatabaseCommon.SelectSql(tableName, 1);
            strSql.Append(whereSql);
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToHashtable(dr);
        }

        /// <summary>
        ///     查询对象、返回哈希表
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public Hashtable FindHashtableBySql(string strSql)
        {
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql);
            return DatabaseReader.ReaderToHashtable(dr);
        }

        /// <summary>
        ///     查询对象、返回哈希表
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public Hashtable FindHashtableBySql(string strSql, DbParameter[] parameters)
        {
            var dr = DbHelper.ExecuteReader(CommandType.Text, strSql, parameters);
            return DatabaseReader.ReaderToHashtable(dr);
        }

        #endregion

        #region 查询数据、返回条数

        /// <summary>
        ///     查询数据、返回条数
        /// </summary>
        /// <returns></returns>
        public int FindCount<T>() where T : new()
        {
            var strSql = DatabaseCommon.SelectCountSql<T>();
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql.ToString()));
        }

        /// <summary>
        ///     查询数据、返回条数
        ///     <param name="propertyName">实体属性名称</param>
        ///     <param name="propertyValue">字段值</param>
        /// </summary>
        /// <returns></returns>
        public int FindCount<T>(string propertyName, string propertyValue) where T : new()
        {
            var strSql = DatabaseCommon.SelectCountSql<T>();
            strSql.Append(" AND " + propertyName + " = " + DbHelper.DbParmChar + propertyName);
            IList<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue));
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameter.ToArray()));
        }

        /// <summary>
        ///     查询数据、返回条数
        /// </summary>
        /// <param name="whereSql">条件</param>
        /// <returns></returns>
        public int FindCount<T>(string whereSql) where T : new()
        {
            var strSql = DatabaseCommon.SelectCountSql<T>();
            strSql.Append(whereSql);
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql.ToString()));
        }

        /// <summary>
        ///     查询数据、返回条数
        /// </summary>
        /// <param name="whereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public int FindCount<T>(string whereSql, DbParameter[] parameters) where T : new()
        {
            var strSql = DatabaseCommon.SelectCountSql<T>();
            strSql.Append(whereSql);
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters));
        }

        /// <summary>
        ///     查询数据、返回条数
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public int FindCountBySql(string strSql)
        {
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql));
        }

        /// <summary>
        ///     查询数据、返回条数
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public int FindCountBySql(string strSql, DbParameter[] parameters)
        {
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql, parameters));
        }

        #endregion

        #region 查询数据、返回最大数

        /// <summary>
        ///     查询数据、返回最大数
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <returns></returns>
        public object FindMax<T>(string propertyName) where T : new()
        {
            var strSql = DatabaseCommon.SelectMaxSql<T>(propertyName);
            return DbHelper.ExecuteScalar(CommandType.Text, strSql.ToString());
        }

        /// <summary>
        ///     查询数据、返回最大数
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="whereSql">条件</param>
        /// <returns></returns>
        public object FindMax<T>(string propertyName, string whereSql) where T : new()
        {
            var strSql = DatabaseCommon.SelectMaxSql<T>(propertyName);
            strSql.Append(whereSql);
            return DbHelper.ExecuteScalar(CommandType.Text, strSql.ToString());
        }

        /// <summary>
        ///     查询数据、返回最大数
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="whereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public object FindMax<T>(string propertyName, string whereSql, DbParameter[] parameters) where T : new()
        {
            var strSql = DatabaseCommon.SelectMaxSql<T>(propertyName);
            strSql.Append(whereSql);
            return DbHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        ///     查询数据、返回最大数
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public object FindMaxBySql(string strSql)
        {
            return DbHelper.ExecuteScalar(CommandType.Text, strSql);
        }

        /// <summary>
        ///     查询数据、返回最大数
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public object FindMaxBySql(string strSql, DbParameter[] parameters)
        {
            return DbHelper.ExecuteScalar(CommandType.Text, strSql, parameters);
        }

        #endregion
    }
}