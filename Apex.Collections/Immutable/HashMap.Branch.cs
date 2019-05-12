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
            private const int MaxLevel = 32 / BitWidth;

            public static readonly Branch Empty = new Branch(0, ImmutableArray<Node>.Empty);

            public uint BitMask { get; }
            public ImmutableArray<Node> Nodes { get; }

            public Branch(uint bitMask, ImmutableArray<Node> nodes)
            {
                BitMask = bitMask;
                Nodes = nodes;
            }

            public Branch Set(IEqualityComparer<K> equalityComparer, int hash, int level, K key, V value, out bool added)
            {
                uint bitmask = GetBitMask(hash, level);
                var index = (int)Popcnt.PopCount(BitMask & (bitmask - 1));

                // Bit is already set
                if ((BitMask & bitmask) != 0)
                {
                    // collision
                    // 2 cases:
                    // Single Key and Value
                    // Pointer to Branch
                    var node = Nodes[index];
                    if (node.Branch != default)
                    {
                        var newBranch = node.Branch.Set(equalityComparer, hash, level + 1, key, value, out added);
                        return new Branch(BitMask, Nodes.SetItem(index, new Node(newBranch)));
                    }

                    if(node.FullHash == hash && equalityComparer.Equals(node.Key, key))
                    {
                        added = false;
                        return new Branch(BitMask, Nodes.SetItem(index, new Node(hash, key, value)));
                    }

                    added = true;
                    var branch = CreateFrom(node, level + 1, hash, key, value);
                    var newNode = new Node(branch);
                    return new Branch(BitMask, Nodes.SetItem(index, newNode));
                }

                added = true;
                return new Branch(BitMask | bitmask, Nodes.Insert(index, new Node(hash, key, value)));
            }

            public Branch Remove(IEqualityComparer<K> equalityComparer, int hash, int level, K key, out bool removed)
            {
                // hash collision
                if (level == MaxLevel)
                {
                    for (int i = 0; i < Nodes.Length; ++i)
                    {
                        if (equalityComparer.Equals(Nodes[i].Key, key))
                        {
                            removed = true;
                            return new Branch(BitMask, Nodes.RemoveAt(i));
                        }
                    }

                    removed = false;
                    return this;
                }

                uint bitmask = GetBitMask(hash, level);
                var index = (int)Popcnt.PopCount(BitMask & (bitmask - 1));

                if ((BitMask & bitmask) == 0)
                {
                    removed = false;
                    return this;
                }

                var node = Nodes[index];
                if (node.Branch != default)
                {
                    var newBranch = node.Branch.Remove(equalityComparer, hash, level + 1, key, out removed);
                    if(newBranch.Nodes.Length == 0)
                    {
                        return new Branch(BitMask & (~bitmask), Nodes.RemoveAt(index));
                    }

                    var newNode = new Node(newBranch);
                    return new Branch(BitMask, Nodes.SetItem(index, newNode));
                }

                if (node.FullHash == hash && equalityComparer.Equals(node.Key, key))
                {
                    removed = true;
                    return new Branch(BitMask & (~bitmask), Nodes.RemoveAt(index));
                }

                removed = false;
                return this;
            }

            public bool TryGet(IEqualityComparer<K> equalityComparer, int hash, int level, K key, out V value)
            {
                // hash collision
                if (level == MaxLevel)
                {
                    for(int i=0;i<Nodes.Length;++i)
                    {
                        if(equalityComparer.Equals(Nodes[i].Key, key))
                        {
                            value = Nodes[i].Value;
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }

                uint bitmask = GetBitMask(hash, level);

                if((BitMask & bitmask) == 0)
                {
                    value = default;
                    return false;
                }

                var index = (int)Popcnt.PopCount(BitMask & (bitmask - 1));
                var node = Nodes[index];
                if(node.Branch != default)
                {
                    return node.Branch.TryGet(equalityComparer, hash, level + 1, key, out value);
                }

                if(node.FullHash == hash && equalityComparer.Equals(node.Key, key))
                {
                    value = node.Value;
                    return true;
                }

                value = default;
                return false;
            }

            private static uint GetBitMask(int hash, int level)
            {
                int bitsFromHash = (hash >> (level * BitWidth)) & SubMask;
                uint bitmask = (uint)(1 << bitsFromHash);
                return bitmask;
            }

            private static Branch CreateFrom(Node node, int level, int hash, K key, V value)
            {
                var firstBitMask = GetBitMask(node.FullHash, level);
                var secondBitMask = GetBitMask(hash, level);

                if (firstBitMask == secondBitMask)
                {
                    // hash collision
                    if(level == MaxLevel)
                    {
                        return new Branch(0, ImmutableArray.Create(node, new Node(hash, key, value)));
                    }

                    return new Branch(firstBitMask, ImmutableArray.Create(new Node(CreateFrom(node, level + 1, hash, key, value))));
                }

                var resultBitBask = firstBitMask | secondBitMask;

                if(firstBitMask < secondBitMask)
                {
                    return new Branch(resultBitBask, ImmutableArray.Create(node, new Node(hash, key, value)));
                }

                return new Branch(resultBitBask, ImmutableArray.Create(new Node(hash, key, value), node));
            }
        }
    }
}
