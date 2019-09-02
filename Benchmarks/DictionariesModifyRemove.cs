using Apex.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using Sasa.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Benchmarks
{
    public class DictionariesModifyRemove : DictionariesBase
    {
        [Benchmark(Baseline = true)]
        public void ImmutableDictionary()
        {
            var t = _immDict;
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_access[i]);
            }
        }

        [Benchmark]
        public void SasaTrie()
        {
            var t = _sasaTrie;
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_access[i]);
            }
        }

        [Benchmark]
        public void ImmutableTrieDictionary()
        {
            var t = _sGuh;
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_access[i]);
            }
        }

        [Benchmark]
        public void ApexHashMap()
        {
            var t = _apexHashMap;
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_access[i]);
            }
        }
    }
}
