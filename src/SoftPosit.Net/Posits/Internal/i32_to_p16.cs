// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit16_t = Posit16;

    internal static partial class Native
    {
        public static posit16_t i32_to_p16(in int value)
        {
            if (value < -134217728)
            {   // -2147483648 to -134217729 rounds to P32 value -268435456
                return new Posit16((ushort)0x8001); //-maxpos
            }

            var sign = value < 0;
            var iA = sign ? -value : value;

            // 0x7FFC: + 4^(12) · 2^(0) · (1 + 0/1) = 16777216
            // 0x7FFD: + 4^(12) · 2^(1) · (1 + 0/1) = 33554432
            // 0x7FFE: + 4^(13) · 2^(0) · (1 + 0/1) = 67108864
            // 0x7FFF: + 4^(14) · 2^(0) · (1 + 0/1) = 268435456 (+maxpos)
            // half way between 0x7FFE & 7FFF is: 167772160

            ushort uiZ;
            if (iA < 2)
            {
                uiZ = (ushort)(iA << (16 - 2)); // 0, 1, -1
            }
            else if (iA > 167772160) // midway between 7FFE and 7FFF (round down)
            {
                uiZ = 0x7FFF; // +maxpos
            }
            else if (iA >= 50331648) // midway between 7FFD and 7FFE (round up)
            {
                uiZ = 0x7FFE;
            }
            else if (iA > 25165824) // midway between 7FFC and 7FFD (round down)
            {
                uiZ = 0x7FFD;
            }
            else
            {
                Debug.Assert(iA > 0, "must be positive");

                // output environment
                const int psZ = 16;
                const int esZ = 1;

                // The maximum value that can reach this point
                // is 25165824, which can fit in 25 bits.
                const int nbits = 25;
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
                var expZ = (ushort)((log2 % (1 << esZ)) << ((psZ - 1) - (k + 2) - esZ));

                var shift = (nbits - psZ) + (k + 2) + esZ;
                var fracZ = (ushort)(shift < 0 ? fracA << -shift : fracA >> shift);
                //var fracZ = (ushort)(fracA >> shift); // original

                uiZ = (ushort)(regimeZ | expZ | fracZ);

                var bitNPlusOneShift = psZ - (k + 2) - esZ;
                var bitNPlusOneMask = mask >> bitNPlusOneShift;

                if ((bitNPlusOneMask & fracA) != 0)
                {
                    var bitLastMask = bitNPlusOneMask << 1;
                    var bitsMoreMask = bitNPlusOneMask - 1;
                    // logic for round to nearest, tie to even
                    if (((bitLastMask| bitsMoreMask) & fracA) != 0)
                    {
                        uiZ++;
                    }
                }

                Debug.Assert(uiZ == ConvertUIntToPositImpl(iA, 25, 16, 1));
            }

            return new Posit16(sign, uiZ);
        }
    }
}
