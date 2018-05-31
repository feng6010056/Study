using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace CacheConsoleApp
{
    public class CacheManager : ICache
    {

        private static Dictionary<string, CacheModel> _cacheDic = new Dictionary<string, CacheModel>();

        private static List<string> notExpire = new List<string>();


        private static List<string> _removeKey = new List<string>();

        private static object lockObj = new object();

        private static Object cacheObj = new object();

        static Thread thread = new Thread(() =>
        {
            while (true)
            {
                lock (cacheObj)
                {
                    foreach (var item in _cacheDic)
                    {
                        //正常时间过期
                        if (!notExpire.Contains(item.Key) && item.Value.expTime < DateTime.Now)
                        {
                            _removeKey.Add(item.Key);
                        }
                        //文件缓存依赖
                        if (!string.IsNullOrWhiteSpace(item.Value.cacheFile))
                        {
                            string fileName = item.Value.cacheFile;
                            if (!File.Exists(fileName))
                            {
                                _removeKey.Add(item.Key);
                                return;
                            }
                            FileInfo info = new FileInfo(fileName);
                            if (item.Value.lastWriteTime < info.LastWriteTime)
                            {
                                _removeKey.Add(item.Key);
                            }
                        }
                    }
                    for (int i = 0; i < _removeKey.Count; i++)
                    {
                        _cacheDic.Remove(_removeKey[i]);
                    }
                    Thread.Sleep(10000);
                }
            }

        });
        static CacheManager()
        {
            try
            {
                thread.Start();
            }
            catch (Exception ex)
            {
                throw;
            }

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
            if (_cacheDic.ContainsKey(key) && Convert.ToDateTime(_cacheDic[key].expTime) > DateTime.Now)
            {
                return true;
            }
            if (_cacheDic.ContainsKey(key) && _cacheDic[key].cacheFile != null && _cacheDic[key].lastWriteTime == (new FileInfo(_cacheDic[key].cacheFile)).LastWriteTime)
            {
                return true;
            }
            return false;
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
                return _cacheDic[key].Value.ToString();
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
                return _cacheDic[key].Value is T ? (_cacheDic[key].Value as T) : null;
            }
            return null;
        }

        public void Set(string key, object value, double time)
        {
            lock (cacheObj)
            {
                if (!this.Contains(key))
                {
                    CacheModel cacheModel = new CacheModel();
                    cacheModel.Value = value;
                    cacheModel.expTime = DateTime.Now.AddMinutes(time);
                    _cacheDic.Add(key, cacheModel);
                }
                else
                {
                    CacheModel cacheModel = new CacheModel();
                    cacheModel.Value = value;
                    cacheModel.expTime = DateTime.Now.AddMinutes(time);
                    _cacheDic[key] = cacheModel;
                }
                if (time == -1)
                {
                    notExpire.Add(key);
                }
            }
        }

        public void Set(string key, object value, string fileName)
        {
            if (File.Exists(fileName))
            {
                FileInfo fileInfo = new FileInfo(fileName);
                CacheModel cache = new CacheModel();
                cache.cacheFile = fileName;
                cache.lastWriteTime = fileInfo.LastWriteTime;
                cache.Value = value;
                if (!this.Contains(key))
                {
                    _cacheDic.Add(key, cache);
                }
                else
                {
                    _cacheDic[key] = cache;
                }
            }
        }
    }
}
