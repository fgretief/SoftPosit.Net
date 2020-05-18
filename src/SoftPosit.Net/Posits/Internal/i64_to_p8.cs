// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t i64_to_p8(in long a)
        {
            var iA = a;

            if (iA < -48)
            {   //-48 to -MAX_INT rounds to P32 value -268435456
                return new Posit8(0x81); // -maxpos
            }

            bool sign = (iA >> 63) != 0;
            if (sign)
            {
                iA = -iA;
            }

            byte uiA;
            if (iA > 48)
            {
                uiA = 0x7F;
            }
            else if (iA < 2)
            {
                uiA = (byte)(iA << 6);
            }
            else
            {
                sbyte k, log2 = 6;//length of bit

                var mask = 0x40;
                var fracA = iA;
                while ((fracA & mask) == 0)
                {
                    log2--;
                    fracA <<= 1;
                }

                k = log2;

                fracA = (fracA ^ mask);

                uiA = (byte)((0x7F ^ (0x3F >> k)) | (fracA >> (k + 1)));

                mask = 0x1 << k; //bitNPlusOne
                if ((mask & fracA) != 0)
                {
                    if ((((mask - 1) & fracA) | ((mask << 1) & fracA)) != 0)
                    {
                        uiA++;
                    }
                }
            }

            var uZ_ui = (byte)(sign ? -uiA & 0xFF : uiA);
            return new Posit8(uZ_ui);
        }
    }
}
