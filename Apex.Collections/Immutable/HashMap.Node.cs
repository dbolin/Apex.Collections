namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<K, V>
    {
        internal struct Node
        {
            public Node(Branch branch)
            {
                FullHash = default;
                Key = default;
                Value = default;
                Branch = branch;
            }

            public Node(int hash, K key, V value)
            {
                FullHash = hash;
                Key = key;
                Value = value;
                Branch = default;
            }

            public int FullHash { get; }
            public K Key { get; }
            public V Value { get; }
            public Branch Branch { get; }
        }
    }
}
