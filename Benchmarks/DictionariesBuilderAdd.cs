using Apex.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Benchmarks
{
    public class DictionariesBuilderAdd
    {
        [Params(5, 100, 10000)]
        public int Count { get; set; }

        private List<KeyValuePair<int,int>> _list;

        [GlobalSetup]
        public void Init2()
        {
            var r = new Random(4);
            _list = new List<KeyValuePair<int, int>>();

            for (int i = 0; i < Count; ++i)
            {
                _list.Add(new KeyValuePair<int, int>(i, i));
            }

            int n = _list.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                var value = _list[k];
                _list[k] = _list[n];
                _list[n] = value;
            }
        }

        [Benchmark(Baseline = true)]
        public object ImmutableDictionary()
        {
            var t = ImmutableDictionary<int, int>.Empty;
            t = t.SetItems(_list);
            return t;
        }

        [Benchmark]
        public object ImmutableTrieDictionary()
        {
            var t = ImmutableTrie.ImmutableTrieDictionary.CreateRange(_list);
            return t;
        }

        [Benchmark]
        public object ApexHashMap()
        {
            var t = HashMap<int, int>.Empty;
            t = t.SetItems(_list);
            return t;
        }
    }
}
