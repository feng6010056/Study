using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreCacheConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CacheManager.Instance.Set("1", "2", 3);
            CacheManager.Instance.Set("2", "2", 10);
            Thread.Sleep(5000);
            Console.WriteLine(CacheManager.Instance.Get<string>("1"));
            Console.WriteLine(CacheManager.Instance.Get<string>("2"));
            Console.Read();
        }

        
    }
}
