using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<K, V>
    {
        internal sealed class Branch
        {
            private const int BitWidth = 5;
            private const int SubMask = (1 << BitWidth) - 1;
            private const int MaxLevel = 32;

            public static readonly Branch Empty = new Branch(0, 0, ImmutableArray<ValueNode>.Empty, ImmutableArray<Branch>.Empty);

            public uint BitMaskValues { get; }
            public uint BitMaskBranches { get; }
            public ImmutableArray<ValueNode> Values { get; }
            public ImmutableArray<Branch> Branches { get; }

            public Branch(uint bitMaskValues, uint bitMaskBranches, ImmutableArray<ValueNode> nodes, ImmutableArray<Branch> branches)
            {
                BitMaskValues = bitMaskValues;
                BitMaskBranches = bitMaskBranches;
                Values = nodes;
                Branches = branches;
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public Branch Set(IEqualityComparer<K> equalityComparer, int hash, int level, K key, V value, out bool added)
            {
                uint bitmask = GetBitMask(hash);
                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    var branch = Branches[branchIndex];
                    var newBranch = branch.Set(equalityComparer, hash >> 5, level + BitWidth, key, value, out added);
                    return new Branch(BitMaskValues, BitMaskBranches, Values, Branches.SetItem(branchIndex, newBranch));
                }

                var valueIndex = (int)Popcnt.PopCount(BitMaskValues & (bitmask - 1));
                if ((BitMaskValues & bitmask) != 0)
                {
                    var node = Values[valueIndex];

                    if (equalityComparer.Equals(node.Key, key))
                    {
                        added = false;
                        return new Branch(BitMaskValues, BitMaskBranches, Values.SetItem(valueIndex, new ValueNode(key, value)), Branches);
                    }

                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    var branch = CreateFrom(equalityComparer, node, level + BitWidth, hash >> 5, key, value);
                    added = true;
                    return new Branch(BitMaskValues & (~bitmask), BitMaskBranches | bitmask, Values.RemoveAt(valueIndex), Branches.Insert(branchIndex, branch));
                }

                added = true;
                return new Branch(BitMaskValues | bitmask, BitMaskBranches, Values.Insert(valueIndex, new ValueNode(key, value)), Branches);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public Branch Remove(IEqualityComparer<K> equalityComparer, int hash, int level, K key, out bool removed)
            {
                uint bitmask = GetBitMask(hash);
                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    var newBranch = Branches[branchIndex].Remove(equalityComparer, hash >> 5, level + BitWidth, key, out removed);
                    if (newBranch.Branches.Length == 0 && newBranch.Values.Length == 0)
                    {
                        return new Branch(BitMaskValues, BitMaskBranches & (~bitmask), Values, Branches.RemoveAt(branchIndex));
                    }

                    return new Branch(BitMaskValues, BitMaskBranches, Values, Branches.SetItem(branchIndex, newBranch));
                }

                if ((BitMaskValues & bitmask) != 0)
                {
                    var valueIndex = (int)Popcnt.PopCount(BitMaskValues & (bitmask - 1));
                    var node = Values[valueIndex];
                    if (equalityComparer.Equals(node.Key, key))
                    {
                        removed = true;
                        return new Branch(BitMaskValues & (~bitmask), BitMaskBranches, Values.RemoveAt(valueIndex), Branches);
                    }
                }

                return RemoveCollisionOrNone(equalityComparer, level, key, out removed);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            private Branch RemoveCollisionOrNone(IEqualityComparer<K> equalityComparer, int level, K key, out bool removed)
            {
                // hash collision
                if (level >= MaxLevel)
                {
                    for (int i = 0; i < Values.Length; ++i)
                    {
                        if (equalityComparer.Equals(Values[i].Key, key))
                        {
                            removed = true;
                            return new Branch(0, 0, Values.RemoveAt(i), ImmutableArray<Branch>.Empty);
                        }
                    }
                }

                removed = false;
                return this;
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public bool TryGet(IEqualityComparer<K> equalityComparer, int hash, int level, K key, out V value)
            {
                uint bitmask = GetBitMask(hash);

                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    return Branches[branchIndex].TryGet(equalityComparer, hash >> 5, level + BitWidth, key, out value);
                }

                if(TryGetValueInternal(bitmask, equalityComparer, key, out value))
                {
                    return true;
                }

                // hash collision if this is max
                return GetCollisionOrNone(equalityComparer, key, level, out value);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            private bool TryGetValueInternal(uint bitmask, IEqualityComparer<K> equalityComparer, K key, out V value)
            {
                if ((BitMaskValues & bitmask) != 0)
                {
                    var index = (int)Popcnt.PopCount(BitMaskValues & (bitmask - 1));
                    var node = Values[index];

                    if (equalityComparer.Equals(node.Key, key))
                    {
                        value = node.Value;
                        return true;
                    }
                }

                value = default;
                return false;
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            private bool GetCollisionOrNone(IEqualityComparer<K> equalityComparer, K key, int level, out V value)
            {
                if (level >= MaxLevel)
                {
                    for (int i = 0; i < Values.Length; ++i)
                    {
                        if (equalityComparer.Equals(Values[i].Key, key))
                        {
                            value = Values[i].Value;
                            return true;
                        }
                    }
                }

                value = default;
                return false;
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            private static uint GetBitMask(int hash)
            {
                int bitsFromHash = hash & SubMask;
                uint bitmask = 1U << bitsFromHash;
                return bitmask;
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            private static Branch CreateFrom(IEqualityComparer<K> equalityComparer, ValueNode node, int level, int hash, K key, V value)
            {
                var firstBitMask = GetBitMask(equalityComparer.GetHashCode(node.Key) >> level);
                var secondBitMask = GetBitMask(hash);

                if (firstBitMask == secondBitMask)
                {
                    // hash collision
                    if(level >= MaxLevel)
                    {
                        return new Branch(0, 0, ImmutableArray.Create(node, new ValueNode(key, value)), ImmutableArray<Branch>.Empty);
                    }

                    var nextBranch = CreateFrom(equalityComparer, node, level + BitWidth, hash, key, value);
                    return new Branch(0, firstBitMask, ImmutableArray<ValueNode>.Empty, ImmutableArray.Create(nextBranch));
                }

                var resultBitBask = firstBitMask | secondBitMask;

                if(firstBitMask < secondBitMask)
                {
                    return new Branch(resultBitBask, 0, ImmutableArray.Create(node, new ValueNode(key, value)), ImmutableArray<Branch>.Empty);
                }

                return new Branch(resultBitBask, 0, ImmutableArray.Create(new ValueNode(key, value), node), ImmutableArray<Branch>.Empty);
            }
        }
    }
}
