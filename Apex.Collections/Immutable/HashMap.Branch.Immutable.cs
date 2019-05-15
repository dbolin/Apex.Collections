using System;
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
            public Branch Set(IEqualityComparer<TKey> equalityComparer, int hash, int level, TKey key, TValue value, bool overwrite, out OperationResult result)
            {
                uint bitmask = GetBitMask(hash);
                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    var branch = Branches[branchIndex];
                    var newBranch = branch.Set(equalityComparer, hash >> 5, level + BitWidth, key, value, overwrite, out result);
                    if(result == OperationResult.NoChange)
                    {
                        return this;
                    }

                    return new Branch(BitMaskValues, BitMaskBranches, Values, SetItem(Branches, branchIndex, newBranch));
                }

                var valueIndex = (int)Popcnt.PopCount(BitMaskValues & (bitmask - 1));
                if ((BitMaskValues & bitmask) != 0)
                {
                    var node = Values[valueIndex];

                    if (equalityComparer.Equals(node.Key, key))
                    {
                        if (!overwrite)
                        {
                            result = OperationResult.NoChange;
                            return this;
                        }

                        result = OperationResult.Replaced;
                        return new Branch(BitMaskValues, BitMaskBranches, SetItem(Values, valueIndex, new ValueNode(key, value)), Branches);
                    }

                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    var branch = CreateFrom(equalityComparer, node, level + BitWidth, hash >> 5, key, value, false);
                    result = OperationResult.Added;
                    return new Branch(BitMaskValues & (~bitmask), BitMaskBranches | bitmask, RemoveAt(Values, valueIndex), Insert(Branches, branchIndex, branch));
                }

                result = OperationResult.Added;
                return new Branch(BitMaskValues | bitmask, BitMaskBranches, Insert(Values, valueIndex, new ValueNode(key, value)), Branches);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public Branch Remove(IEqualityComparer<TKey> equalityComparer, int hash, int level, TKey key, out OperationResult result)
            {
                uint bitmask = GetBitMask(hash);
                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = (int)Popcnt.PopCount(BitMaskBranches & (bitmask - 1));
                    var newBranch = Branches[branchIndex].Remove(equalityComparer, hash >> 5, level + BitWidth, key, out result);
                    if(result == OperationResult.NoChange)
                    {
                        return this;
                    }

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
                        result = OperationResult.Removed;
                        return new Branch(BitMaskValues & (~bitmask), BitMaskBranches, RemoveAt(Values, valueIndex), Branches);
                    }
                }

                return RemoveCollisionOrNone(equalityComparer, level, key, out result);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            private Branch RemoveCollisionOrNone(IEqualityComparer<TKey> equalityComparer, int level, TKey key, out OperationResult result)
            {
                // hash collision
                if (level >= MaxLevel)
                {
                    for (int i = 0; i < Values.Length; ++i)
                    {
                        if (equalityComparer.Equals(Values[i].Key, key))
                        {
                            result = OperationResult.Removed;
                            return new Branch(0, 0, RemoveAt(Values, i), Array.Empty<Branch>());
                        }
                    }
                }

                result = OperationResult.NoChange;
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
