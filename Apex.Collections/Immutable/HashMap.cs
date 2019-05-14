using System.Collections;
using System.Collections.Generic;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public static readonly HashMap<TKey, TValue> Empty = new HashMap<TKey, TValue>(Branch.Empty, 0);

        private readonly Branch _root;
        private readonly IEqualityComparer<TKey> _equalityComparer = EqualityComparer<TKey>.Default;
        public int Count { get; }

        internal HashMap(Branch root, int count)
        {
            _root = root;
            Count = count;
        }

        public HashMap<TKey, TValue> SetItem(TKey key, TValue value)
        {
            var hash = _equalityComparer.GetHashCode(key);
            return new HashMap<TKey, TValue>(_root.Set(_equalityComparer, hash, 0, key, value, out bool added), added ? Count + 1 : Count);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var hash = _equalityComparer.GetHashCode(key);
            return _root.TryGet(_equalityComparer, hash, 0, key, out value);
        }

        public HashMap<TKey, TValue> Remove(TKey key)
        {
            var hash = _equalityComparer.GetHashCode(key);
            return new HashMap<TKey, TValue>(_root.Remove(_equalityComparer, hash, 0, key, out bool removed), removed ? Count - 1 : Count);
        }

        public Enumerator GetEnumerator() => new Enumerator(_root);
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
