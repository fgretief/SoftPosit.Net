// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit64_t = Posit64;

    internal static partial class Native
    {
        public static posit64_t i32_to_p64(in int value)
        {
            if (value == int.MinValue)
            {
                return new Posit64(0x8480_0000_0000_0000ul); // -2147483648
            }

            var sign = value < 0;
            int iA = sign ? -value : value;

            Console.WriteLine(" sign={0}, value={1}  {2:X8}", sign ? '-' : '+', iA, iA);

            ulong uiZ;
            if (iA < 2)
            {
                uiZ = (ulong)iA << 62; // 0, 1, -1
            }
            else
            {
                Debug.Assert(iA > 0, "must be positive");

                // output environment
                const int psZ = 64;
                const int esZ = 3;

                // The maximum value that can reach this point
                // is 2147483647, which can fit in 31 bits.
                const int nbits = 31;
                Debug.Assert(nbits > 0);

                var log2 = nbits - 1;
                var fracA = (ulong)iA;
                var mask = 1ul << log2;
                Debug.Assert((fracA & ~((1u << nbits) - 1)) == 0, "Upper bits must be clear");
                Debug.Assert(fracA != 0, "Can cause an infinite loop");
                while ((fracA & mask) == 0)
                {
                    log2--;
                    fracA <<= 1;
                }

                fracA ^= mask; // clear silent one

                const ulong signMaskZ = 1ul << (psZ - 1);

                var k = (sbyte)(log2 >> esZ);
                var regimeZ = (signMaskZ - 1) ^ (((signMaskZ >> 1) - 1) >> k);
                var expZ = (ulong)(log2 % (1 << esZ)) << ((psZ - 1) - (k + 2) - esZ);

                var shift = (nbits - psZ) + (k + 2) + esZ;
                var fracZ = shift < 0 ? fracA << -shift : fracA >> shift;

                uiZ = regimeZ | expZ | fracZ;

                var bitNPlusOneShift = psZ - (k + 2) - esZ;
                if (!(bitNPlusOneShift < nbits))
                {
                    //Console.WriteLine("!! No rounding needed. Enough space to fit input");
                }
                else
                {
                    var bitNPlusOneMask = mask >> bitNPlusOneShift;
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
                }

                Debug.Assert(uiZ == ConvertUIntToPositImpl(iA, 31, 64, 3));
            }

            return new Posit64(sign, uiZ);
        }
    }
}