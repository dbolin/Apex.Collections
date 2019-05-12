using Apex.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Benchmarks
{
    public class DictionariesLookup
    {
        private Dictionary<int, int> _dict;
        private ImmutableDictionary<int, int> _immDict;
        private ImmutableSortedDictionary<int, int> _immSortedDict;
        private HashMap<int, int> _apexHashMap;
        private List<int> _access;

        [Params(100, 10000, 1000000)]
        public int Capacity { get; set; }

        [GlobalSetup]
        public void Init()
        {
            var r = new Random();
            _access = new List<int>();
            _dict = new Dictionary<int, int>();
            _immDict = ImmutableDictionary<int, int>.Empty;
            _immSortedDict = ImmutableSortedDictionary<int, int>.Empty;
            _apexHashMap = HashMap<int, int>.Empty;
            for (int i = 0; i < Capacity; ++i)
            {
                _dict.Add(i, i);
                _immDict = _immDict.SetItem(i, i);
                _immSortedDict = _immSortedDict.SetItem(i, i);
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

        [Benchmark]
        public void SystemDictionary()
        {
            for(int t=0;t<Capacity;++t)
            {
                _dict.TryGetValue(_access[t], out _);
            }
        }

        [Benchmark]
        public void SystemImmutableDictionary()
        {
            for (int t = 0; t < Capacity; ++t)
            {
                _immDict.TryGetValue(_access[t], out _);
            }
        }

        [Benchmark]
        public void SystemSortedImmutableDictionary()
        {
            for (int t = 0; t < Capacity; ++t)
            {
                _immSortedDict.TryGetValue(_access[t], out _);
            }
        }

        [Benchmark]
        public void ApexHashMap()
        {
            for (int t = 0; t < Capacity; ++t)
            {
                _apexHashMap.TryGetValue(_access[t], out _);
            }
        }
    }
}
