// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static uint p8_to_ui32(in posit8_t pA)
        {
            uint iZ;
            byte uiA = pA.ui;

            if (uiA >= 0x80) return 0;                // NaR or negative

            if (uiA <= 0x20)
            {                                         // 0 <= |pA| <= 1/2 rounds to zero.
                return 0;
            }
            else if (uiA < 0x50)
            {                                         // 1/2 < x < 3/2 rounds to 1.
                iZ = 1;
            }
            else
            {                                        // Decode the posit, left-justifying as we go.
                byte scale = 0;
                uiA -= 0x40;                         // Strip off first regime bit (which is a 1).
                while ((0x20 & uiA) != 0)
                {                                    // Increment scale by 1 for each regime sign bit.
                    scale++;                         // Regime sign bit is always 1 in this range.
                    uiA = (byte)((uiA - 0x20) << 1); // Remove the bit; line up the next regime bit.
                }
                uiA <<= 1;                           // Skip over termination bit, which is 0.

                iZ = ((uint)uiA | 0x40) << 24;       // Left-justify fraction in 32-bit result (one left bit padding)

                uint mask = 0x40000000u >> scale;

                bool bitLast = (iZ & mask) != 0;
                mask >>= 1;
                uint tmp = (iZ & mask);
                bool bitNPlusOne = tmp != 0;
                iZ ^= tmp;                           // Erase the bit, if it was set.
                tmp = iZ & (mask - 1);               // tmp has any remaining bits. // This is bitsMore
                iZ ^= tmp;                           // Erase those bits, if any were set.

                if (bitNPlusOne)
                {                                    // logic for round to nearest, tie to even
                    if (bitLast || tmp != 0)
                        iZ += (mask << 1);
                }
                iZ = iZ >> (30 - scale);             // Right-justify the integer.
            }

            return iZ;
        }
    }
}
