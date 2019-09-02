using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchmarks
{
    public class DictionariesBuilderRemove : DictionariesBase
    {
        private List<KeyValuePair<int,int>> _list;

        [GlobalSetup(Targets = new[] { nameof(ImmutableDictionary), nameof(ImmutableTrieDictionary), nameof(ApexHashMap) })]
        public void Init2()
        {
            Init();

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
            var t = _immDict;
            t = t.RemoveRange(_list.Select(x => x.Key));
            return t;
        }

        [Benchmark]
        public object ImmutableTrieDictionary()
        {
            var t = _sGuh;
            t = t.RemoveRange(_list.Select(x => x.Key));
            return t;
        }

        [Benchmark]
        public object ApexHashMap()
        {
            var t = _apexHashMap;
            t = t.RemoveRange(_list.Select(x => x.Key));
            return t;
        }
    }
}
