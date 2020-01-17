using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Apex.Collections.Tests
{
    public class PriorityQueueTests
    {
        [Theory]
        [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })]
        [InlineData(new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 })]
        [InlineData(new[] { 1, 6, 7, 3, 4, 2, 9, 8, 4 })]
        [InlineData(new[] { 4, 4, 4, 4, 1, 1, 2, 2, 9 })]
        public void SortedIntegers(int[] input)
        {
            var sut = new PriorityQueue<int>();

            foreach(var i in input)
            {
                sut.Enqueue(i);
            }

            var sorted = input.OrderBy(x => x).ToList();
            for(int i=0;i<sorted.Count;++i)
            {
                sut.Dequeue().Should().Be(sorted[i]);
            }
        }

        [Theory]
        [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })]
        [InlineData(new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 })]
        [InlineData(new[] { 1, 6, 7, 3, 4, 2, 9, 8, 4 })]
        [InlineData(new[] { 4, 4, 4, 4, 1, 1, 2, 2, 9 })]
        public void PeekReturnsFirstElement(int[] input)
        {
            var sut = new PriorityQueue<int>();

            foreach (var i in input)
            {
                sut.Enqueue(i);
            }

            sut.Peek().Should().Be(1);
        }
    }
}
