using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Apex.Collections.Tests
{
    public class LeastRecentlyUsedCacheTests
    {

        public static IEnumerable<object[]> Test1Data = new[] {
            new [] {
                new [] { "a", "bc", "def", "a", "ghij", "a", "klmno", "bc", "a", "p", "qr", "bc", "bc"}
            }
        };

        [Theory]
        [MemberData(nameof(Test1Data))]
        public void Test1(string[] keys)
        {
            var sut = new LeastRecentlyUsedCache<string, int>(5);

            AssertContentsPerStep(sut, k => k.Length, keys, new[] {
                new [] { "a" },
                new [] { "a", "bc" },
                new [] { "a", "bc", "def" },
                new [] { "bc", "def", "a" },
                new [] { "bc", "def", "a", "ghij" },
                new [] { "bc", "def", "ghij", "a" },
                new [] { "bc", "def", "ghij", "a", "klmno" },
                new [] { "def", "ghij", "a", "klmno", "bc" },
                new [] { "def", "ghij", "klmno", "bc", "a" },
                new [] { "ghij", "klmno", "bc", "a", "p" },
                new [] { "klmno", "bc", "a", "p", "qr" },
                new [] { "klmno", "a", "p", "qr", "bc" },
                new [] { "klmno", "a", "p", "qr", "bc" },
                }
            );
        }

        private void AssertContentsPerStep<K, V>(LeastRecentlyUsedCache<K, V> sut, Func<K, V> valueCreator, K[] keys, K[][] stepValues)
        {
            stepValues.Length.Should().Be(keys.Length, "one result is needed per step key");

            for(int i=0;i<keys.Length;++i)
            {
                sut.GetOrAdd(keys[i], valueCreator);

                AssertContents(sut, valueCreator, stepValues[i]);
            }
        }

        private void AssertContents<K, V>(LeastRecentlyUsedCache<K, V> sut, Func<K, V> valueCreator, K[] k)
        {
            int index = 0;
            foreach(var kvp in sut)
            {
                var expectedKey = k[index];
                var expectedValue = valueCreator(expectedKey);

                kvp.Should().BeEquivalentTo(new KeyValuePair<K, V>(expectedKey, expectedValue));
                index++;
            }
        }
    }
}
