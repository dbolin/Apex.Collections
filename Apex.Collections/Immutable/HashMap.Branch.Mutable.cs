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
            public Branch SetMutate(IEqualityComparer<TKey> equalityComparer,
                int hash, int level, TKey key, TValue value, Builder.State builderState, out bool added, out bool mutated)
            {
                uint bitmask = GetBitMask(hash);
                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = PopCount(BitMaskBranches & (bitmask - 1));
                    var branch = Branches[branchIndex];
                    var newBranch = branch.SetMutate(equalityComparer,
                        hash >> 5, level + BitWidth, key, value, builderState, out added, out var newBranchMutated);

                    if(builderState.IsFrozen(OwnerToken))
                    {
                        mutated = false;
                        return new Branch(builderState.OwnerToken, BitMaskValues, BitMaskBranches, Values,
                            SetItem(Branches, branchIndex, newBranch));
                    }

                    mutated = true;
                    if(!newBranchMutated)
                    {
                        Branches[branchIndex] = newBranch;
                    }

                    return this;
                }

                var valueIndex = PopCount(BitMaskValues & (bitmask - 1));
                if ((BitMaskValues & bitmask) != 0)
                {
                    var node = Values[valueIndex];

                    if (equalityComparer.Equals(node.Key, key))
                    {
                        added = false;
                        if(builderState.IsFrozen(OwnerToken))
                        {
                            mutated = false;
                            return new Branch(builderState.OwnerToken, BitMaskValues, BitMaskBranches,
                                SetItem(Values, valueIndex, new ValueNode(key, value)), Branches);
                        }

                        mutated = true;
                        Values[valueIndex] = new ValueNode(key, value);
                        return this;
                    }

                    var branchIndex = PopCount(BitMaskBranches & (bitmask - 1));
                    var branch = CreateFrom(equalityComparer, node, level + BitWidth, hash >> 5, key, value, builderState.OwnerToken);
                    added = true;
                    
                    if(builderState.IsFrozen(OwnerToken))
                    {
                        mutated = false;
                        return new Branch(builderState.OwnerToken, BitMaskValues & (~bitmask), BitMaskBranches | bitmask,
                            RemoveAt(Values, valueIndex), Insert(Branches, branchIndex, branch));
                    }

                    mutated = true;
                    BitMaskValues &= ~bitmask;
                    BitMaskBranches |= bitmask;
                    Values = RemoveAt(Values, valueIndex);
                    Branches = Insert(Branches, branchIndex, branch);
                    return this;
                }

                added = true;
                if(builderState.IsFrozen(OwnerToken))
                {
                    mutated = false;
                    return new Branch(builderState.OwnerToken, BitMaskValues | bitmask, BitMaskBranches,
                        Insert(Values, valueIndex, new ValueNode(key, value)), Branches);
                }

                mutated = true;
                BitMaskValues |= bitmask;
                Values = Insert(Values, valueIndex, new ValueNode(key, value));
                return this;
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public Branch RemoveMutate(IEqualityComparer<TKey> equalityComparer,
                int hash, int level, TKey key, Builder.State builderState, out bool removed, out bool mutated)
            {
                uint bitmask = GetBitMask(hash);
                if ((BitMaskBranches & bitmask) != 0)
                {
                    var branchIndex = PopCount(BitMaskBranches & (bitmask - 1));
                    var newBranch = Branches[branchIndex].RemoveMutate(equalityComparer,
                        hash >> 5, level + BitWidth, key, builderState, out removed, out var newBranchMutated);
                    if (newBranch.Branches.Length == 0 && newBranch.Values.Length == 0)
                    {
                        if(builderState.IsFrozen(OwnerToken))
                        {
                            mutated = false;
                            return new Branch(builderState.OwnerToken, BitMaskValues, BitMaskBranches & (~bitmask), Values,
                                RemoveAt(Branches, branchIndex));
                        }

                        mutated = true;
                        BitMaskBranches &= ~bitmask;
                        Branches = RemoveAt(Branches, branchIndex);
                        return this;
                    }

                    if(!removed)
                    {
                        mutated = false;
                        return this;
                    }

                    if (builderState.IsFrozen(OwnerToken))
                    {
                        mutated = false;
                        return new Branch(builderState.OwnerToken, BitMaskValues, BitMaskBranches, Values,
                            SetItem(Branches, branchIndex, newBranch));
                    }

                    mutated = true;
                    if(!newBranchMutated)
                    {
                        Branches[branchIndex] = newBranch;
                    }
                    return this;
                }

                if ((BitMaskValues & bitmask) != 0)
                {
                    var valueIndex = PopCount(BitMaskValues & (bitmask - 1));
                    var node = Values[valueIndex];
                    if (equalityComparer.Equals(node.Key, key))
                    {
                        removed = true;

                        if (builderState.IsFrozen(OwnerToken))
                        {
                            mutated = false;
                            return new Branch(builderState.OwnerToken, BitMaskValues & (~bitmask), BitMaskBranches,
                                RemoveAt(Values, valueIndex), Branches);
                        }

                        mutated = true;
                        BitMaskValues &= ~bitmask;
                        Values = RemoveAt(Values, valueIndex);
                        return this;
                    }
                }

                return RemoveCollisionOrNone(equalityComparer, level, key, builderState, out removed, out mutated);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            private Branch RemoveCollisionOrNone(IEqualityComparer<TKey> equalityComparer,
                int level, TKey key, Builder.State builderState, out bool removed, out bool mutated)
            {
                // hash collision
                if (level >= MaxLevel)
                {
                    for (int i = 0; i < Values.Length; ++i)
                    {
                        if (equalityComparer.Equals(Values[i].Key, key))
                        {
                            removed = true;

                            if(builderState.IsFrozen(OwnerToken))
                            {
                                mutated = false;
                                return new Branch(builderState.OwnerToken, 0, 0, RemoveAt(Values, i), Array.Empty<Branch>());
                            }

                            mutated = true;
                            Values = RemoveAt(Values, i);
                            return this;
                        }
                    }
                }

                removed = false;
                mutated = false;
                return this;
            }
        }
    }
}
