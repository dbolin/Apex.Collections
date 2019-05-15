﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<TKey, TValue>
    {
        internal sealed partial class Branch
        {
            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public Branch Set(IEqualityComparer<TKey> equalityComparer, int hash, int level, TKey key, TValue value, out bool added)
            {
                uint bitmask = GetBitMask(hash);
                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    var branch = Branches[branchIndex];
                    var newBranch = branch.Set(equalityComparer, hash >> 5, level + BitWidth, key, value, out added);
                    return new Branch(BitMaskValues, BitMaskBranches, Values, SetItem(Branches, branchIndex, newBranch));
                }

                var valueIndex = (int)Popcnt.PopCount(BitMaskValues & (bitmask - 1));
                if ((BitMaskValues & bitmask) != 0)
                {
                    var node = Values[valueIndex];

                    if (equalityComparer.Equals(node.Key, key))
                    {
                        added = false;
                        return new Branch(BitMaskValues, BitMaskBranches, SetItem(Values, valueIndex, new ValueNode(key, value)), Branches);
                    }

                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    var branch = CreateFrom(equalityComparer, node, level + BitWidth, hash >> 5, key, value, false);
                    added = true;
                    return new Branch(BitMaskValues & (~bitmask), BitMaskBranches | bitmask, RemoveAt(Values, valueIndex), Insert(Branches, branchIndex, branch));
                }

                added = true;
                return new Branch(BitMaskValues | bitmask, BitMaskBranches, Insert(Values, valueIndex, new ValueNode(key, value)), Branches);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public Branch Remove(IEqualityComparer<TKey> equalityComparer, int hash, int level, TKey key, out bool removed)
            {
                uint bitmask = GetBitMask(hash);
                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    var newBranch = Branches[branchIndex].Remove(equalityComparer, hash >> 5, level + BitWidth, key, out removed);
                    if (newBranch.Branches.Length == 0 && newBranch.Values.Length == 0)
                    {
                        return new Branch(BitMaskValues, BitMaskBranches & (~bitmask), Values, RemoveAt(Branches, branchIndex));
                    }

                    return new Branch(BitMaskValues, BitMaskBranches, Values, SetItem(Branches, branchIndex, newBranch));
                }

                if ((BitMaskValues & bitmask) != 0)
                {
                    var valueIndex = (int)Popcnt.PopCount(BitMaskValues & (bitmask - 1));
                    var node = Values[valueIndex];
                    if (equalityComparer.Equals(node.Key, key))
                    {
                        removed = true;
                        return new Branch(BitMaskValues & (~bitmask), BitMaskBranches, RemoveAt(Values, valueIndex), Branches);
                    }
                }

                return RemoveCollisionOrNone(equalityComparer, level, key, out removed);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            private Branch RemoveCollisionOrNone(IEqualityComparer<TKey> equalityComparer, int level, TKey key, out bool removed)
            {
                // hash collision
                if (level >= MaxLevel)
                {
                    for (int i = 0; i < Values.Length; ++i)
                    {
                        if (equalityComparer.Equals(Values[i].Key, key))
                        {
                            removed = true;
                            return new Branch(0, 0, RemoveAt(Values, i), Array.Empty<Branch>());
                        }
                    }
                }

                removed = false;
                return this;
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public bool TryGet(IEqualityComparer<TKey> equalityComparer, int hash, int level, TKey key, out TValue value)
            {
                uint bitmask = GetBitMask(hash);

                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    return Branches[branchIndex].TryGet(equalityComparer, hash >> 5, level + BitWidth, key, out value);
                }

                if (TryGetValueInternal(bitmask, equalityComparer, key, out value))
                {
                    return true;
                }

                // hash collision if this is max
                return GetCollisionOrNone(equalityComparer, key, level, out value);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            private bool TryGetValueInternal(uint bitmask, IEqualityComparer<TKey> equalityComparer, TKey key, out TValue value)
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
            private bool GetCollisionOrNone(IEqualityComparer<TKey> equalityComparer, TKey key, int level, out TValue value)
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
        }
    }
}
