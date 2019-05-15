using Apex.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Benchmarks
{
    public class DictionariesBuilder
    {
        [Params(100, 10000, 1000000)]
        public int Count { get; set; }

        private List<KeyValuePair<int,int>> _list;

        [GlobalSetup]
        public void Init()
        {
            var r = new Random();
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
        public void Dictionary()
        {
            var t = new Dictionary<int, int>(_list);
            foreach(var kvp in _list)
            {
                t.Remove(kvp.Key);
            }
        }

        [Benchmark]
        public void ImmutableDictionary()
        {
            var t = ImmutableDictionary<int, int>.Empty;
            t = t.SetItems(_list);
            t = t.RemoveRange(_list.Select(x => x.Key));
        }

        [Benchmark]
        public void ImmutableSortedDictionary()
        {
            var t = ImmutableSortedDictionary<int, int>.Empty;
            t = t.SetItems(_list);
            t = t.RemoveRange(_list.Select(x => x.Key));
        }

        [Benchmark]
        public void ApexHashMap()
        {
            var t = HashMap<int, int>.Empty;
            t = t.SetItems(_list);
            t = t.RemoveRange(_list.Select(x => x.Key));
        }
    }
}
