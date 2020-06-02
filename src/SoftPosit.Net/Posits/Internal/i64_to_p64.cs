// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit64_t = Posit64;

    internal static partial class Native
    {
        public static posit64_t i64_to_p64(in long value)
        {
            if (value == long.MinValue)
            {
                return new Posit64(0x8048_0000_0000_0000ul); // -9223372036854775808 = -2^63
            }

            var sign = value < 0;
            var iA = sign ? -value : value;

            ulong uiZ;
            if (iA < 2)
            {
                uiZ = (ulong)iA << 62; // 0, 1, -1
            }
            else if (iA >= 9223372036854773760L)
            {
                uiZ = 0x7FB7_FFFF_FFFF_FFFFul; 
            }
            else
            {
                uiZ = convertUI64ToP64((ulong)iA);
                
                Debug.Assert(uiZ == ConvertUIntToPositImpl(iA, 63, 64, 3));
            }

            return new Posit64(sign, uiZ);
        }
    }
}