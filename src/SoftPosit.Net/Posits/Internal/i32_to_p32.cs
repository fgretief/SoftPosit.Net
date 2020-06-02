// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit32_t = Posit32;

    internal static partial class Native
    {
        public static posit32_t i32_to_p32(in int value)
        {
            if (value < -2147483135)
            {   //-2147483648 to -2147483136 rounds to P32 value -2147483648
                return new Posit32(0x80500000);
            }

            var sign = value < 0;
            var iA = sign ? -value : value;

            uint uiZ;
            if (iA < 0x2)
            {
                uiZ = (uint)(iA << 30); // 0, 1, -1
            }
            else if (iA > 2147483135) //2147483136 to 2147483647 rounds to P32 value (2147483648)=> 0x7FB00000
            {
                uiZ = 0x7FB00000;
            }
            else
            {
                Debug.Assert(iA > 0, "must be positive");

                // output environment
                const int psZ = 32;
                const int esZ = 2;

                // The maximum value that can reach this point
                // is 2147483135, which can fit in 31 bits.
                const int nbits = 31;
                Debug.Assert(nbits > 0);

                var log2 = nbits - 1;
                var fracA = (uint)iA;
                var mask = 1u << log2;
                Debug.Assert((fracA & ~((1u << nbits) - 1)) == 0, "Upper bits must be clear");
                Debug.Assert(fracA != 0, "Can cause an infinite loop");
                while ((fracA & mask) == 0)
                {
                    log2--;
                    fracA <<= 1;
                }
                fracA ^= mask; // clear silent one

                const uint signMaskZ = 1u << (psZ - 1);

                var k = (sbyte)(log2 >> esZ);
                var regimeZ = (signMaskZ - 1) ^ (((signMaskZ >> 1) - 1) >> k);
                var expZ = ((uint)log2 % (1 << esZ)) << ((psZ - 1) - (k + 2) - esZ);

                var shift = (nbits - psZ) + (k + 2) + esZ;
                var fracZ = shift < 0 ? fracA << -shift : fracA >> shift;

                uiZ = regimeZ | expZ | fracZ;

                var bitNPlusOneShift = psZ - (k + 2) - esZ;
                var bitNPlusOneMask = mask >> bitNPlusOneShift;

                if (!(bitNPlusOneShift < nbits))
                {
                    //Console.WriteLine("!! No rounding needed. Enough space to fit input");
                }
                else if ((bitNPlusOneMask & fracA) != 0)
                {
                    var bitLastMask = bitNPlusOneMask << 1;
                    var bitsMoreMask = bitNPlusOneMask - 1;
                    // logic for round to nearest, tie to even
                    if (((bitLastMask | bitsMoreMask) & fracA) != 0)
                    {
                        uiZ++;
                    }
                }

                Debug.Assert(uiZ == ConvertUIntToPositImpl(iA, 31, 32, 2));
            }

            return new Posit32(sign, uiZ);
        }
    }
}
