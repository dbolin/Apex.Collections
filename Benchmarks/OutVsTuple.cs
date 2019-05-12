using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benchmarks
{
    public class OutVsTuple
    {
        [Params(1,3,5)]
        public int Depth { get; set; }

        public bool OutMethod<T>(int depth, out T result)
        {
            if(depth == 0)
            {
                result = default;
                return true;
            }

            return OutMethod<T>(depth - 1, out result);
        }

        public (bool, T) TupleMethod<T>(int depth)
        {
            if(depth == 0)
            {
                return (true, default);
            }

            return TupleMethod<T>(depth - 1);
        }


        [Benchmark]
        public int OutInt()
        {
            OutMethod<int>(Depth, out var result);
            return result;
        }

        [Benchmark]
        public int TupleInt()
        {
            var (_, result) = TupleMethod<int>(Depth);
            return result;
        }

        [Benchmark]
        public Guid OutGuid()
        {
            OutMethod<Guid>(Depth, out var result);
            return result;
        }

        [Benchmark]
        public Guid TupleGuid()
        {
            var (_, result) = TupleMethod<Guid>(Depth);
            return result;
        }

    }
}
