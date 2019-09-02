using Apex.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using Sasa.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Benchmarks
{
    public class DictionariesModifyAdd
    {
        private List<int> _access;

        [GlobalSetup]
        public void Init()
        {
            var r = new Random(4);
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

        [Params(5, 100, 10000)]
        public int Count { get; set; }

        [Benchmark(Baseline = true)]
        public void ImmutableDictionary()
        {
            var t = ImmutableDictionary<int, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                t = t.SetItem(i, i);
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
        }

        [Benchmark]
        public void ImmutableTrieDictionary()
        {
            var t = ImmutableTrie.ImmutableTrieDictionary.Create<int, int>();
            for (int i = 0; i < Count; ++i)
            {
                t = t.Add(i, i);
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
        }
    }
}
