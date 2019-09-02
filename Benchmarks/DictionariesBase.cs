using Apex.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using ImmutableTrie;
using Sasa.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Benchmarks
{
    public class DictionariesBase
    {
        protected ImmutableDictionary<int, int> _immDict;
        protected Trie<int, int> _sasaTrie;
        protected ImmutableTrieDictionary<int, int> _sGuh;
        protected HashMap<int, int> _apexHashMap;
        protected List<int> _access;

        [Params(5, 100, 10000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Init()
        {
            var r = new Random(4);
            _access = new List<int>();
            _immDict = ImmutableDictionary<int, int>.Empty;
            _sasaTrie = Trie<int, int>.Empty;
            _sGuh = ImmutableTrieDictionary.Create<int, int>();
            _apexHashMap = HashMap<int, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                _immDict = _immDict.SetItem(i, i);
                _sasaTrie = _sasaTrie.Add(i, i);
                _sGuh = _sGuh.Add(i, i);
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
    }
}
