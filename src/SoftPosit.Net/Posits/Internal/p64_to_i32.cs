// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit64_t = Posit64;

    internal static partial class Native
    {
        public static int p64_to_i32(in posit64_t value)
        {
            if (Posit.IsZeroOrNaR(value))
            {
                return 0;
            }

            var (sign, uiA) = value;

            int iZ;
            if (uiA <= 0x3C00_0000_0000_0000) return 0;     // 0 <= |x| <= 1/2 rounds to zero.
            else if (uiA < 0x4200_0000_0000_0000) iZ = 1;  // 1/2 < |x| < 3/2 rounds to 1.
            else if (uiA <= 0x4500_0000_0000_0000) iZ = 2; // 3/2 <= |x| <= 5/2 rounds to 2. // For speed. Can be commented out
            else if (uiA >= 0x7B80_0000_0000_0000)          // 2^31 <= |x| <= inf
            {
                // overflow, so return max integer value
                return sign ? int.MinValue : int.MaxValue; // INT_MAX
            }
            else
            {
                iZ = (int)convertP64ToUI64(uiA);
            }

            return sign ? -iZ : iZ;
        }
    }
}