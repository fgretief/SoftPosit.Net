// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t p8_roundToInt(posit8_t a)
        {
            var (sign, uiA) = a;

            if (uiA <= 0x20)
            {                 // 0 <= |pA| <= 1/2 rounds to zero.
                return Posit8.Zero;
            }
            else if (uiA < 0x50)
            {                 // 1/2 < x < 3/2 rounds to 1.
                uiA = 0x40;
            }
            else if (uiA <= 0x64)
            {                 // 3/2 <= x <= 5/2 rounds to 2.
                uiA = 0x60;
            }
            else if (uiA >= 0x78)
            {                 // If |A| is 8 or greater, leave it unchanged.
                return a;     // This also takes care of the NaR case, 0x80.
            }
            else
            {
                var mask = (byte)0x20;
                var scale = 0;

                while ((mask & uiA) != 0)
                {
                    scale += 1;
                    mask >>= 1;
                }

                mask >>= scale;
                var bitLast = (uiA & mask) != 0;

                mask >>= 1;
                byte tmp = (byte)(uiA & mask);
                var bitNPlusOne = tmp != 0;
                uiA ^= tmp;
                tmp = (byte)(uiA & (mask - 1)); //bitsMore
                uiA ^= tmp;

                if (bitNPlusOne)
                {
                    if (bitLast || (tmp != 0)) uiA += (byte)(mask << 1);
                }
            }

            return new Posit8(sign, uiA);
        }
    }
}
