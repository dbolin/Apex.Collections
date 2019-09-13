namespace Apex.Collections.Immutable
{
    internal sealed class ArrayPooler<T>
    {
        private struct Arrays
        {
            public T[] A;
            public T[] B;

            public T[] Get()
            {
                if(A != null)
                {
                    var r = A;
                    A = null;
                    return r;
                }
                if (B != null)
                {
                    var r = B;
                    B = null;
                    return r;
                }

                return null;
            }

            public void Return(T[] array)
            {
                if(A == null)
                {
                    A = array;
                    return;
                }
                if(B == null)
                {
                    B = array;
                    return;
                }
            }
        }

        private Arrays[] _stack;
        private int _count;

        public T[] Get(int length)
        {
            if(_count < 32)
            {
                _count++;
                return new T[length];
            }
            else if (_stack == null)
            {
                _stack = new Arrays[33];
            }

            return _stack[length].Get() ?? new T[length];
        }

        public void Return(T[] array)
        {
            if(_stack == null)
            {
                return;
            }

            _stack[array.Length].Return(array);
        }
    }
}
