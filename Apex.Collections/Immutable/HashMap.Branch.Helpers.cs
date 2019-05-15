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
        private static Branch CreateFrom(IEqualityComparer<TKey> equalityComparer, ValueNode node, int level, int hash, TKey key, TValue value, bool mutable)
        {
            var firstBitMask = GetBitMask(equalityComparer.GetHashCode(node.Key) >> level);
            var secondBitMask = GetBitMask(hash);

            if (firstBitMask == secondBitMask)
            {
                // hash collision
                if (level >= Branch.MaxLevel)
                {
                    return new Branch(0, 0, new[] { node, new ValueNode(key, value) }, Array.Empty<Branch>());
                }

                var nextBranch = CreateFrom(equalityComparer, node, level + Branch.BitWidth, hash, key, value, mutable);
                return new Branch(!mutable, 0, firstBitMask, Array.Empty<ValueNode>(), new[] { nextBranch });
            }

            var resultBitBask = firstBitMask | secondBitMask;

            if (firstBitMask < secondBitMask)
            {
                return new Branch(!mutable, resultBitBask, 0, new[] { node, new ValueNode(key, value) }, Array.Empty<Branch>());
            }

            return new Branch(!mutable, resultBitBask, 0, new[] { new ValueNode(key, value), node }, Array.Empty<Branch>());
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static unsafe T[] Insert<T>(T[] array, int index, T item)
        {
            if (array.Length == 0)
            {
                return new T[] { item };
            }

            T[] tmp = new T[array.Length + 1];
            tmp[index] = item;

            if (index != 0)
            {
                Unsafe.CopyBlock(Unsafe.AsPointer(ref tmp[0]), Unsafe.AsPointer(ref array[0]), (uint)(Unsafe.SizeOf<T>() * index));
            }
            if (index != array.Length)
            {
                Unsafe.CopyBlock(Unsafe.AsPointer(ref tmp[index + 1]), Unsafe.AsPointer(ref array[index]), (uint)(Unsafe.SizeOf<T>() * (array.Length - index)));
            }

            return tmp;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static unsafe T[] RemoveAt<T>(T[] array, int index)
        {
            if (array.Length == 1)
            {
                return Array.Empty<T>();
            }

            T[] tmp = new T[array.Length - 1];
            Unsafe.CopyBlock(Unsafe.AsPointer(ref tmp[0]), Unsafe.AsPointer(ref array[0]), (uint)(Unsafe.SizeOf<T>() * index));
            if (index < tmp.Length)
            {
                Unsafe.CopyBlock(Unsafe.AsPointer(ref tmp[index]), Unsafe.AsPointer(ref array[index + 1]), (uint)(Unsafe.SizeOf<T>() * (array.Length - index - 1)));
            }

            return tmp;
        }
    }
}
