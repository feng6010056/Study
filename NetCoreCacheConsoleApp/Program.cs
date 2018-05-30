using System;

namespace NetCoreCacheConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CacheManager.Instance.Set("aaa", "bbb");
            CacheManager.Instance.Get("aaa");
            CacheManager.Instance.Contains("aaa");
            CacheManager.Instance.Remove("aaa");
            CacheManager.Instance.Clear();
            Console.Read();
        }
    }
}
