using AnJie.ERP.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnJie.ERP.Repository
{
    /// <summary>
    /// 操作数据库工厂
    /// </summary>
    public class DataFactory
    {
        private static readonly object Locker = new object();
        private static Database _db;

        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static IDatabase Database(string connString)
        {
            if (_db == null)
            {
                lock (Locker)
                {
                    _db = new Database(connString);
                }
            }
            return _db;
        }

        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDatabase Database()
        {
            return Database("ERP_SqlServer");
        }
    }
}
