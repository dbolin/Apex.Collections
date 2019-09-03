using Apex.Collections.Immutable;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Apex.Collections.Tests
{
    public class HashMapTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(-1)]
        [InlineData(555)]
        [InlineData(12345)]
        [InlineData(51234123)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void SingleValue(int n)
        {
            var sut = HashMap<int, int>.Empty;
            var t = sut.SetItem(n, n);

            var result1 = t.TryGetValue(n, out var v1);
            var result2 = t.TryGetValue(777, out _);
            var result3 = sut.TryGetValue(n, out _);
            result1.Should().BeTrue();
            result2.Should().BeFalse();
            result3.Should().BeFalse();

            v1.Should().Be(n);

            t = t.Remove(n);
            t.Count.Should().Be(0);
            t.TryGetValue(n, out _).Should().BeFalse();
        }

        [Fact]
        public void ReverseInsertOrder()
        {
            var sut = HashMap<int, int>.Empty;
            sut = sut.SetItem(5, 5);
            sut = sut.SetItem(1, 1);

            sut.Count.Should().Be(2);
        }

        [Fact]
        public void RemoveNonExisting()
        {
            var sut = HashMap<int, int>.Empty;
            sut = sut.SetItem(1, 1);
            sut = sut.Remove(2);
            sut.Count.Should().Be(1);
        }

        [Fact]
        public void ManyValues()
        {
            var sut = HashMap<int, int>.Empty;

            for(int i=0;i<10000;++i)
            {
                sut = sut.SetItem(i, i);
                sut.Count.Should().Be(i + 1);
            }

            for (int i = 0; i < 10000; ++i)
            {
                sut = sut.Remove(i);
                sut.Count.Should().Be(9999 - i);
            }
        }

        [Fact]
        public void Enumeration()
        {
            var sut = HashMap<int, int>.Empty;

            for (int i = 0; i < 10000; ++i)
            {
                sut = sut.SetItem(i, i);
            }

            var enumerated = sut.OrderBy(x => x.Key).ToList();
            for (int i = 0; i < 10000; ++i)
            {
                enumerated[i].Key.Should().Be(i);
                enumerated[i].Value.Should().Be(i);
            }
        }

        [Fact]
        public void DuplicateKey()
        {
            var sut = HashMap<int, int>.Empty;
            var t = sut.SetItem(1, 1);
            t = t.SetItem(1, 1);
            t.Count.Should().Be(1);

            t = t.SetItem(3, 3);
            t = t.SetItem(3, 3);
            t.Count.Should().Be(2);
        }

        [Fact]
        public void HashCollision()
        {
            var sut = HashMap<KeyWithHashCode, int>.Empty;
            var key1 = new KeyWithHashCode(1, 1);
            var key2 = new KeyWithHashCode(2, 1);

            var t = sut.SetItem(key1, 1);
            t = t.SetItem(key2, 2);

            t.TryGetValue(key1, out var r1);
            t.TryGetValue(key2, out var r2);

            r1.Should().Be(1);
            r2.Should().Be(2);

            var count = 0;
            foreach(var x in t)
            {
                count++;
            }
            count.Should().Be(2);

            t = t.Remove(key1);
            t.Count.Should().Be(1);

            t.TryGetValue(key1, out r1);
            t.TryGetValue(key2, out r2);

            r1.Should().Be(0);
            r2.Should().Be(2);
        }

        [Fact]
        public void SimilarHashes()
        {
            var sut = HashMap<KeyWithHashCode, int>.Empty;
            var key1 = new KeyWithHashCode(1, 0x7FFFFFFF);
            var key2 = new KeyWithHashCode(2, 0x6FFFFFFF);

            var t = sut.SetItem(key1, 1);
            t = t.SetItem(key2, 2);

            t.TryGetValue(key1, out var r1);
            t.TryGetValue(key2, out var r2);

            r1.Should().Be(1);
            r2.Should().Be(2);

            t = t.Remove(key1);
            t.Count.Should().Be(1);

            t.TryGetValue(key1, out r1);
            t.TryGetValue(key2, out r2);

            r1.Should().Be(0);
            r2.Should().Be(2);
        }

        [Fact]
        public void SetItemsFromEmpty()
        {
            var sut = HashMap<int, int>.Empty;
            var list = new List<KeyValuePair<int, int>>();
            for(int i=0;i<10000;++i)
            {
                list.Add(new KeyValuePair<int, int>(i, i));
            }

            sut = sut.SetItems(list);
            sut.Count.Should().Be(10000);

            for (int i = 0; i < 10000; ++i)
            {
                sut.TryGetValue(i, out var v);
                v.Should().Be(i);
            }
        }

        [Fact]
        public void SetItemsSharedStructure()
        {
            var sut = HashMap<int, int>.Empty;
            var list = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < 10000; ++i)
            {
                if (i < 5000)
                {
                    sut = sut.SetItem(i, i);
                }
                else
                {
                    list.Add(new KeyValuePair<int, int>(i, i));
                }
            }

            var sut5000 = sut;

            sut = sut.SetItems(list);

            for (int i = 0; i < 10000; ++i)
            {
                sut.TryGetValue(i, out var v);
                v.Should().Be(i);
            }

            sut5000.Count.Should().Be(5000);
            for (int i = 0; i < 10000; ++i)
            {
                sut5000.TryGetValue(i, out var v);
                if (i < 5000)
                {
                    v.Should().Be(i);
                }
                else
                {
                    v.Should().Be(0);
                }
            }

            for (int i = 0; i < 5000; ++i)
            {
                sut5000 = sut5000.Remove(i);
            }

            for (int i = 0; i < 10000; ++i)
            {
                sut.TryGetValue(i, out var v);
                v.Should().Be(i);
            }
        }

        [Fact]
        public void RemoveRange()
        {
            var sut = HashMap<int, int>.Empty;
            var list = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < 10000; ++i)
            {
                if (i > 5000)
                {
                    list.Add(new KeyValuePair<int, int>(i, i));
                }
                sut = sut.SetItem(i, i);
            }

            sut.Count.Should().Be(10000);

            sut = sut.RemoveRange(list.Select(x => x.Key));

            sut.Count.Should().Be(5001);

            for (int i = 0; i < 10000; ++i)
            {
                sut.TryGetValue(i, out var v);
                if (i <= 5000)
                {
                    v.Should().Be(i);
                }
                else
                {
                    v.Should().Be(0);
                }
            }
        }

        [Fact]
        public void Strings()
        {
            var sut = HashMap<string, int>.Empty.WithComparer(Apex.Collections.StringComparer.NonRandomOrdinalIgnoreCase);

            for(int i=0;i<1000;++i)
            {
                var k = $"t{i}";
                sut = sut.SetItem(k, i);
                sut.TryGetValue(k.ToUpperInvariant(), out var v);
                v.Should().Be(i);
                sut = sut.Remove(k);
                sut.Count.Should().Be(0);
            }

            sut = HashMap<string, int>.Empty.WithComparer(Apex.Collections.StringComparer.NonRandomOrdinalIgnoreCase);

            for (int i = 0; i < 1000; ++i)
            {
                var k = $"tķ{i}";
                sut = sut.SetItem(k, i);
                sut.TryGetValue(k.ToUpperInvariant(), out var v);
                v.Should().Be(i);
                sut = sut.Remove(k);
                sut.Count.Should().Be(0);
            }

            sut = HashMap<string, int>.Empty.WithComparer(Apex.Collections.StringComparer.NonRandomOrdinal);

            for (int i = 0; i < 1000; ++i)
            {
                var k = $"t{i}";
                sut = sut.SetItem(k, i);
                sut.TryGetValue(k, out var v);
                v.Should().Be(i);
                sut = sut.Remove(k);
                sut.Count.Should().Be(0);
            }

        }


        private class KeyWithHashCode : IEquatable<KeyWithHashCode>
        {
            public int Key;
            public int HashCode;

            public KeyWithHashCode(int key, int hashCode)
            {
                Key = key;
                HashCode = hashCode;
            }

            public override bool Equals(object obj) => Equals(obj as KeyWithHashCode);
            public bool Equals(KeyWithHashCode other) => other != null && Key == other.Key && HashCode == other.HashCode;
            public override int GetHashCode() => HashCode.GetHashCode();
        }
    }
}
