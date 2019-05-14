namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<TKey, TValue>
    {
        internal struct ValueNode
        {
            public ValueNode(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }

            public TKey Key { get; }
            public TValue Value { get; }
        }
    }
}
