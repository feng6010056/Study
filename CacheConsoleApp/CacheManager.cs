using System;
using System.Collections.Generic;
using System.Text;


namespace CacheConsoleApp
{
    public class CacheManager : ICache
    {
        private static object lockObj = new object();

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

        public bool Contains(string key)
        {

            return System.Web.HttpRuntime.Cache.Get(key) != null;
        }

        public void Clear()
        {
            for (int i = 0; i < System.Web.HttpRuntime.Cache.Count; i++)
            {
                string key = System.Web.HttpRuntime.Cache.GetEnumerator().Entry.Key.ToString();
                this.Remove(key);
            }

           
        }

        public string Get(string key)
        {
            object value = System.Web.HttpRuntime.Cache.Get(key);
            if (value != null)
            {
                return value.ToString();
            }
            return null;
        }

        public bool Remove(string key)
        {
            return System.Web.HttpRuntime.Cache.Remove(key) != null;
        }

        public void Set(string key, string value, TimeSpan cacheTime)
        {
            System.Web.HttpRuntime.Cache.Insert(key, value);
        }
    }
}
