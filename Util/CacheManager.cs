using System;

using System.Collections.Generic;
using System.Text;

namespace com.hujun64.util
{
    public class CacheManager
    {
        private static Dictionary<String, Object> cachePool = new Dictionary<String, Object>();



        /**
         * get specified object in cache pool
         * 
         * @param key
         * @return
         */
        public static Object GetCacheObject(String key)
        {
            if (key == null)
                return null;
            key = key.Trim();


            if (cachePool.ContainsKey(key))
                return cachePool[key];
            else
                return false;
        }


        /**
         * save object into Cache pool
         * 
         * @param key
         * @param cacheItem
         * @param duration
         */
        public static void SaveCacheObject(String key, Object cacheItem)
        {
            if (key == null)
                return;

            key = key.Trim();

            cachePool.Add(key, cacheItem);
        }
        public static void removeCacheObject(String key)
        {
            if (key == null)
                return;

            key = key.Trim();

            cachePool.Remove(key);
        }

    }
}
