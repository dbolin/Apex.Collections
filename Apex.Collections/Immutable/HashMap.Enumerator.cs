using System;
using System.Collections;
using System.Collections.Generic;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<TKey, TValue>
    {
        internal struct EnumeratorState
        {
            public Branch Branch;
            public int Index;

            public EnumeratorState(Branch branch, int index)
            {
                Branch = branch;
                Index = index;
            }
        }

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private EnumeratorState _currentState;
            private int _currentSlot;
            private bool _onValues;
            private EnumeratorState _slot0;
            private EnumeratorState _slot1;
            private EnumeratorState _slot2;
            private EnumeratorState _slot3;
            private EnumeratorState _slot4;
            private EnumeratorState _slot5;
            private EnumeratorState _slot6;
            private readonly Branch _start;

            internal Enumerator(Branch start)
            {
                _start = start;
                _slot0 = default;
                _slot1 = default;
                _slot2 = default;
                _slot3 = default;
                _slot4 = default;
                _slot5 = default;
                _slot6 = default;
                _currentState = new EnumeratorState(start, -1);
                _onValues = true;
                _currentSlot = 0;
            }

            internal static ref EnumeratorState CurrentStackState(ref Enumerator e)
            {
                switch(e._currentSlot)
                {
                    case 0:
                        return ref e._slot0;
                    case 1:
                        return ref e._slot1;
                    case 2:
                        return ref e._slot2;
                    case 3:
                        return ref e._slot3;
                    case 4:
                        return ref e._slot4;
                    case 5:
                        return ref e._slot5;
                    case 6:
                        return ref e._slot6;
                    default:
                        ThrowOutOfRange();
                        return ref e._slot6;
                }
            }

            private static void ThrowOutOfRange()
            {
                throw new InvalidOperationException();
            }

            public KeyValuePair<TKey, TValue> Current {
                get
                {
                    var branch = _currentState.Branch;
                    var node = branch.Values[_currentState.Index];
                    return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                var index = _currentState.Index + 1;
                if (_onValues)
                {
                    if (index >= _currentState.Branch.Values.Length)
                    {
                        index = 0;
                        _onValues = false;
                    }
                    else
                    {
                        _currentState.Index = index;
                        return true;
                    }
                }
                _currentState.Index = index;

                if (index >= _currentState.Branch.Branches.Length)
                {
                    if(_currentSlot == 0)
                    {
                        return false;
                    }
                    _currentSlot--;
                    _currentState = CurrentStackState(ref this);
                    return MoveNext();
                }

                var branch = _currentState.Branch.Branches[index];
                ref var nextState = ref CurrentStackState(ref this);
                nextState = _currentState;
                _currentState = new EnumeratorState(branch, -1);
                _onValues = true;
                _currentSlot++;
                return MoveNext();
            }

            public void Reset()
            {
                _currentSlot = 0;
                _currentState = new EnumeratorState(_start, -1);
                _onValues = true;
            }
        }
    }
}
