using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NetCoreCacheConsoleApp
{
    public class CacheManager : ICache
    {
        private static Dictionary<string, Tuple<Object, DateTime>> _cacheDic = new Dictionary<string, Tuple<Object, DateTime>>();

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
                    if (item.Value.Item2 < DateTime.Now)
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

        public void Clear()
        {
            lock (cacheObj)
            {
                _cacheDic.Clear();
            }
        }

        public bool Contains(string key)
        {
            return _cacheDic.ContainsKey(key)
                && _cacheDic[key].Item2 > DateTime.Now;
        }

        public T Get<T>(string key)
            where T : class
        {
            if (this.Contains(key))
            {
                return _cacheDic[key].Item1 is T ? (_cacheDic[key].Item1 as T) : null;
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

        public void Set(string key, object value, double time)
        {
            lock (cacheObj)
            {
                if (!this.Contains(key))
                {
                    Tuple<object, DateTime> item = new Tuple<object, DateTime>(value, DateTime.Now.AddMinutes(time));
                    _cacheDic.Add(key, item);
                }
                else
                {
                    _cacheDic[key] = new Tuple<object, DateTime>(value, DateTime.Now.AddMinutes(time));
                }
            }
        }

        public string Get(string key)
        {
            if (this.Contains(key))
            {
                return _cacheDic[key].Item1.ToString();
            }
            return null;
        }
    }
}
