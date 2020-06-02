// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit16_t = Posit16;

    internal static partial class Native
    {
        public static posit16_t ui32_to_p16(in uint a)
        {
            var uiA = a;

            // 0x7FFC: + 4^(12) · 2^(0) · (1 + 0/1) = 16777216
            // 0x7FFD: + 4^(12) · 2^(1) · (1 + 0/1) = 33554432
            // 0x7FFE: + 4^(13) · 2^(0) · (1 + 0/1) = 67108864
            // 0x7FFF: + 4^(14) · 2^(0) · (1 + 0/1) = 268435456 (+maxpos)
            // half way between 0x7FFE & 7FFF is: 167772160

            ushort uiZ;
            if (uiA < 2)
            {
                uiZ = (ushort)(uiA << (16 - 2)); // 0, 1
            }
            else if (uiA > 167772160) // midway between 7FFE and 7FFF (round down)
            {
                uiZ = 0x7FFF; // +maxpos
            }
            else if (uiA >= 50331648) // midway between 7FFD and 7FFE (round up)
            {
                uiZ = 0x7FFE;
            }
            else if (uiA > 25165824) // midway between 7FFC and 7FFD (round down)
            {
                uiZ = 0x7FFD;
            }
            else
            {
                // output environment
                const int psZ = 16;
                const int esZ = 1;

                // The maximum value that can reach this point
                // is 25165824, which can fit in 25 bits.
                const int nbits = 25;

                uiZ = (ushort) ConvertUIntToPositImpl(uiA, nbits, psZ, esZ);
            }

            return new Posit16(uiZ);
        }
    }
}