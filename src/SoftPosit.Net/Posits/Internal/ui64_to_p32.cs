// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit32_t = Posit32;

    internal static partial class Native
    {
        public static posit32_t ui64_to_p32(in ulong value)
        {
            var uiA = value;

            uint uiZ;

            if (uiA < 2)
            {
                uiZ = (uint)(uiA << (32 - 2)); // 0, 1
            }
            else if (uiA > 18445618173802708992) // 18445618173802708992 to 18446744073709551615 rounds to P64 value (18446744073709551616) => 0x7FFF_C000
            {
                uiZ = 0x7FFF_C000u;
            }
            else
            {
                // output environment
                const int psZ = 32;
                const int esZ = 2;

                // The maximum value that can reach this point
                // is 2147483135, which can fit in 31 bits.
                const int nbits = 64;

                uiZ = (uint)ConvertUIntToPositImpl(uiA, nbits, psZ, esZ);
            }

            return new Posit32(uiZ);
        }
    }
}
