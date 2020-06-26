// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit64_t = Posit64;

    internal static partial class Native
    {
        public static uint p64_to_ui32(in posit64_t value)
        {
            if ((long)value.ui <= 0)
            {
                return 0; // Zero, NaR, negative values
            }

            var uiA = value.ui;

            if (uiA <= 0x3400_0000_0000_0000) return 0; // 0 <= |pA| <= 1/2 rounds to zero.
            if (uiA < 0x4200_0000_0000_0000) return 1;  // 1/2 < x < 3/2 rounds to 1.
            if (uiA <= 0x4500_0000_0000_0000) return 2; // 3/2 <= x <= 5/2 rounds to 2. // For speed. Can be commented out
            if (uiA > 0x7BFFFFFFFF000000ul)         // 2^32 <= x <= inf
            {
                // overflow, so return max integer value
                return uint.MaxValue; // UINT_MAX
            }

            return (uint) convertP64ToUI64(uiA);
        }
    }
}
