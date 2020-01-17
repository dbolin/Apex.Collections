using Apex.Collections;
using BenchmarkDotNet.Attributes;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace Benchmarks
{
    public class PriorityQueue
    {
        private List<int> _randomInts = new List<int>();
        private PriorityQueueRx<Item> _pqRx = new PriorityQueueRx<Item>();
        private PriorityQueue<Item> _pqa = new PriorityQueue<Item>();
        private Priority_Queue.FastPriorityQueue<FPQ_Node> _fpq = new Priority_Queue.FastPriorityQueue<FPQ_Node>(10000);

        [GlobalSetup]
        public void Init()
        {
            for (int i = 0; i < Count; ++i)
            {
                _pqRx.Enqueue(new Item(i));
                _pqa.Enqueue(new Item(i));
            }

            var r = new Random();
            for(int i=0;i<Count;++i)
            {
                _randomInts.Add(r.Next(100000));
            }
        }

        [Params(100, 10000)]
        public int Count { get; set; }

        public struct Item : IComparable<Item>
        {
            public int Priority;

            public Item(int i)
            {
                Priority = i;
            }

            public int CompareTo(Item other)
            {
                return Priority.CompareTo(other.Priority);
            }
        }

        public class FPQ_Node : FastPriorityQueueNode
        {
            public FPQ_Node(int priority)
            {
                Priority = priority;
            }
        }

        [Benchmark]
        public void PQRxAddHighest()
        {
            var x = new PriorityQueueRx<Item>(Count);
            for (int i = 0; i < Count; ++i)
            {
                x.Enqueue(new Item(i));
            }
        }

        [Benchmark]
        public void PQRxAddLowest()
        {
            var x = new PriorityQueueRx<Item>(Count);
            for (int i = 0; i < Count; ++i)
            {
                x.Enqueue(new Item(-i));
            }
        }

        [Benchmark]
        public void PQRxAddRandom()
        {
            var x = new PriorityQueueRx<Item>(Count);
            for (int i = 0; i < Count; ++i)
            {
                var p = _randomInts[i];
                x.Enqueue(new Item(p));
            }
        }

        [Benchmark]
        public void PQApexAddHighest()
        {
            var x = new PriorityQueue<Item>(Count);
            for (int i = 0; i < Count; ++i)
            {
                x.Enqueue(new Item(i));
            }
        }

        [Benchmark]
        public void PQApexAddLowest()
        {
            var x = new PriorityQueue<Item>(Count);
            for (int i = 0; i < Count; ++i)
            {
                x.Enqueue(new Item(-i));
            }
        }

        [Benchmark]
        public void PQApexAddRandom()
        {
            var x = new PriorityQueue<Item>(Count);
            for (int i = 0; i < Count; ++i)
            {
                var p = _randomInts[i];
                x.Enqueue(new Item(p));
            }
        }

        [Benchmark]
        public void PQFastAddHighest()
        {
            var x = new FastPriorityQueue<FPQ_Node>(Count);
            for (int i = 0; i < Count; ++i)
            {
                x.Enqueue(new FPQ_Node(i), i);
            }
        }

        [Benchmark]
        public void PQFastAddLowest()
        {
            var x = new FastPriorityQueue<FPQ_Node>(Count);
            for (int i = 0; i < Count; ++i)
            {
                x.Enqueue(new FPQ_Node(-i), -i);
            }
        }

        [Benchmark]
        public void PQFastAddRandom()
        {
            var x = new FastPriorityQueue<FPQ_Node>(Count);
            for (int i = 0; i < Count; ++i)
            {
                var p = _randomInts[i];
                x.Enqueue(new FPQ_Node(p), p);
            }
        }
    }
}
