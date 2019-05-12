using Apex.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using Sasa.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Benchmarks
{
    public class DictionariesEnumeration
    {
        private Dictionary<int, int> _dict;
        private ImmutableDictionary<int, int> _immDict;
        private ImmutableSortedDictionary<int, int> _immSortedDict;
        private Trie<int, int> _sasaTrie;
        private HashMap<int, int> _apexHashMap;

        [Params(100, 10000, 1000000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Init()
        {
            _dict = new Dictionary<int, int>();
            _immDict = ImmutableDictionary<int, int>.Empty;
            _immSortedDict = ImmutableSortedDictionary<int, int>.Empty;
            _sasaTrie = Trie<int, int>.Empty;
            _apexHashMap = HashMap<int, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                _dict.Add(i, i);
                _immDict = _immDict.SetItem(i, i);
                _immSortedDict = _immSortedDict.SetItem(i, i);
                _sasaTrie = _sasaTrie.Add(i, i);
                _apexHashMap = _apexHashMap.SetItem(i, i);
            }
        }

        [Benchmark(Baseline = true)]
        public void SystemDictionary()
        {
            foreach (var kvp in _dict)
                ;
        }

        [Benchmark]
        public void SystemImmutableDictionary()
        {
            foreach (var kvp in _immDict)
                ;
        }

        [Benchmark]
        public void SystemSortedImmutableDictionary()
        {
            foreach (var kvp in _immSortedDict)
                ;
        }

        [Benchmark]
        public void SasaTrie()
        {
            foreach (var kvp in _sasaTrie)
                ;
        }

        [Benchmark]
        public void ApexHashMap()
        {
            foreach (var kvp in _apexHashMap)
                ;
        }
    }
}
