using System;
using System.Collections.Generic;

namespace Apex.Collections
{
    public sealed class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> items;

        public PriorityQueue(int capacity = 4)
        {
            items = new List<T>(capacity);
        }

        public void Enqueue(T val)
        {
            int i = items.Count;
            items.Add(val);
            while (i > 0 && items[(i - 1) / 2].CompareTo(val) > 0)
            {
                items[i] = items[(i - 1) / 2];
                i = (i - 1) / 2;
            }

            if (i < items.Count)
            {
                items[i] = val;
            }
        }

        public T Dequeue()
        {
            T res = items[0];
            T tmp = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);

            if (items.Count > 0)
            {
                int i = 0;
                while (i < items.Count / 2)
                {
                    int j = 2 * i + 1;
                    if ((j < items.Count - 1) && (items[j].CompareTo(items[j + 1]) > 0))
                    {
                        ++j;
                    }
                    if (items[j].CompareTo(tmp) > 0)
                    {
                        break;
                    }

                    items[i] = items[j];
                    i = j;
                }

                items[i] = tmp;
            }

            return res;
        }

        public T Peek()
        {
            return items[0];
        }

        public bool IsEmpty => items.Count == 0;

        public void Clear()
        {
            items.Clear();
        }
    }
}
