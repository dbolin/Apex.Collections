using System.Collections.Generic;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<TKey, TValue>
    {
        public sealed class Builder
        {
            private HashMap<TKey, TValue> _hashMap;

            internal Builder(HashMap<TKey, TValue> hashMap)
            {
                _hashMap = new HashMap<TKey, TValue>(hashMap._root, hashMap.Count);
            }

            public void SetItem(TKey key, TValue value)
            {
                _hashMap.SetItemMutate(key, value);
            }

            public void SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
            {
                foreach(var item in items)
                {
                    _hashMap.SetItemMutate(item.Key, item.Value);
                }
            }

            public void Remove(TKey key)
            {
                _hashMap.RemoveMutate(key);
            }

            public void RemoveRange(IEnumerable<TKey> keys)
            {
                foreach (var key in keys)
                {
                    _hashMap.RemoveMutate(key);
                }
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                return _hashMap.TryGetValue(key, out value);
            }

            public HashMap<TKey, TValue> ToImmutable()
            {
                _hashMap.Freeze();
                return _hashMap;
            }
        }

        private void SetItemMutate(TKey key, TValue value)
        {
            var hash = _equalityComparer.GetHashCode(key);
            var newRoot = _root.SetMutate(_equalityComparer, hash, 0, key, value, out var added, out var mutated);
            if(!mutated)
            {
                _root = newRoot;
            }

            if(added)
            {
                Count++;
            }
        }

        private void RemoveMutate(TKey key)
        {
            var hash = _equalityComparer.GetHashCode(key);
            var newRoot = _root.RemoveMutate(_equalityComparer, hash, 0, key, out var removed, out var mutated);
            if (!mutated)
            {
                _root = newRoot;
            }

            if (removed)
            {
                Count--;
            }
        }

        private void Freeze()
        {
            if (!_root.Frozen)
            {
                _root.Freeze();
            }
        }
    }
}
