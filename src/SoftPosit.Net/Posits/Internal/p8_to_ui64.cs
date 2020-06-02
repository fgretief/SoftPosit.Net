// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static ulong p8_to_ui64(in posit8_t a)
        {
            var uiA = a.ui;                             // Copy of the input.
            if (uiA >= 0x80)
            {
                return 0;  // Negative or NaR
            }

            ulong uiZ;
            if (uiA <= 0x20)
            {                     // 0 <= |pA| <= 1/2 rounds to zero.
                return 0;
            }
            else if (uiA < 0x50)
            {                 // 1/2 < x < 3/2 rounds to 1.
                uiZ = 1;
            }
            else
            {                                   // Decode the posit, left-justifying as we go.
                byte scale = 0;

                uiA -= 0x40;                       // Strip off first regime bit (which is a 1).
                while ((0x20 & uiA) != 0)
                {               // Increment scale by 1 for each regime sign bit.
                    scale++;                      // Regime sign bit is always 1 in this range.
                    uiA = (byte)((uiA - 0x20) << 1); // Remove the bit; line up the next regime bit.
                }
                uiA <<= 1;                           // Skip over termination bit, which is 0.

                uiZ = ((ulong)uiA | 0x40) << 55;      // Left-justify fraction in 32-bit result (one left bit padding)

                var mask = 0x2000000000000000ul >> scale;  // Point to the last bit of the integer part.

                var bitLast = (uiZ & mask);      // Extract the bit, without shifting it.
                mask >>= 1;
                var tmp = (uiZ & mask);
                var bitNPlusOne = tmp != 0;
                uiZ ^= tmp;                           // Erase the bit, if it was set.
                tmp = uiZ & (mask - 1);               // tmp has any remaining bits. // This is bitsMore
                uiZ ^= tmp;                           // Erase those bits, if any were set.

                if (bitNPlusOne)
                {                   // logic for round to nearest, tie to even
                    if ((bitLast | tmp) != 0)
                    {
                        uiZ += (mask << 1);
                    }
                }
                uiZ = (ulong)uiZ >> (61 - scale);      // Right-justify the integer.
            }

            return uiZ;
        }
    }
}
