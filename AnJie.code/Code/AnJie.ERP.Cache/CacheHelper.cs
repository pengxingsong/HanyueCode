using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace AnJie.ERP.Cache
{
    public class CacheHelper
    {
        private static readonly System.Web.Caching.Cache Cache = HttpRuntime.Cache;

        /// <summary>
        /// 插入缓存，如果存在则替换
        /// </summary>
        /// <param name="argKey"></param>
        /// <param name="argValue"></param>
        /// <returns></returns>
        public static int Insert(string argKey, object argValue)
        {
            Cache.Insert(argKey, argValue);
            return 1;
        }

        /// <summary>
        /// 插入缓存，如果存在则替换
        /// </summary>
        /// <param name="argKey"></param>
        /// <param name="argValue"></param>
        /// <param name="argDependency"></param>
        /// <returns></returns>
        public static int Insert(string argKey, object argValue, CacheDependency argDependency)
        {
            Cache.Insert(argKey, argValue, argDependency);
            return 1;
        }

        /// <summary>
        /// 插入缓存，如果存在则替换
        /// </summary>
        /// <param name="argKey"></param>
        /// <param name="argValue"></param>
        /// <param name="argDependency"></param>
        /// <param name="argExpiration"></param>
        /// <returns></returns>
        public static int Insert(string argKey, object argValue, CacheDependency argDependency, DateTime argExpiration)
        {
            Cache.Insert(argKey, argValue, argDependency, argExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
            return 1;
        }

        /// <summary>
        /// 添加缓存，如果存在则抛出异常
        /// </summary>
        /// <param name="argKey"></param>
        /// <param name="argValue"></param>
        /// <returns></returns>
        public static int Add(string argKey, object argValue)
        {
            Cache.Add(argKey, argValue, null, DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration,
                CacheItemPriority.Default, null);
            return 1;
        }

        /// <summary>
        /// 添加缓存，如果存在则抛出异常
        /// </summary>
        /// <param name="argKey"></param>
        /// <param name="argValue"></param>
        /// <param name="argDependency"></param>
        /// <returns></returns>
        public static int Add(string argKey, object argValue, CacheDependency argDependency)
        {
            Cache.Add(argKey, argValue, argDependency, DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration,
                CacheItemPriority.Default, null);
            return 1;
        }

        /// <summary>
        /// 添加缓存，如果存在则抛出异常
        /// </summary>
        /// <param name="argKey"></param>
        /// <param name="argValue"></param>
        /// <param name="argDependency"></param>
        /// <param name="argExpiration"></param>
        /// <returns></returns>
        public static int Add(string argKey, object argValue, CacheDependency argDependency, DateTime argExpiration)
        {
            Cache.Add(argKey, argValue, argDependency, argExpiration, System.Web.Caching.Cache.NoSlidingExpiration,
                CacheItemPriority.Default, null);
            return 0;
        }

        /// <summary>
        /// 返回缓存中的所有数据行
        /// </summary>
        /// <returns></returns>
        public static int Count()
        {
            return Cache.Count;
        }

        /// <summary>
        /// 根据键值获得缓存值
        /// </summary>
        /// <param name="argKey"></param>
        /// <returns></returns>
        public static object Get(string argKey)
        {
            return Cache[argKey];
        }

        /// <summary>
        /// 根据键值获得特定类型的缓存值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argKey"></param>
        /// <returns></returns>
        public static T Get<T>(string argKey)
        {
            if (Cache[argKey] != null)
            {
                return (T) Cache[argKey];
            }
            return default(T);
        }

        /// <summary>
        /// 移除特定的键值
        /// </summary>
        /// <param name="argKey"></param>
        /// <returns></returns>
        public static int Remove(string argKey)
        {
            Cache.Remove(argKey);
            return 1;
        }
    }
}