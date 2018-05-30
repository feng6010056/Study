using System;
using System.Collections.Generic;
using System.Text;

namespace CacheConsoleApp
{
    public interface ICache
    {
        string Get(string key);

        void Set(string key, string value, TimeSpan expTime);

        bool Remove(string key);

        void Clear();

        bool Contains(string key);
    }
}
