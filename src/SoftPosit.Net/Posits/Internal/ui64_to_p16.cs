// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit16_t = Posit16;

    internal static partial class Native
    {
        public static posit16_t ui64_to_p16(in ulong value)
        {
            var uiA = value;

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

                uiZ = (ushort)ConvertUIntToPositImpl(uiA, nbits, psZ, esZ);
            }

            return new Posit16(uiZ);
        }
    }
}
