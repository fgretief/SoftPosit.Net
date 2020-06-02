// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit64_t = Posit64;

    internal static partial class Native
    {
        public static ulong p64_to_ui64(in posit64_t value)
        {
            if ((long)value.ui <= 0)
            {
                return 0; // Zero, NaR, negative values
            }

            var uiA = value.ui;

            if (uiA <= 0x3400_0000_0000_0000) return 0; // 0 <= |pA| <= 1/2 rounds to zero.
            if (uiA < 0x4200_0000_0000_0000) return 1;  // 1/2 < x < 3/2 rounds to 1.
            if (uiA <= 0x4500_0000_0000_0000) return 2; // 3/2 <= x <= 5/2 rounds to 2. // For speed. Can be commented out
            if (uiA >= 0x7FC0_0000_0000_0000ul)         // 2^64 <= x <= inf
            {
                // overflow, so return max integer value
                return ulong.MaxValue; // UINT_MAX
            }

            return convertP64ToUI64(uiA);
        }

        private static ulong convertP64ToUI64(ulong uiA)
        {
            var (k, tmp) = splitRegimeP64(uiA);

            // input environment
            const int psA = 64;
            const int esA = 3;

            const int expMask = ((1 << esA) - 1);
            var expA = (sbyte)((tmp >> (psA - 1 - esA)) & expMask);

            const ulong signMask = 1ul << (psA - 1);
            var fracA = signMask | (tmp << esA); // add silent 1
            var scaleA = expA + k * (1 << esA);
            var shiftA = (psA - 1) - scaleA;
            var uiZ = fracA >> shiftA;

            if (shiftA > (k + 2 + esA)) // only shifts larger than this will have rounding
            {
                Debug.Assert((scaleA + 1) < 64);
                var bitNPlusOneMask = signMask >> (scaleA + 1);
                if ((bitNPlusOneMask & fracA) != 0)
                {
                    var bitLastMask = bitNPlusOneMask << 1;
                    var bitsMoreMask = bitNPlusOneMask - 1;
                    // logic for round to nearest, tie to even
                    if (((bitLastMask | bitsMoreMask) & fracA) != 0)
                    {
                        uiZ += 1;
                    }
                }
            }

            return uiZ;
        }

        private static (sbyte, ulong) splitRegimeP64(in ulong uiA)
        {
            Debug.Assert((long)uiA > 0, "must be positive");

            const int psA = 64;
            const ulong signMask = 1ul << (psA - 1);

            var signOfRegime = (uiA & (signMask >> 1)) != 0;
            var tmp = uiA << 2;
            sbyte k;

            if (signOfRegime)
            {
                k = 0;
                while ((tmp & signMask) != 0)
                {
                    k++;
                    tmp <<= 1;
                }
            }
            else
            {
                k = -1;
                while ((tmp & signMask) == 0)
                {
                    k--;
                    tmp <<= 1;
                }
                tmp &= signMask - 1;
            }

            return (k, tmp);
        }
    }
}
