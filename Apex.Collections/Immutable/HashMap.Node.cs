namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<K, V>
    {
        internal struct ValueNode
        {
            public ValueNode(K key, V value)
            {
                Key = key;
                Value = value;
            }

            public K Key { get; }
            public V Value { get; }
        }
    }
}
