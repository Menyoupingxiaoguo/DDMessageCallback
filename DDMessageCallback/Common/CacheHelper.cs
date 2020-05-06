using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace DDMessageCallback.Common
{
    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 内存缓存对象
        /// </summary>
        protected static ObjectCache cache;
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static CacheHelper()
        {
            cache = MemoryCache.Default;
        }

        /// <summary>
        /// 添加缓存如果已存在则不添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="Minutes">缓存时间（分钟）</param>
        /// <returns></returns>
        public static bool AddCache(string key, object value, int Minutes)
        {
            bool resultBl = false;
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Minutes);
            resultBl = cache.Add(key, value, policy);
            return resultBl;
        }
        /// <summary>
        /// 设置缓存已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="Minutes">缓存时间（分钟）</param>
        public static void SetCache(string key, object value, int Minutes)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Minutes);
            cache.Set(key, value, policy);
        }
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            object resultObj = null;
            if (cache.Count() > 0)
            {
                resultObj = cache.Get(key);
            }
            return resultObj;
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object DeleteCache(string key)
        {
            return cache.Remove(key);
        }
        /// <summary>
        /// 查看缓存是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsExistCache(string key)
        {
            return cache.Contains(key);
        }
        /// <summary>
        /// 获取当前缓存对象数量
        /// </summary>
        /// <returns></returns>
        public static int GetCacheCount()
        {
            return cache.Count();
        }
    }
}