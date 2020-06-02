// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit64_t = Posit64;

    internal static partial class Native
    {
        public static long p64_to_i64(in posit64_t value)
        {
            if (Posit.IsZeroOrNaR(value))
            {
                return 0;
            }

            var (sign, uiA) = value;

            long iZ;
            if (uiA <= 0x3400_0000_0000_0000) return 0;    // 0 <= |pA| <= 1/2 rounds to zero.
            else if (uiA < 0x4200_0000_0000_0000) iZ = 1;  // 1/2 < x < 3/2 rounds to 1.
            else if (uiA <= 0x4500_0000_0000_0000) iZ = 2; // 3/2 <= x <= 5/2 rounds to 2. // For speed. Can be commented out
            else if (uiA >= 0x7FB8_0000_0000_0000ul)       // 2^63 <= x <= inf
            {
                // overflow, so return max integer value
                return sign ? long.MinValue : long.MaxValue; // INT_MAX
            }
            else
            {
                iZ = (long)convertP64ToUI64(uiA);
            }

            return sign ? -iZ : iZ;
        }
    }
}
