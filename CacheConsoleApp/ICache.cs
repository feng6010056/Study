using System;
using System.Collections.Generic;
using System.Text;

namespace CacheConsoleApp
{
    public interface ICache
    {
        T Get<T>(string key) where T : class;

        void Set(string key, object value, double time);

        bool Remove(string key);

        void Clear();

        bool Contains(string key);

        string Get(string key);
    }
}
