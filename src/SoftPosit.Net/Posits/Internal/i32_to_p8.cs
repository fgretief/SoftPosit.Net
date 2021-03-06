﻿// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t i32_to_p8(in int value)
        {
            if (value < -48)
            {
                // -48 to -MAX_INT rounds to P8 value -maxpos
                return new Posit8(0x81); // -maxpos
            }

            var sign = value < 0;
            var iA = sign ? -value : value;

            byte uiZ;
            if (iA > 48)
            {
                uiZ = 0x7F;
            }
            else if (iA < 2)
            {
                uiZ = (byte)(iA << (8-2));
            }
            else
            {
                Debug.Assert(iA > 0, "must be positive");

                // output environment
                const int psZ = 8;
                const int esZ = 0;

                // The maximum value that can reach this point
                // is 48, which can fit in 6 bits.
                const int nbits = 6;
                Debug.Assert(nbits > 0);

                int log2 = nbits - 1;
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
                var fracZ = fracA >> shift;

                uiZ = (byte)(regimeZ | expZ | fracZ);

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

                Debug.Assert(uiZ == ConvertUIntToPositImpl(iA, 6, 8, 0));
            }

            return new Posit8(sign, uiZ);
        }
    }
}
