using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class DictionariesBuilderRemove<T> : DictionariesBase<T>
    {
        [Benchmark(Baseline = true)]
        public object ImmutableDictionary()
        {
            var t = _immDict;
            t = t.RemoveRange(_keys);
            return t;
        }

        [Benchmark]
        public object ImmutableTrieDictionary()
        {
            var t = _sGuh;
            t = t.RemoveRange(_keys);
            return t;
        }

        [Benchmark]
        public object ApexHashMap()
        {
            var t = _apexHashMap;
            t = t.RemoveRange(_keys);
            return t;
        }
    }
}
