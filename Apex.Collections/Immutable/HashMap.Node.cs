namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<K, V>
    {
        internal struct ValueNode
        {
            public ValueNode(int hash, K key, V value)
            {
                FullHash = hash;
                Key = key;
                Value = value;
            }

            public int FullHash { get; }
            public K Key { get; }
            public V Value { get; }
        }
    }
}
