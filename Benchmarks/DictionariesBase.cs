using Apex.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using ImmutableTrie;
using Sasa.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using TunnelVisionLabs.Collections.Trees.Immutable;

namespace Benchmarks
{
    [GenericTypeArguments(typeof(int))]
    [GenericTypeArguments(typeof(string))]
    public abstract class DictionariesBase<T>
    {
        protected ImmutableDictionary<T, int> _immDict;
        protected Trie<T, int> _sasaTrie;
        protected ImmutableTrieDictionary<T, int> _sGuh;
        protected ImmutableTreeDictionary<T, int> _tvl;
        protected HashMap<T, int> _apexHashMap;
        protected List<T> _keys;

        public static T GenerateValue(Random r)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)r.Next();
            if (typeof(T) == typeof(string))
                return (T)(object)r.Next().ToString("D8");

            throw new NotImplementedException($"{typeof(T).Name} is not implemented");
        }

        [Params(5, 100, 10000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Init()
        {
            var r = new Random(4);
            _keys = new List<T>();
            _immDict = ImmutableDictionary<T, int>.Empty;
            _sasaTrie = Trie<T, int>.Empty;
            _sGuh = ImmutableTrieDictionary.Create<T, int>();
            _tvl = ImmutableTreeDictionary<T, int>.Empty;
            _apexHashMap = HashMap<T, int>.Empty;
            for (int i = 0; i < Count; ++i)
            {
                var k = GenerateValue(r);
                _immDict = _immDict.SetItem(k, i);
                _sasaTrie = _sasaTrie.Add(k, i);
                _sGuh = _sGuh.Add(k, i);
                _tvl = _tvl.Add(k, i);
                _apexHashMap = _apexHashMap.SetItem(k, i);
                _keys.Add(k);
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
    }
}
