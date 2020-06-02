// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit16_t = Posit16;

    internal static partial class Native
    {
        public static posit16_t i64_to_p16(in long value)
        {
            if (value < -167772160)
            {
                // -268435455 to -167772160 rounds to P32 value -268435456
                return new Posit16(0x8001); // -maxpos
            }

            var sign = value < 0;
            var iA = sign ? -value : value;

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
                // output environment
                const int psZ = 16;
                const int esZ = 1;

                // The maximum value that can reach this point
                // is 25165824, which can fit in 25 bits.
                const int nbits = 25;

                uiZ = (ushort)ConvertUIntToPositImpl(iA, nbits, psZ, esZ);
            }

            return new Posit16(sign, uiZ);
        }
    }
}
