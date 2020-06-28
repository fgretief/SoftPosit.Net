// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit32_t = Posit32;

    internal static partial class Native
    {
        public static int p32_to_i32(in posit32_t value)
        {
            if (Posit.IsZeroOrNaR(value))
            {
                return 0;
            }

            var (sign, uiA) = value;

            int iZ;
            if (uiA <= 0x3800_0000u) return 0;    // 0 <= |pA| <= 1/2 rounds to zero.
            else if (uiA < 0x4400_0000u) iZ = 1;  // 1/2 < x < 3/2 rounds to 1.
            else if (uiA <= 0x4600_0000u) iZ = 2; // 3/2 <= x <= 5/2 rounds to 2. // For speed. Can be commented out
            else if (uiA >= 0x7FB0_0000u)         // 2^31 <= x <= inf
            {
                // overflow, so return max integer value
                return sign ? int.MinValue : int.MaxValue; // INT_MAX
            }
            else
            {
                iZ = (int)convertP32ToUI64(uiA);
            }

            return sign ? -iZ : iZ;
        }
    }
}