using Apex.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using Sasa.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using TunnelVisionLabs.Collections.Trees.Immutable;

namespace Benchmarks
{
    [GenericTypeArguments(typeof(int))]
    [GenericTypeArguments(typeof(string))]
    public class DictionariesModifyAdd<T>
    {
        private List<T> _keys;

        [GlobalSetup]
        public void Init()
        {
            var r = new Random(4);
            _keys = new List<T>();

            for (int i = 0; i < Count; ++i)
            {
                _keys.Add(DictionariesBase<T>.GenerateValue(r));
            }

            int n = _keys.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                var value = _keys[k];
                _keys[k] = _keys[n];
                _keys[n] = value;
            }
        }

        [Params(5, 100, 10000)]
        public int Count { get; set; }

        [Benchmark(Baseline = true)]
        public void ImmutableDictionary()
        {
            var t = ImmutableDictionary<T, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                t = t.SetItem(_keys[i], i);
            }
        }

        [Benchmark]
        public void SasaTrie()
        {
            var t = Trie<T, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                t = t.Add(_keys[i], i);
            }
        }

        [Benchmark]
        public void ImmutableTrieDictionary()
        {
            var t = ImmutableTrie.ImmutableTrieDictionary.Create<T, int>();
            for (int i = 0; i < Count; ++i)
            {
                t = t.Add(_keys[i], i);
            }
        }

        [Benchmark]
        public void ImmutableTreeDictionary()
        {
            var t = ImmutableTreeDictionary<T, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                t = t.Add(_keys[i], i);
            }
        }

        //[Benchmark]
        public void ImToolsHashMap()
        {
            var t = ImTools.ImHashMap<T, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                //t = t.AddOrUpdate(_keys[i], i);
            }
        }

        [Benchmark]
        public void ApexHashMap()
        {
            var t = HashMap<T, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                t = t.SetItem(_keys[i], i);
            }
        }
    }
}
