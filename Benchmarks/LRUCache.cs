using Apex.Collections;
using BenchmarkDotNet.Attributes;
using System;
using System.Threading.Tasks;

namespace Benchmarks
{
    public class LRUCache
    {
        private readonly Func<int, int> _lru1 = k => k;

        [Params(0, 5, 50, 95, 100)]
        public int HitRate { get; set; }

        [Params(100, 1000)]
        public int Capacity { get; set; }

        private int Iterations = 100000;

        [Benchmark]
        public object LRUCacheST()
        {
            var lru = new LeastRecentlyUsedCache<int, int>(Capacity);

            if (HitRate == 0)
            {
                for (int i = 0; i < Iterations; ++i)
                {
                    lru.GetOrAdd(i, _lru1);
                }
                return lru;
            }

            if(HitRate == 100)
            {
                for (int i = 0; i < Iterations; ++i)
                {
                    lru.GetOrAdd(0, _lru1);
                }
                return lru;
            }

            if(HitRate == 50)
            {
                for (int i = 0; i < Iterations; ++i)
                {
                    if ((i % 2) == 0)
                    {
                        lru.GetOrAdd(0, _lru1);
                    }
                    else
                    {
                        lru.GetOrAdd(i, _lru1);
                    }
                }
                return lru;
            }

            if (HitRate == 5)
            {
                for (int i = 0; i < Iterations; ++i)
                {
                    if ((i % 20) == 0)
                    {
                        lru.GetOrAdd(0, _lru1);
                    }
                    else
                    {
                        lru.GetOrAdd(i, _lru1);
                    }
                }
                return lru;
            }

            if (HitRate == 95)
            {
                for (int i = 0; i < Iterations; ++i)
                {
                    if ((i % 20) != 0)
                    {
                        lru.GetOrAdd(0, _lru1);
                    }
                    else
                    {
                        lru.GetOrAdd(i, _lru1);
                    }
                }
                return lru;
            }

            throw new InvalidOperationException("Unsupported HitRate");
        }

        //[Benchmark]
        public void LRUCache50HitHighContention()
        {
            var lru = new LeastRecentlyUsedCache<int, int>(Capacity);

            var t = Task.Run(() =>
            {
                for (int i = 0; i < 5000; ++i)
                {
                    lock (lru)
                    {
                        if ((i % 2) == 0)
                        {
                            lru.GetOrAdd(0, _lru1);
                        }
                        else
                        {
                            lru.GetOrAdd(i, _lru1);
                        }
                    }
                }
            });
            for (int i = 0; i < 5000; ++i)
            {
                lock (lru)
                {
                    if ((i % 2) == 0)
                    {
                        lru.GetOrAdd(0, _lru1);
                    }
                    else
                    {
                        lru.GetOrAdd(i, _lru1);
                    }
                }
            }
            t.Wait();
        }
    }
}
