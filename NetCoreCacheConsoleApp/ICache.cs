using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCacheConsoleApp
{
    public interface ICache
    {
        string Get(string key);

        void Set(string key, string value);

        bool Remove(string key);

        void Clear();

        bool Contains(string key);
    }
}
