using System;
using System.Runtime.CompilerServices;

namespace Apex.Collections.Immutable
{
    public sealed partial class HashMap<TKey, TValue>
    {
        internal sealed partial class Branch
        {
            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            private static uint GetBitMask(int hash)
            {
                int bitsFromHash = hash & SubMask;
                uint bitmask = 1U << bitsFromHash;
                return bitmask;
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            private static unsafe T[] Insert<T>(T[] array, int index, T item)
            {
                if (array.Length == 0)
                {
                    return new T[] { item };
                }

                T[] tmp = new T[array.Length + 1];
                tmp[index] = item;

                if (index != 0)
                {
                    Unsafe.CopyBlock(Unsafe.AsPointer(ref tmp[0]), Unsafe.AsPointer(ref array[0]), (uint)(Unsafe.SizeOf<T>() * index));
                }
                if (index != array.Length)
                {
                    Unsafe.CopyBlock(Unsafe.AsPointer(ref tmp[index + 1]), Unsafe.AsPointer(ref array[index]), (uint)(Unsafe.SizeOf<T>() * (array.Length - index)));
                }

                return tmp;
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            private static unsafe T[] RemoveAt<T>(T[] array, int index)
            {
                if (array.Length == 1)
                {
                    return Array.Empty<T>();
                }

                T[] tmp = new T[array.Length - 1];
                Unsafe.CopyBlock(Unsafe.AsPointer(ref tmp[0]), Unsafe.AsPointer(ref array[0]), (uint)(Unsafe.SizeOf<T>() * index));
                if (index < tmp.Length)
                {
                    Unsafe.CopyBlock(Unsafe.AsPointer(ref tmp[index]), Unsafe.AsPointer(ref array[index + 1]), (uint)(Unsafe.SizeOf<T>() * (array.Length - index - 1)));
                }

                return tmp;
            }
        }
    }
}
