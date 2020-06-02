// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit32_t = Posit32;

    internal static partial class Native
    {
        public static posit32_t i64_to_p32(in long value)
        {
            if (value < -9222809086901354496L)
            {
                return new Posit32(0x80005000u);
            }

            var sign = value < 0;
            var iA = sign ? -value : value;

            uint uiZ;
            if (iA < 0x2)
            {
                uiZ = (uint)(iA << (32 - 2)); // 0, 1, -1
            }
            else if (iA > 9222809086901354496L)
            {
                uiZ = 0x7FFFB000u;
            }
            else
            {
                // output environment
                const int psZ = 32;
                const int esZ = 2;

                // The maximum value that can reach this point
                // is 9222809086901354496, which can fit in 63 bits.
                const int nbits = 63;

                uiZ = (uint)ConvertUIntToPositImpl(iA, nbits, psZ, esZ);
            }

            return new Posit32(sign, uiZ);
        }
    }
}