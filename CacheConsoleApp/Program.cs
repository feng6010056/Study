using System;
using System.Collections.Generic;
using System.Text;

namespace CacheConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CacheManager.Instance.Set("aaa", "bbb", new TimeSpan(1000));
            CacheManager.Instance.Get("aaa");
            CacheManager.Instance.Contains("aaa");
            CacheManager.Instance.Remove("aaa");
            CacheManager.Instance.Clear();
            Console.Read();
        }
    }
}
