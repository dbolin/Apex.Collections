namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<TKey, TValue>
    {
        internal enum OperationType
        {
            Remove,
            Set
        }

        internal struct Operation
        {
            public OperationType Type { get; }
            public TKey Key { get; }
            public TValue Value { get; }

            public Operation(OperationType type, TKey key, TValue value)
            {
                Type = type;
                Key = key;
                Value = value;
            }
        }
    }
}
