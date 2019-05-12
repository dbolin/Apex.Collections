using System.Collections.Generic;
using System.Collections.Immutable;
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

            public Branch Set(IEqualityComparer<K> equalityComparer, int hash, int level, K key, V value, out bool added)
            {
                uint bitmask = GetBitMask(hash, level);
                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    var branch = Branches[branchIndex];
                    var newBranch = branch.Set(equalityComparer, hash, level + BitWidth, key, value, out added);
                    return new Branch(BitMaskValues, BitMaskBranches, Values, Branches.SetItem(branchIndex, newBranch));
                }

                var valueIndex = (int)Popcnt.PopCount(BitMaskValues & (bitmask - 1));
                if ((BitMaskValues & bitmask) != 0)
                {
                    var node = Values[valueIndex];

                    if (node.FullHash == hash && equalityComparer.Equals(node.Key, key))
                    {
                        added = false;
                        return new Branch(BitMaskValues, BitMaskBranches, Values.SetItem(valueIndex, new ValueNode(hash, key, value)), Branches);
                    }

                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    var branch = CreateFrom(node, level + BitWidth, hash, key, value);
                    added = true;
                    return new Branch(BitMaskValues & (~bitmask), BitMaskBranches | bitmask, Values.RemoveAt(valueIndex), Branches.Insert(branchIndex, branch));
                }

                added = true;
                return new Branch(BitMaskValues | bitmask, BitMaskBranches, Values.Insert(valueIndex, new ValueNode(hash, key, value)), Branches);
            }

            public Branch Remove(IEqualityComparer<K> equalityComparer, int hash, int level, K key, out bool removed)
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

                    removed = false;
                    return this;
                }

                uint bitmask = GetBitMask(hash, level);
                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    var newBranch = Branches[branchIndex].Remove(equalityComparer, hash, level + BitWidth, key, out removed);
                    if (newBranch.Branches.Length == 0 && newBranch.Values.Length == 0)
                    {
                        return new Branch(BitMaskValues, BitMaskBranches & (~bitmask), Values, Branches.RemoveAt(branchIndex));
                    }

                    return new Branch(BitMaskValues, BitMaskBranches, Values, Branches.SetItem(branchIndex, newBranch));
                }

                if ((BitMaskValues & bitmask) == 0)
                {
                    removed = false;
                    return this;
                }

                var valueIndex = (int)Popcnt.PopCount(BitMaskValues & (bitmask - 1));
                var node = Values[valueIndex];

                if (node.FullHash == hash && equalityComparer.Equals(node.Key, key))
                {
                    removed = true;
                    return new Branch(BitMaskValues & (~bitmask), BitMaskBranches, Values.RemoveAt(valueIndex), Branches);
                }

                removed = false;
                return this;
            }

            public bool TryGet(IEqualityComparer<K> equalityComparer, int hash, int level, K key, out V value)
            {
                uint bitmask = GetBitMask(hash, level);

                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    return Branches[branchIndex].TryGet(equalityComparer, hash, level + BitWidth, key, out value);
                }

                if ((BitMaskValues & bitmask) != 0)
                {
                    var index = (int)Popcnt.PopCount(BitMaskValues & (bitmask - 1));
                    var node = Values[index];

                    if (node.FullHash == hash && equalityComparer.Equals(node.Key, key))
                    {
                        value = node.Value;
                        return true;
                    }
                }

                // hash collision
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

            private static uint GetBitMask(int hash, int level)
            {
                int bitsFromHash = (hash >> level) & SubMask;
                uint bitmask = (uint)(1 << bitsFromHash);
                return bitmask;
            }

            private static Branch CreateFrom(ValueNode node, int level, int hash, K key, V value)
            {
                var firstBitMask = GetBitMask(node.FullHash, level);
                var secondBitMask = GetBitMask(hash, level);

                if (firstBitMask == secondBitMask)
                {
                    // hash collision
                    if(level >= MaxLevel)
                    {
                        return new Branch(0, 0, ImmutableArray.Create(node, new ValueNode(hash, key, value)), ImmutableArray<Branch>.Empty);
                    }

                    var nextBranch = CreateFrom(node, level + BitWidth, hash, key, value);
                    return new Branch(0, firstBitMask, ImmutableArray<ValueNode>.Empty, ImmutableArray.Create(nextBranch));
                }

                var resultBitBask = firstBitMask | secondBitMask;

                if(firstBitMask < secondBitMask)
                {
                    return new Branch(resultBitBask, 0, ImmutableArray.Create(node, new ValueNode(hash, key, value)), ImmutableArray<Branch>.Empty);
                }

                return new Branch(resultBitBask, 0, ImmutableArray.Create(new ValueNode(hash, key, value), node), ImmutableArray<Branch>.Empty);
            }
        }
    }
}
