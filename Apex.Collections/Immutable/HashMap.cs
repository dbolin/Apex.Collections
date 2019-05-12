using System.Collections;
using System.Collections.Generic;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        public static readonly HashMap<K, V> Empty = new HashMap<K, V>(Branch.Empty, 0);

        private readonly Branch _root;
        private readonly IEqualityComparer<K> _equalityComparer = EqualityComparer<K>.Default;
        public int Count { get; }

        internal HashMap(Branch root, int count)
        {
            _root = root;
            Count = count;
        }

        public HashMap<K, V> SetItem(K key, V value)
        {
            var hash = _equalityComparer.GetHashCode(key);
            return new HashMap<K, V>(_root.Set(_equalityComparer, hash, 0, key, value, out bool added), added ? Count + 1 : Count);
        }

        public bool TryGetValue(K key, out V value)
        {
            var hash = _equalityComparer.GetHashCode(key);
            return _root.TryGet(_equalityComparer, hash, 0, key, out value);
        }

        public HashMap<K, V> Remove(K key)
        {
            var hash = _equalityComparer.GetHashCode(key);
            return new HashMap<K, V>(_root.Remove(_equalityComparer, hash, 0, key, out bool removed), removed ? Count - 1 : Count);
        }

        public Enumerator GetEnumerator() => new Enumerator(_root);
        IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
