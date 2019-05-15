using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<TKey, TValue>
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static uint GetBitMask(int hash)
        {
            int bitsFromHash = hash & Branch.SubMask;
            uint bitmask = 1U << bitsFromHash;
            return bitmask;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static Branch CreateFrom(IEqualityComparer<TKey> equalityComparer, ValueNode node, int level, int hash, TKey key, TValue value,
            bool mutable)
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
                    return new Branch(0, 0, newNodesInner, Array.Empty<Branch>());
                }

                var nextBranch = CreateFrom(equalityComparer, node, level + Branch.BitWidth, hash, key, value, mutable);
                return new Branch(!mutable, 0, firstBitMask, Array.Empty<ValueNode>(), new[] { nextBranch });
            }

            var resultBitBask = firstBitMask | secondBitMask;

            var newNodes = new ValueNode[2];
            if (firstBitMask < secondBitMask)
            {
                newNodes[0] = node;
                newNodes[1] = new ValueNode(key, value);
                return new Branch(!mutable, resultBitBask, 0, newNodes, Array.Empty<Branch>());
            }

            newNodes[0] = new ValueNode(key, value);
            newNodes[1] = node;
            return new Branch(!mutable, resultBitBask, 0, newNodes, Array.Empty<Branch>());
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static T[] Insert<T>(T[] array, int index, T item)
        {
            var newArray = new T[array.Length + 1];
            newArray[index] = item;

            if (index != 0)
            {
                Array.Copy(array, 0, newArray, 0, index);
            }
            if (index != array.Length)
            {
                Array.Copy(array, index, newArray, index + 1, array.Length - index);
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

            if (array.Length == 1)
            {
                return newArray;
            }

            Array.Copy(array, 0, newArray, 0, index);
            if (index < newArray.Length)
            {
                Array.Copy(array, index + 1, newArray, index, array.Length - index - 1);
            }

            return newArray;
        }
    }
}
