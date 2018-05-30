using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CacheConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CacheManager.Instance.Set("1", "2", 0.03);
            CacheManager.Instance.Set("2", "2", 1);
            Thread.Sleep(8000);
            Console.WriteLine(CacheManager.Instance.Get<string>("1"));
            Console.WriteLine(CacheManager.Instance.Get<string>("2"));
            Console.Read();
        }
    }
}
