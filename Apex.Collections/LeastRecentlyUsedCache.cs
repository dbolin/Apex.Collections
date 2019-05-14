using System;
using System.Collections;
using System.Collections.Generic;

namespace Apex.Collections
{
    public sealed class LeastRecentlyUsedCache<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> _lookup;
        private readonly LinkedList<KeyValuePair<TKey, TValue>> _list;

        public int Capacity { get; }

        public LeastRecentlyUsedCache(int capacity, IEqualityComparer<TKey> equalityComparer = null)
        {
            Capacity = capacity;
            _lookup = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>(capacity, equalityComparer ?? EqualityComparer<TKey>.Default);
            _list = new LinkedList<KeyValuePair<TKey, TValue>>();
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueCreator)
        {
            if (Get(key, out var result))
            {
                return result;
            }

            if (_list.Count >= Capacity)
            {
                var node = RemoveFirst();
                result = valueCreator(key);
                AddFromExistingNode(key, result, node);

                return result;
            }

            result = valueCreator(key);
            Add(key, result);

            return result;
        }

        private LinkedListNode<KeyValuePair<TKey, TValue>> RemoveFirst()
        {
            var lru = _list.First;
            _lookup.Remove(lru.Value.Key);
            _list.RemoveFirst();
            return lru;
        }

        private void Add(TKey key, TValue value)
        {
            var newNode = _list.AddLast(new KeyValuePair<TKey, TValue>(key, value));
            _lookup.Add(key, newNode);
        }

        private void AddFromExistingNode(TKey key, TValue result, LinkedListNode<KeyValuePair<TKey, TValue>> node)
        {
            node.Value = new KeyValuePair<TKey, TValue>(key, result);
            _list.AddLast(node);
            _lookup.Add(key, node);
        }

        private bool Get(TKey key, out TValue value)
        {
            if (_lookup.TryGetValue(key, out var node))
            {
                _list.Remove(node);
                _list.AddLast(node);
                value = node.Value.Value;
                return true;
            }

            value = default;
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    }
}
