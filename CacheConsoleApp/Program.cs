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
            Thread thread1 = new Thread(() =>
            {
                CacheManager.Instance.Set("3", "1", 2);
            });
            thread1.Start();

            Thread thread2 = new Thread(() =>
            {
                CacheManager.Instance.Set("3", "2", 2);
            });
            thread2.Start();

            Thread thread3 = new Thread(() =>
            {
                CacheManager.Instance.Set("3", "2", 2);
            });
            thread3.Start();

            Console.WriteLine(CacheManager.Instance.Get<string>("3"));

            CacheManager.Instance.Set("1", "2", 0.03);
            CacheManager.Instance.Set("2", "2", 1);
            Thread.Sleep(8000);
            Console.WriteLine(CacheManager.Instance.Get<string>("1"));
            Console.WriteLine(CacheManager.Instance.Get<string>("2"));

            CacheManager.Instance.Set("byfile","123",@"d://guihub.txt");
            Console.WriteLine(CacheManager.Instance.Get<string>("byfile"));
            Console.Read();
        }
    }
}
