// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit32_t = Posit32;

    internal static partial class Native
    {
        public static posit32_t ui32_to_p32(in uint a)
        {
            var uiA = a;

            uint uiZ;
            if (uiA < 2)
            {
                uiZ = (uint)(uiA << (32-2)); // 0, 1
            }
            else if (uiA > 4294966272u) // 4294966272 to 4294967295 round up
            {
                uiZ = 0x7FC0_0000u;
            }
            else
            {
                // output environment
                const int psZ = 32;
                const int esZ = 2;

                // The maximum value that can reach this point
                // is 4294966272, which can fit in 32 bits.
                const int nbits = 32;

                uiZ = (uint) ConvertUIntToPositImpl(uiA, nbits, psZ, esZ);
            }

            return new Posit32(uiZ);
        }
    }
}