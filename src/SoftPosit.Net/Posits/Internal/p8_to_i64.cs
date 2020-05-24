// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static long p8_to_i64(in posit8_t a)
        {
            if (Posit.IsNaR(a))
            {
                return 0; // NaR
            }

            var (sign, uiA) = a;
            long iZ;
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
                byte scale = 0;

                uiA -= 0x40;                         // Strip off first regime bit (which is a 1).
                while ((0x20 & uiA) != 0)
                {                                    // Increment scale by 1 for each regime sign bit.
                    scale++ ;                        // Regime sign bit is always 1 in this range.
                    uiA = (byte)((uiA - 0x20) << 1); // Remove the bit; line up the next regime bit.
                }
                uiA <<= 1;                           // Skip over termination bit, which is 0.

                iZ = (long)(((ulong)uiA | 0x40) << 55); // Left-justify fraction in 32-bit result (one left bit padding)

                var mask = 0x2000000000000000 >> scale;

                var bitLast = (iZ & mask) != 0;
                mask >>= 1;
                var tmp = (iZ & mask);
                var bitNPlusOne = tmp != 0;
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

            return sign ? -iZ : iZ;                  // Apply the sign of the input.
        }
    }
}
