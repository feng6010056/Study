using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace CacheConsoleApp
{
    public class CacheManager : ICache
    {

        private static Dictionary<string, ArrayList> _cacheDic = new Dictionary<string, ArrayList>();

        private static object lockObj = new object();

        private static Object cacheObj = new object();

        static System.Timers.Timer timer = new System.Timers.Timer();

        static CacheManager()
        {
            timer.Interval = 5000;
            timer.Elapsed += delegate
            {
                foreach (var item in _cacheDic)
                {
                    if (Convert.ToDateTime(item.Value[1]) < DateTime.Now)
                    {
                        _cacheDic.Remove(item.Key);
                    }
                }
            };
        }

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
            return _cacheDic.ContainsKey(key)
                && Convert.ToDateTime(_cacheDic[key][1]) > DateTime.Now;
        }

        public void Clear()
        {
            lock (cacheObj)
            {
                _cacheDic.Clear();
            }
        }

        public string Get(string key)
        {
            if (this.Contains(key))
            {
                return _cacheDic[key][0].ToString();
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

        public T Get<T>(string key) where T : class
        {
            if (this.Contains(key))
            {
                return _cacheDic[key][0] is T ? (_cacheDic[key][0] as T) : null;
            }
            return null;
        }

        public void Set(string key, object value, double time)
        {
            lock (cacheObj)
            {
                if (!this.Contains(key))
                {
                    ArrayList array = new ArrayList();
                    array.Add(value);
                    array.Add(DateTime.Now.AddMinutes(time));
                    _cacheDic.Add(key, array);
                }
                else
                {
                    _cacheDic[key] = new ArrayList { value, DateTime.Now.AddMinutes(time) };
                }
            }
        }


    }
}
