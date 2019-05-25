using Apex.Collections;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benchmarks
{
    public class HashCode
    {

        [Benchmark]
        public int OrdinalIgnoreCase()
        {
            return System.StringComparer.OrdinalIgnoreCase.GetHashCode("asdfgh");
        }

        [Benchmark]
        public int NonRandomOrdinalIgnoreCase()
        {
            return Apex.Collections.StringComparer.NonRandomOrdinalIgnoreCase.GetHashCode("asdfgh");
        }

        [Benchmark]
        public int NonRandomOrdinal()
        {
            return Apex.Collections.StringComparer.NonRandomOrdinal.GetHashCode("asdfgh");
        }
    }
}
