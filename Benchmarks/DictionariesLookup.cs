using Apex.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using Sasa.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using TunnelVisionLabs.Collections.Trees.Immutable;

namespace Benchmarks
{
    public class DictionariesLookup
    {
        private Dictionary<int, int> _dict;
        private ImmutableDictionary<int, int> _immDict;
        private ImmutableSortedDictionary<int, int> _immSortedDict;
        private Trie<int, int> _sasaTrie;
        private ImmutableTrie.ImmutableTrieDictionary<int, int> _sGuh;
        private ImmutableTreeDictionary<int, int> _tvl;
        private HashMap<int, int> _apexHashMap;
        private List<int> _access;

        [Params(5, 100, 10000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Init()
        {
            var r = new Random(4);
            _access = new List<int>();
            _dict = new Dictionary<int, int>();
            _immDict = ImmutableDictionary<int, int>.Empty;
            _immSortedDict = ImmutableSortedDictionary<int, int>.Empty;
            _sasaTrie = Trie<int, int>.Empty;
            _sGuh = ImmutableTrie.ImmutableTrieDictionary.Create<int, int>();
            _tvl = ImmutableTreeDictionary<int, int>.Empty;
            _apexHashMap = HashMap<int, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                _dict.Add(i, i);
                _immDict = _immDict.SetItem(i, i);
                _immSortedDict = _immSortedDict.SetItem(i, i);
                _sasaTrie = _sasaTrie.Add(i, i);
                _sGuh = _sGuh.Add(i, i);
                _tvl = _tvl.Add(i, i);
                _apexHashMap = _apexHashMap.SetItem(i, i);
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

        //[Benchmark(Baseline = true)]
        public void SystemDictionary()
        {
            for(int t=0;t<Count;++t)
            {
                _dict.TryGetValue(_access[t], out _);
            }
        }

        [Benchmark(Baseline = true)]
        public void ImmutableDictionary()
        {
            for (int t = 0; t < Count; ++t)
            {
                _immDict.TryGetValue(_access[t], out _);
            }
        }

        //[Benchmark]
        public void SystemSortedImmutableDictionary()
        {
            for (int t = 0; t < Count; ++t)
            {
                _immSortedDict.TryGetValue(_access[t], out _);
            }
        }

        [Benchmark]
        public void SasaTrie()
        {
            for (int t = 0; t < Count; ++t)
            {
                _sasaTrie.TryGetValue(_access[t], out _);
            }
        }

        [Benchmark]
        public void ImmutableTrieDictionary()
        {
            for (int t = 0; t < Count; ++t)
            {
                _sGuh.TryGetValue(_access[t], out _);
            }
        }

        // Was many times slower than standard immutable dictionary
        //[Benchmark]
        public void TunnelVisionLabs()
        {
            for (int t = 0; t < Count; ++t)
            {
                _tvl.TryGetValue(_access[t], out _);
            }
        }

        [Benchmark]
        public void ApexHashMap()
        {
            for (int t = 0; t < Count; ++t)
            {
                _apexHashMap.TryGetValue(_access[t], out _);
            }
        }
    }
}
