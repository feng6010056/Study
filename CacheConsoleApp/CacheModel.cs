using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CacheConsoleApp
{
    public class CacheModel
    {
        public Object Value { get; set; }

        public DateTime expTime { get; set; }

        public string cacheFile { get; set; }

        public DateTime lastWriteTime { get; set; }

    }
}
