using Apex.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Benchmarks
{
    public class DictionariesConstruction
    {
        private List<int> _access;

        [GlobalSetup]
        public void Init()
        {
            var r = new Random();
            _access = new List<int>();

            for (int i = 0; i < Capacity; ++i)
            {
                _access.Add(i);
            }

            int n = _access.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                var value = _access[k];
                _access[k] = _access[n];
                _access[n] = value;
            }
        }
        [Params(100, 10000)]
        public int Capacity { get; set; }

        [Benchmark]
        public void SystemDictionaryAddOnly()
        {
            var t = new Dictionary<int, int>();
            for(int i=0;i<Capacity;++i)
            {
                t.Add(i, i);
            }
        }

        [Benchmark]
        public void SystemImmutableDictionaryAddOnly()
        {
            var t = ImmutableDictionary<int, int>.Empty;
            for (int i = 0; i < Capacity; ++i)
            {
                t = t.SetItem(i, i);
            }
        }

        [Benchmark]
        public void SystemSortedImmutableDictionaryAddOnly()
        {
            var t = ImmutableSortedDictionary<int, int>.Empty;
            for (int i = 0; i < Capacity; ++i)
            {
                t = t.SetItem(i, i);
            }
        }

        [Benchmark]
        public void ApexHashMapAddOnly()
        {
            var t = HashMap<int, int>.Empty;
            for (int i = 0; i < Capacity; ++i)
            {
                t = t.SetItem(i, i);
            }
        }

        [Benchmark]
        public void SystemDictionaryAddRemove()
        {
            var t = new Dictionary<int, int>();
            for (int i = 0; i < Capacity; ++i)
            {
                t.Add(i, i);
            }
            for (int i = 0; i < Capacity; ++i)
            {
                t.Remove(_access[i]);
            }
        }

        [Benchmark]
        public void SystemImmutableDictionaryAddRemove()
        {
            var t = ImmutableDictionary<int, int>.Empty;
            for (int i = 0; i < Capacity; ++i)
            {
                t = t.SetItem(i, i);
            }
            for (int i = 0; i < Capacity; ++i)
            {
                t = t.Remove(_access[i]);
            }
        }

        [Benchmark]
        public void SystemSortedImmutableDictionaryAddRemove()
        {
            var t = ImmutableSortedDictionary<int, int>.Empty;
            for (int i = 0; i < Capacity; ++i)
            {
                t = t.SetItem(i, i);
            }
            for (int i = 0; i < Capacity; ++i)
            {
                t = t.Remove(_access[i]);
            }
        }

        [Benchmark]
        public void ApexHashMapAddRemove()
        {
            var t = HashMap<int, int>.Empty;
            for (int i = 0; i < Capacity; ++i)
            {
                t = t.SetItem(i, i);
            }
            for (int i = 0; i < Capacity; ++i)
            {
                t = t.Remove(_access[i]);
            }
        }
    }
}
