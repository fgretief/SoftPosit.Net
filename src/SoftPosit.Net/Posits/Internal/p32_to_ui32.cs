// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit32_t = Posit32;

    internal static partial class Native
    {
        public static uint p32_to_ui32(in posit32_t value)
        {
            if ((int)value.ui <= 0)
            {
                return 0; // Zero, NaR, negative values
            }

            var uiA = value.ui;

            if (uiA <= 0x3800_0000u) return 0; // 0 <= |pA| <= 1/2 rounds to zero.
            if (uiA < 0x4400_0000u) return 1; // 1/2 < x < 3/2 rounds to 1.
            if (uiA <= 0x4600_0000u) return 2; // 3/2 <= x <= 5/2 rounds to 2. // For speed. Can be commented out
            if (uiA >= 0x7FC0_0000u)           // 2^32 <= x <= inf
            {
                // overflow, so return max integer value
                return uint.MaxValue; // UINT_MAX
            }

            return (uint) convertP32ToUI64(uiA);
        }
    }
}
