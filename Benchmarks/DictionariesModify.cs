using Apex.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using Sasa.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Benchmarks
{
    public class DictionariesModify
    {
        private List<int> _access;

        [GlobalSetup]
        public void Init()
        {
            var r = new Random();
            _access = new List<int>();

            for (int i = 0; i < Count; ++i)
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

        [Params(100, 10000, 1000000)]
        public int Count { get; set; }

        [Benchmark(Baseline = true)]
        public void SystemDictionary()
        {
            var t = new Dictionary<int, int>();
            for (int i = 0; i < Count; ++i)
            {
                t.Add(i, i);
            }
            for (int i = 0; i < Count; ++i)
            {
                t.Remove(_access[i]);
            }
        }

        [Benchmark]
        public void SystemImmutableDictionary()
        {
            var t = ImmutableDictionary<int, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                t = t.SetItem(i, i);
            }
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_access[i]);
            }
        }

        [Benchmark]
        public void SystemSortedImmutableDictionary()
        {
            var t = ImmutableSortedDictionary<int, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                t = t.SetItem(i, i);
            }
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_access[i]);
            }
        }

        [Benchmark]
        public void SasaTrie()
        {
            var t = Trie<int, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                t = t.Add(i, i);
            }
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_access[i]);
            }
        }

        [Benchmark]
        public void ApexHashMap()
        {
            var t = HashMap<int, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                t = t.SetItem(i, i);
            }
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_access[i]);
            }
        }
    }
}
