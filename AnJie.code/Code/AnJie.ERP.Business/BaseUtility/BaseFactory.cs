using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnJie.ERP.Business
{
    /// <summary>
    /// 定义通用的功能工厂
    /// </summary>
    public class BaseFactory
    {
        /// <summary>
        /// 对象用于锁
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// 
        /// </summary>
        private static readonly BaseManager BaseManager = null;

        /// <summary>
        /// 通用操作
        /// </summary>
        /// <returns></returns>
        public static IBaseManager BaseHelper()
        {
            if (BaseManager == null)
            {
                return new BaseManager();
            }
            lock (Locker)
            {
                return BaseManager;
            }
        }
    }
}
