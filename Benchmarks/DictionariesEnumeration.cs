using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class DictionariesEnumeration<T> : DictionariesBase<T>
    {
        [Benchmark(Baseline = true)]
        public void ImmutableDictionary()
        {
            foreach (var kvp in _immDict)
                ;
        }

        [Benchmark]
        public void SasaTrie()
        {
            foreach (var kvp in _sasaTrie)
                ;
        }

        [Benchmark]
        public void ImmutableTrieDictionary()
        {
            foreach (var kvp in _sGuh)
                ;
        }

        [Benchmark]
        public void ImmutableTreeDictionary()
        {
            foreach (var kvp in _tvl)
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
