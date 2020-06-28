// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit16_t = Posit16;

    internal static partial class Native
    {
        public static int p16_to_i32(in posit16_t value)
        {
            if (Posit.IsZeroOrNaR(value))
            {
                return 0;
            }

            var (sign, uiA) = value;

            int iZ;
            if (uiA <= 0x3800u) return 0; // 0 <= |x| <= 1/2 rounds to zero.
            else if (uiA < 0x4400u) iZ = 1; // 1/2 < |x| < 3/2 rounds to 1.
            else if (uiA <= 0x5400u) iZ = 2; // 3/2 <= |x| <= 5/2 rounds to 2. // For speed. Can be commented out
            else
            {
                iZ = (int)convertP16ToUI64(uiA);
            }

            return sign ? -iZ : iZ;
        }
    }
}