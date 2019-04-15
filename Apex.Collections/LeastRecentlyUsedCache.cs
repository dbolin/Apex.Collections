using System;
using System.Collections;
using System.Collections.Generic;

namespace Apex.Collections
{
    public sealed class LeastRecentlyUsedCache<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        private readonly Dictionary<K, LinkedListNode<KeyValuePair<K, V>>> _lookup;
        private readonly LinkedList<KeyValuePair<K, V>> _list;

        public int Capacity { get; }

        public LeastRecentlyUsedCache(int capacity, IEqualityComparer<K> equalityComparer = null)
        {
            Capacity = capacity;
            _lookup = new Dictionary<K, LinkedListNode<KeyValuePair<K, V>>>(capacity, equalityComparer ?? EqualityComparer<K>.Default);
            _list = new LinkedList<KeyValuePair<K, V>>();
        }

        public V GetOrAdd(K key, Func<K, V> valueCreator)
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

        private LinkedListNode<KeyValuePair<K, V>> RemoveFirst()
        {
            var lru = _list.First;
            _lookup.Remove(lru.Value.Key);
            _list.RemoveFirst();
            return lru;
        }

        private void Add(K key, V value)
        {
            var newNode = _list.AddLast(new KeyValuePair<K, V>(key, value));
            _lookup.Add(key, newNode);
        }

        private void AddFromExistingNode(K key, V result, LinkedListNode<KeyValuePair<K, V>> node)
        {
            node.Value = new KeyValuePair<K, V>(key, result);
            _list.AddLast(node);
            _lookup.Add(key, node);
        }

        private bool Get(K key, out V value)
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

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    }
}
