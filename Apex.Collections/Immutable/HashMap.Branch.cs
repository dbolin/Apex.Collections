using System;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<TKey, TValue>
    {
        internal sealed partial class Branch
        {
            internal const int BitWidth = 5;
            internal const int SubMask = (1 << BitWidth) - 1;
            internal const int MaxLevel = 32;

            public static readonly Branch Empty = new Branch(0, 0, Array.Empty<ValueNode>(), Array.Empty<Branch>());

            public enum OperationResult
            {
                NoChange,
                Added,
                Replaced,
                Removed
            }

            public bool Frozen { get; private set; }
            public uint BitMaskValues { get; private set; }
            public uint BitMaskBranches { get; private set; }
            public ValueNode[] Values { get; private set; }
            public Branch[] Branches { get; private set; }

            public Branch(bool frozen, uint bitMaskValues, uint bitMaskBranches, ValueNode[] nodes, Branch[] branches)
            {
                Frozen = frozen;
                BitMaskValues = bitMaskValues;
                BitMaskBranches = bitMaskBranches;
                Values = nodes;
                Branches = branches;
            }

            public Branch(uint bitMaskValues, uint bitMaskBranches, ValueNode[] nodes, Branch[] branches)
                : this(true, bitMaskValues, bitMaskBranches, nodes, branches)
            {
            }
        }
    }
}
