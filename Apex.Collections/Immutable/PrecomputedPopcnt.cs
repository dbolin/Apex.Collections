namespace Apex.Collections.Immutable
{
    internal static class PrecomputedPopcnt
    {
        internal unsafe struct BitsWrapper
        {
            internal fixed ushort wordBits[65536];
        }

        internal static readonly BitsWrapper Bits = InitWordBits();

        private static unsafe BitsWrapper InitWordBits()
        {
            var result = new BitsWrapper();

            uint i;
            ushort x;
            int count;
            for (i = 0; i <= 0xFFFF; i++)
            {
                x = (ushort)i;
                for (count = 0; x != 0; count++)
                    x &= (ushort) (x - 1);
                result.wordBits[i] = (ushort)count;
            }

            return result;
        }
    }
}
