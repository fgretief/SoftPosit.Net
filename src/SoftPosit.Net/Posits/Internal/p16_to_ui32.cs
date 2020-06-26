// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit16_t = Posit16;

    internal static partial class Native
    {
        public static uint p16_to_ui32(in posit16_t value)
        {
            if ((short)value.ui <= 0)
            {
                return 0; // Zero, NaR, negative values
            }

            var uiA = value.ui;

            if (uiA <= 0x3000) return 0; // 0 <= |pA| <= 1/2 rounds to zero.
            if (uiA < 0x4800) return 1; // 1/2 < x < 3/2 rounds to 1.
            if (uiA <= 0x5400) return 2; // 3/2 <= x <= 5/2 rounds to 2. // For speed. Can be commented out

            return (uint)convertP16ToUI64(uiA);
        }
    }
}
