using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<TKey, TValue>
    {
        public sealed class Builder
        {
            internal class State
            {
                public BuilderToken OwnerToken;

                public State()
                {
                    OwnerToken = new BuilderToken();
                }

                public void Freeze()
                {
                    OwnerToken = new BuilderToken();
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool IsFrozen(BuilderToken owner)
                {
                    return !ReferenceEquals(OwnerToken, owner);
                }
            }

            private readonly HashMap<TKey, TValue> _hashMap;
            private readonly State _state;

            internal Builder(HashMap<TKey, TValue> hashMap)
            {
                _hashMap = new HashMap<TKey, TValue>(hashMap._equalityComparer, hashMap._root, hashMap.Count);
                _state = new State();
            }

            public void SetItem(TKey key, TValue value)
            {
                _hashMap.SetItemMutate(key, value, _state);
            }

            public void SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
            {
                foreach(var item in items)
                {
                    _hashMap.SetItemMutate(item.Key, item.Value, _state);
                }
            }

            public void Remove(TKey key)
            {
                _hashMap.RemoveMutate(key, _state);
            }

            public void RemoveRange(IEnumerable<TKey> keys)
            {
                foreach (var key in keys)
                {
                    _hashMap.RemoveMutate(key, _state);
                }
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                return _hashMap.TryGetValue(key, out value);
            }

            public HashMap<TKey, TValue> ToImmutable()
            {
                _state.Freeze();
                return _hashMap;
            }
        }

        private void SetItemMutate(TKey key, TValue value, Builder.State builderState)
        {
            var hash = _equalityComparer.GetHashCode(key);
            var newRoot = _root.SetMutate(_equalityComparer, hash, 0, key, value, builderState, out var added, out var mutated);
            if(!mutated)
            {
                _root = newRoot;
            }

            if(added)
            {
                Count++;
            }
        }

        private void RemoveMutate(TKey key, Builder.State builderState)
        {
            var hash = _equalityComparer.GetHashCode(key);
            var newRoot = _root.RemoveMutate(_equalityComparer,
                hash, 0, key, builderState, out var removed, out var mutated);
            if (!mutated)
            {
                _root = newRoot;
            }

            if (removed)
            {
                Count--;
            }
        }
    }
}
