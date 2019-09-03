using BenchmarkDotNet.Attributes;
using Sasa.Collections;

namespace Benchmarks
{
    public class DictionariesLookup<T> : DictionariesBase<T>
    {
        [Benchmark(Baseline = true)]
        public void ImmutableDictionary()
        {
            for (int t = 0; t < Count; ++t)
            {
                _immDict.TryGetValue(_keys[t], out _);
            }
        }

        [Benchmark]
        public void SasaTrie()
        {
            for (int t = 0; t < Count; ++t)
            {
                _sasaTrie.TryGetValue(_keys[t], out _);
            }
        }

        [Benchmark]
        public void ImmutableTrieDictionary()
        {
            for (int t = 0; t < Count; ++t)
            {
                _sGuh.TryGetValue(_keys[t], out _);
            }
        }

        [Benchmark]
        public void ImmutableTreeDictionary()
        {
            for (int t = 0; t < Count; ++t)
            {
                _tvl.TryGetValue(_keys[t], out _);
            }
        }

        [Benchmark]
        public void ApexHashMap()
        {
            for (int t = 0; t < Count; ++t)
            {
                _apexHashMap.TryGetValue(_keys[t], out _);
            }
        }
    }
}
