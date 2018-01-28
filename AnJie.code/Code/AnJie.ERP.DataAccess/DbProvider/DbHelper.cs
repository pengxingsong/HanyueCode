using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using AnJie.ERP.Utilities;

namespace AnJie.ERP.DataAccess
{
    /// <summary>
    ///     数据库操作基类
    /// </summary>
    public class DbHelper
    {
        public DbHelper(string connstring)
        {
            var conStringDesEncrypt = ConfigurationManager.AppSettings["ConStringDESEncrypt"];
            ConnectionString = ConfigurationManager.ConnectionStrings[connstring].ConnectionString;
            if (conStringDesEncrypt == "true")
            {
                ConnectionString = DESEncrypt.Decrypt(ConnectionString);
            }

            DbParmChar = DbFactory.CreateDbParmCharacter();
        }

        /// <summary>
        ///     调试日志
        /// </summary>
        private static LogHelper Log
        {
            get { return LogFactory.GetLogger(typeof(DbHelper)); }
        }

        /// <summary>
        ///     连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        ///     数据库命名参数符号
        /// </summary>
        public static string DbParmChar { get; set; }

        /// <summary>
        ///     执行 SQL 语句，并返回受影响的行数。
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            var num = 0;
            try
            {
                var cmd = DbFactory.CreateDbCommand();
                using (var conn = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                    num = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                num = -1;
                Log.Error(ex.Message);
                throw;
            }
            return num;
        }

        /// <summary>
        ///     执行 SQL 语句，并返回受影响的行数。
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText)
        {
            var num = 0;
            try
            {
                var cmd = DbFactory.CreateDbCommand();
                using (var conn = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                    num = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                num = -1;
                Log.Error(ex.Message);
            }
            return num;
        }

        /// <summary>
        ///     执行 SQL 语句，并返回受影响的行数。
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbConnection connection, CommandType cmdType, string cmdText,
            params DbParameter[] parameters)
        {
            var num = 0;
            try
            {
                var cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, connection, null, cmdType, cmdText, parameters);
                num = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                num = -1;
                Log.Error(ex.Message);
            }
            return num;
        }

        /// <summary>
        ///     执行 SQL 语句，并返回受影响的行数。
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbConnection connection, CommandType cmdType, string cmdText)
        {
            var num = 0;
            try
            {
                var cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
                num = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                num = -1;
                Log.Error(ex.Message);
            }
            return num;
        }

        /// <summary>
        ///     执行 SQL 语句，并返回受影响的行数。
        /// </summary>
        /// <param name="isOpenTrans">事务对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbTransaction isOpenTrans, CommandType cmdType, string cmdText,
            params DbParameter[] parameters)
        {
            var num = 0;
            try
            {
                var cmd = DbFactory.CreateDbCommand();
                if (isOpenTrans == null || isOpenTrans.Connection == null)
                {
                    using (var conn = DbFactory.CreateDbConnection(ConnectionString))
                    {
                        PrepareCommand(cmd, conn, isOpenTrans, cmdType, cmdText, parameters);
                        num = cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    PrepareCommand(cmd, isOpenTrans.Connection, isOpenTrans, cmdType, cmdText, parameters);
                    num = cmd.ExecuteNonQuery();
                }
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                num = -1;
                Log.Error(ex.Message);
                throw;
            }
            return num;
        }

        /// <summary>
        ///     执行 SQL 语句，并返回受影响的行数。
        /// </summary>
        /// <param name="isOpenTrans">事务对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbTransaction isOpenTrans, CommandType cmdType, string cmdText)
        {
            var num = 0;
            try
            {
                var cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, isOpenTrans.Connection, isOpenTrans, cmdType, cmdText, null);
                num = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                num = -1;
                Log.Error(ex.Message);
                throw;
            }
            return num;
        }

        /// <summary>
        ///     使用提供的参数，执行有结果集返回的数据库操作命令、并返回SqlDataReader对象
        /// </summary>
        /// <param name="isOpenTrans">事务对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static IDataReader ExecuteReader(DbTransaction isOpenTrans, CommandType cmdType, string cmdText,
            params DbParameter[] parameters)
        {
            var cmd = DbFactory.CreateDbCommand();
            var conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, isOpenTrans, cmdType, cmdText, parameters);
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     使用提供的参数，执行有结果集返回的数据库操作命令、并返回SqlDataReader对象
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static IDataReader ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            var cmd = DbFactory.CreateDbCommand();
            var conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     使用提供的参数，执行有结果集返回的数据库操作命令、并返回SqlDataReader对象
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static IDataReader ExecuteReader(CommandType cmdType, string cmdText)
        {
            var cmd = DbFactory.CreateDbCommand();
            var conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     查询数据填充到数据集DataSet中
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns>数据集DataSet对象</returns>
        public static DataSet GetDataSet(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            var ds = new DataSet();
            var cmd = DbFactory.CreateDbCommand();
            var conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                var sda = DbFactory.CreateDataAdapter(cmd);
                sda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     查询数据填充到数据集DataSet中
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">命令文本</param>
        /// <returns>数据集DataSet对象</returns>
        public static DataSet GetDataSet(CommandType cmdType, string cmdText)
        {
            var ds = new DataSet();
            var cmd = DbFactory.CreateDbCommand();
            var conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                var sda = DbFactory.CreateDataAdapter(cmd);
                sda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     依靠数据库连接字符串connectionString,
        ///     使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            try
            {
                var cmd = DbFactory.CreateDbCommand();
                using (var connection = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, parameters);
                    var val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     依靠数据库连接字符串connectionString,
        ///     使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText)
        {
            try
            {
                var cmd = DbFactory.CreateDbCommand();
                using (var connection = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
                    var val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     依靠数据库连接字符串connectionString,
        ///     使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(DbConnection connection, CommandType cmdType, string cmdText,
            params DbParameter[] parameters)
        {
            try
            {
                var cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, connection, null, cmdType, cmdText, parameters);
                var val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     依靠数据库连接字符串connectionString,
        ///     使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(DbConnection connection, CommandType cmdType, string cmdText)
        {
            try
            {
                var cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
                var val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     依靠数据库连接字符串connectionString,
        ///     使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="isOpenTrans"></param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(DbConnection conn, DbTransaction isOpenTrans, CommandType cmdType,
            string cmdText)
        {
            try
            {
                var cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, conn, isOpenTrans, cmdType, cmdText, null);
                var val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     依靠数据库连接字符串connectionString,
        ///     使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <param name="isOpenTrans">事务</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(DbTransaction isOpenTrans, CommandType cmdType, string cmdText,
            params DbParameter[] parameters)
        {
            try
            {
                var cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, isOpenTrans.Connection, isOpenTrans, cmdType, cmdText, parameters);
                var val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     为即将执行准备一个命令
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <param name="conn">SqlConnection对象</param>
        /// <param name="isOpenTrans">DbTransaction对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction isOpenTrans,
            CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (isOpenTrans != null)
                cmd.Transaction = isOpenTrans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }
    }
}