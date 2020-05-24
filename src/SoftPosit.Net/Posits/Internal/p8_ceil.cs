// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t p8_ceil(posit8_t a)
        {
            var (sign, uiA) = a;

            if (uiA == 0)
            {
                return a;
            }
            else if (uiA <= 0x40)
            {                    // 0 <= |pA| < 1 floor to zero.(if not negative and whole number)
                uiA = (byte)((sign && (uiA != 0x40)) ? 0x0 : 0x40);
            }
            else if (uiA <= 0x60)
            {                    // 1 <= x < 2 floor to 1 (if not negative and whole number)
                uiA = (byte)((sign & (uiA != 0x60)) ? 0x40 : 0x60);
            }
            else if (uiA <= 0x68)
            {                    // 2 <= x < 3 floor to 2 (if not negative and whole number)
                uiA = (byte)((sign & (uiA != 0x68)) ? 0x60 : 0x68);
            }
            else if (uiA >= 0x78)
            {                    // If |A| is 8 or greater, leave it unchanged.
                return a;        // This also takes care of the NaR case, 0x80.
            }
            else
            {
                byte mask = 0x20, scale = 0;

                while ((mask & uiA) != 0)
                {
                    scale += 1;
                    mask >>= 1;
                }

                mask >>= scale;

                mask >>= 1;
                var tmp = (byte)(uiA & mask);
                var bitNPlusOne = tmp != 0;
                uiA ^= tmp;
                tmp = (byte)(uiA & (mask - 1));     //bitsMore
                uiA ^= tmp;

                if (!sign && (bitNPlusOne || (tmp != 0)))
                {
                    uiA += (byte)(mask << 1);
                }
            }

            return new Posit8(sign, uiA);
        }
    }
}
