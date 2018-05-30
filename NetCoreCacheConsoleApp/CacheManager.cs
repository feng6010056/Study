using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NetCoreCacheConsoleApp
{
    public class CacheManager : ICache
    {
        private static Dictionary<string, object> _cacheDic = new Dictionary<string, object>();

        private static object lockObj = new object();

        private static Object cacheObj = new object();

        private CacheManager()
        {

        }

        private static CacheManager _instance;

        public static CacheManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            return _instance = new CacheManager();
                        }
                    }
                }
                return _instance;
            }

        }

        public void Clear()
        {
            lock (cacheObj)
            {
                _cacheDic.Clear();
            }
        }

        public bool Contains(string key)
        {
            return _cacheDic.ContainsKey(key);
        }

        public string Get(string key)
        {
            if (this.Contains(key))
            {
                return _cacheDic[key].ToString();
            }
            return null;
        }

        public bool Remove(string key)
        {
            lock (cacheObj)
            {
                return _cacheDic.Remove(key);
            }
        }

        public void Set(string key, string value)
        {
            lock (cacheObj)
            {
                if (!this.Contains(key))
                {
                    _cacheDic.Add(key, value);
                }
            }
        }
    }
}
