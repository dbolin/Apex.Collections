using BenchmarkDotNet.Attributes;
using Sasa.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Benchmarks
{
    public class DictionariesModifyRemove<T> : DictionariesBase<T>
    {
        [Benchmark(Baseline = true)]
        public void ImmutableDictionary()
        {
            var t = _immDict;
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_keys[i]);
            }
        }

        [Benchmark]
        public void SasaTrie()
        {
            var t = _sasaTrie;
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_keys[i]);
            }
        }

        [Benchmark]
        public void ImmutableTrieDictionary()
        {
            var t = _sGuh;
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_keys[i]);
            }
        }

        [Benchmark]
        public void ImmutableTreeDictionary()
        {
            var t = _tvl;
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_keys[i]);
            }
        }

        [Benchmark]
        public void ApexHashMap()
        {
            var t = _apexHashMap;
            for (int i = 0; i < Count; ++i)
            {
                t = t.Remove(_keys[i]);
            }
        }
    }
}
