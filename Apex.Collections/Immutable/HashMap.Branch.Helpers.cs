using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<TKey, TValue>
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static unsafe int PopCount(uint x)
        {
            if(Popcnt.IsSupported)
            {
                return (int)Popcnt.PopCount(x);
            }

            return PrecomputedPopcnt.Bits.wordBits[x & 0xFFFF] + PrecomputedPopcnt.Bits.wordBits[x >> 16];
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static uint GetBitMask(int hash)
        {
            int bitsFromHash = hash & Branch.SubMask;
            uint bitmask = 1U << bitsFromHash;
            return bitmask;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static Branch CreateFrom(IEqualityComparer<TKey> equalityComparer, ValueNode node, int level, int hash, TKey key, TValue value,
            BuilderToken builderToken)
        {
            var firstBitMask = GetBitMask(equalityComparer.GetHashCode(node.Key) >> level);
            var secondBitMask = GetBitMask(hash);

            if (firstBitMask == secondBitMask)
            {
                // hash collision
                if (level >= Branch.MaxLevel)
                {
                    var newNodesInner = new ValueNode[2];
                    newNodesInner[0] = node;
                    newNodesInner[1] = new ValueNode(key, value);
                    return new Branch(builderToken, 0, 0, newNodesInner, Array.Empty<Branch>());
                }

                var nextBranch = CreateFrom(equalityComparer, node, level + Branch.BitWidth, hash >> 5, key, value, builderToken);
                return new Branch(builderToken, 0, firstBitMask, Array.Empty<ValueNode>(), new[] { nextBranch });
            }

            var resultBitBask = firstBitMask | secondBitMask;

            var newNodes = new ValueNode[2];
            if (firstBitMask < secondBitMask)
            {
                newNodes[0] = node;
                newNodes[1] = new ValueNode(key, value);
                return new Branch(builderToken, resultBitBask, 0, newNodes, Array.Empty<Branch>());
            }

            newNodes[0] = new ValueNode(key, value);
            newNodes[1] = node;
            return new Branch(builderToken, resultBitBask, 0, newNodes, Array.Empty<Branch>());
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static T[] SetItem<T>(T[] array, int index, T item)
        {
            var length = array.Length;
            var newArray = new T[length];

            if (length != 1)
            {
                Array.Copy(array, 0, newArray, 0, array.Length);
            }
            newArray[index] = item;

            return newArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static T[] Insert<T>(T[] array, int index, T item)
        {
            var newArray = new T[array.Length + 1];
            newArray[index] = item;

            if(array.Length == 0)
            {
                return newArray;
            }

            int s = 0;
            for (int d = 0; d != index; ++s, ++d)
            {
                newArray[d] = array[s];
            }

            for (int d = index + 1; d < newArray.Length; ++s, ++d)
            {
                newArray[d] = array[s];
            }

            return newArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static T[] RemoveAt<T>(T[] array, int index)
        {
            if (array.Length == 1)
            {
                return Array.Empty<T>();
            }

            var newArray = new T[array.Length - 1];

            int s = 0;
            for (int d = 0; d < newArray.Length; ++s, ++d)
            {
                if(s == index)
                {
                    d--;
                    continue;
                }
                newArray[d] = array[s];
            }

            return newArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static T[] BuilderInsert<T>(T[] array, int index, T item, ArrayPooler<T> arrayPool)
        {
            var newArray = arrayPool.Get(array.Length + 1);
            newArray[index] = item;

            if (array.Length == 0)
            {
                arrayPool.Return(array);
                return newArray;
            }

            int s = 0;
            for (int d = 0; d != index; ++s, ++d)
            {
                newArray[d] = array[s];
            }

            for (int d = index + 1; d < newArray.Length; ++s, ++d)
            {
                newArray[d] = array[s];
            }

            arrayPool.Return(array);
            return newArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static T[] BuilderRemoveAt<T>(T[] array, int index, ArrayPooler<T> arrayPool)
        {
            if (array.Length == 1)
            {
                arrayPool.Return(array);
                return Array.Empty<T>();
            }

            var newArray = arrayPool.Get(array.Length - 1);
            int s = 0;
            for (int d = 0; d < newArray.Length; ++s, ++d)
            {
                if (s == index)
                {
                    d--;
                    continue;
                }
                newArray[d] = array[s];
            }

            arrayPool.Return(array);
            return newArray;
        }
    }
}
