// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit64_t = Posit64;

    internal static partial class Native
    {
        public static posit64_t ui64_to_p64(in ulong value)
        {
            var uiA = value;

            ulong uiZ;
            if (uiA < 2)
            {
                uiZ = uiA << (64-2); // 0, 1
            }
            else
            {
                uiZ = convertUI64ToP64(uiA);
                Debug.Assert(uiZ == ConvertUIntToPositImpl(uiA, 64, 64, 3));
            }

            return new Posit64(uiZ);
        }

        private static ulong convertUI64ToP64(ulong uiA)
        {
            Debug.Assert(uiA > 0, "must be positive");

            // output environment
            const int psZ = 64;
            const int esZ = 3;

            // the maximum number of bits in uiA
            const int nbits = 64;

            var log2 = nbits - 1;
            var fracA = uiA;
            var mask = 1ul << log2;
            Debug.Assert(fracA != 0, "Can cause an infinite loop");
            while ((fracA & mask) == 0)
            {
                log2--;
                fracA <<= 1;
            }
            fracA ^= mask; // clear silent one

            const ulong signMask = 1ul << (psZ - 1);

            var k = (sbyte)(log2 >> esZ);
            var expZ = ((ulong)log2 % (1 << esZ)) << ((psZ - 1) - (k + 2) - esZ);
            var fracZ = fracA >> ((nbits - psZ) + (k + 2) + esZ);
            var regimeZ = (signMask - 1) ^ (((signMask >> 1) - 1) >> k);

            var uiZ = regimeZ | expZ | fracZ;

            var bitNPlusOneMask = mask >> (psZ - (k + 2) - esZ);
            if ((bitNPlusOneMask & fracA) != 0)
            {
                var bitLastMask = bitNPlusOneMask << 1;
                var bitsMoreMask = bitNPlusOneMask - 1;
                // logic for round to nearest, tie to even
                if (((bitLastMask | bitsMoreMask) & fracA) != 0)
                {
                    uiZ++;
                }
            }

            return uiZ;
        }
    }
}