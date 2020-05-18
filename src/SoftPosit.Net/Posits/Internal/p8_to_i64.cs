// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static long p8_to_i64(in posit8_t a)
        {

            //union ui8_p8 uA;
            long mask, iZ, tmp;
            byte scale = 0;

            var uiA = a.ui;                             // Copy of the input.
            if (uiA == 0x80)
            {                 // NaR
                return 0;
            }

            bool sign = (uiA & Posit8.SignMask) != 0;
            if (sign)
            {
                uiA = (byte)(-(sbyte)uiA & 0xFF);
            }

            if (uiA <= 0x20)
            {                 // 0 <= |pA| <= 1/2 rounds to zero.
                return 0;
            }
            else if (uiA < 0x50)
            {                 // 1/2 < x < 3/2 rounds to 1.
                iZ = 1;
            }
            else
            {                 // Decode the posit, left-justifying as we go.
                bool bitLast, bitNPlusOne;

                uiA -= 0x40;                         // Strip off first regime bit (which is a 1).
                while ((0x20 & uiA) != 0)
                {                                    // Increment scale by 1 for each regime sign bit.
                    scale++ ;                        // Regime sign bit is always 1 in this range.
                    uiA = (byte)((uiA - 0x20) << 1); // Remove the bit; line up the next regime bit.
                }
                uiA <<= 1;                           // Skip over termination bit, which is 0.

                iZ = (long)(((ulong)uiA | 0x40) << 55); // Left-justify fraction in 32-bit result (one left bit padding)

                mask = 0x2000000000000000 >> scale;  // Point to the last bit of the integer part.

                bitLast = (iZ & mask) != 0;          // Extract the bit, without shifting it.
                mask >>= 1;
                tmp = (iZ & mask);
                bitNPlusOne = tmp != 0;              // "True" if nonzero.
                iZ ^= tmp;                           // Erase the bit, if it was set.
                tmp = iZ & (mask - 1);               // tmp has any remaining bits. // This is bitsMore
                iZ ^= tmp;                           // Erase those bits, if any were set.

                if (bitNPlusOne)
                {                                    // logic for round to nearest, tie to even
                    if (((bitLast?1L:0L) | tmp) != 0)
                    {
                        iZ += (mask << 1);
                    }
                }
                iZ = (long)((ulong)iZ >> (61 - scale)); // Right-justify the integer.
            }

            if (sign) iZ = -iZ;                      // Apply the sign of the input.
            return iZ;
        }
    }
}
