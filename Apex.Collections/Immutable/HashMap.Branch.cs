using System;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<K, V>
    {
        internal sealed partial class Branch
        {
            private const int BitWidth = 5;
            private const int SubMask = (1 << BitWidth) - 1;
            private const int MaxLevel = 32;

            public static readonly Branch Empty = new Branch(0, 0, Array.Empty<ValueNode>(), Array.Empty<Branch>());

            public uint BitMaskValues { get; }
            public uint BitMaskBranches { get; }
            public ValueNode[] Values { get; }
            public Branch[] Branches { get; }

            public Branch(uint bitMaskValues, uint bitMaskBranches, ValueNode[] nodes, Branch[] branches)
            {
                BitMaskValues = bitMaskValues;
                BitMaskBranches = bitMaskBranches;
                Values = nodes;
                Branches = branches;
            }
        }
    }
}
