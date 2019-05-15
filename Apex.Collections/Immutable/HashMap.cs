using System.Collections;
using System.Collections.Generic;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public static readonly HashMap<TKey, TValue> Empty = new HashMap<TKey, TValue>(EqualityComparer<TKey>.Default, Branch.Empty, 0);

        private Branch _root;
        private IEqualityComparer<TKey> _equalityComparer;
        public int Count { get; internal set; }

        internal HashMap(IEqualityComparer<TKey> equalityComparer ,Branch root, int count)
        {
            _equalityComparer = equalityComparer;
            _root = root;
            Count = count;
        }

        public HashMap<TKey, TValue> WithComparer(IEqualityComparer<TKey> keyComparer)
        {
            if(keyComparer == _equalityComparer)
            {
                return this;
            }

            return new HashMap<TKey, TValue>(keyComparer, Branch.Empty, 0).SetItems(this);
        }

        public HashMap<TKey, TValue> SetItem(TKey key, TValue value)
        {
            var hash = _equalityComparer.GetHashCode(key);
            var newRoot = _root.Set(_equalityComparer, hash, 0, key, value, true, out var result);
            if(result == Branch.OperationResult.NoChange)
            {
                return this;
            }

            return new HashMap<TKey, TValue>(_equalityComparer, newRoot, result == Branch.OperationResult.Added ? Count + 1 : Count);
        }

        public HashMap<TKey, TValue> TryAdd(TKey key, TValue value, out bool added)
        {
            var hash = _equalityComparer.GetHashCode(key);
            var newRoot = _root.Set(_equalityComparer, hash, 0, key, value, false, out var result);
            added = result == Branch.OperationResult.Added;
            if (result == Branch.OperationResult.NoChange)
            {
                return this;
            }

            return new HashMap<TKey, TValue>(_equalityComparer, newRoot, added ? Count + 1 : Count);
        }

        public HashMap<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            var builder = ToBuilder();
            foreach (var item in items)
            {
                builder.SetItem(item.Key, item.Value);
            }

            return builder.ToImmutable();
        }

        public HashMap<TKey, TValue> RemoveRange(IEnumerable<TKey> keys)
        {
            var builder = ToBuilder();
            foreach (var key in keys)
            {
                builder.Remove(key);
            }

            return builder.ToImmutable();
        }

        public Builder ToBuilder()
        {
            return new Builder(this);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var hash = _equalityComparer.GetHashCode(key);
            return _root.TryGet(_equalityComparer, hash, 0, key, out value);
        }

        public bool ContainsKey(TKey key)
        {
            var hash = _equalityComparer.GetHashCode(key);
            return _root.TryGet(_equalityComparer, hash, 0, key, out _);
        }

        public HashMap<TKey, TValue> Remove(TKey key)
        {
            var hash = _equalityComparer.GetHashCode(key);
            var newRoot = _root.Remove(_equalityComparer, hash, 0, key, out var result);
            if(result == Branch.OperationResult.NoChange)
            {
                return this;
            }

            return new HashMap<TKey, TValue>(_equalityComparer, newRoot, result == Branch.OperationResult.Removed ? Count - 1 : Count);
        }

        public HashMap<TKey, TValue> Clear()
        {
            if(Count == 0)
            {
                return this;
            }

            return new HashMap<TKey, TValue>(_equalityComparer, Branch.Empty, 0);
        }

        public Enumerator GetEnumerator() => new Enumerator(_root);
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
