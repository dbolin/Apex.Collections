using Microsoft.Extensions.ObjectPool;
using System.Collections;
using System.Collections.Generic;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<K, V>
    {
        internal static readonly ObjectPool<Stack<EnumeratorState>> _stackPool
            = new DefaultObjectPool<Stack<EnumeratorState>>(new DefaultPooledObjectPolicy<Stack<EnumeratorState>>());

        internal struct EnumeratorState
        {
            public Branch Branch;
            public bool OnValues;
            public int Index;

            public EnumeratorState(Branch branch, bool onValues, int index)
            {
                Branch = branch;
                OnValues = onValues;
                Index = index;
            }
        }

        public struct Enumerator : IEnumerator<KeyValuePair<K, V>>
        {
            private readonly Branch _start;
            private Stack<EnumeratorState> _stack;
            private EnumeratorState _current;

            internal Enumerator(Branch start)
            {
                _start = start;
                _stack = _stackPool.Get();
                _current = new EnumeratorState(start, true, -1);
            }

            public KeyValuePair<K, V> Current {
                get {
                    var branch = _current.Branch;
                    var node = branch.Values[_current.Index];
                    return new KeyValuePair<K, V>(node.Key, node.Value);
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _stackPool.Return(_stack);
                _stack = null;
                _current = default;
            }
            public bool MoveNext()
            {
                var index = _current.Index + 1;
                if (_current.OnValues)
                {
                    if (index >= _current.Branch.Values.Length)
                    {
                        index = 0;
                        _current.OnValues = false;
                    }
                    else
                    {
                        _current.Index = index;
                        return true;
                    }
                }
                _current.Index = index;

                if (index >= _current.Branch.Branches.Length)
                {
                    if(_stack.Count == 0)
                    {
                        return false;
                    }
                    _current = _stack.Pop();
                    return MoveNext();
                }

                var branch = _current.Branch.Branches[index];
                _stack.Push(_current);
                _current = new EnumeratorState(branch, true, -1);
                return MoveNext();
            }
            public void Reset()
            {
                _stack.Clear();
                _current = new EnumeratorState(_start, true, -1);
            }
        }
    }
}
