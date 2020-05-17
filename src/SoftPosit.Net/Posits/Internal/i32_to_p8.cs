// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t i32_to_p8(int iA)
        {
            int k, log2 = 6;//length of bit

            if (iA < -48)
            {
                //-48 to -MAX_INT rounds to P32 value -268435456
                return new Posit8(0x81); //-maxpos
            }

            var sign = ((uint)iA >> 31) != 0;
            if (sign)
            {
                iA = (int)(-iA & 0xFFFFFFFFu);
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
                uint fracA = (uint)iA;
                while ((fracA & 0x40) == 0)
                {
                    log2--;
                    fracA <<= 1;
                }

                k = log2;

                fracA = (fracA ^ 0x40);

                uiA = (byte)((0x7F ^ (0x3F >> k)) | (fracA >> (k + 1)));

                var mask = 0x1 << k; //bitNPlusOne
                if ((mask & fracA) != 0)
                {
                    if ((((mask - 1) & fracA) | ((mask << 1) & fracA)) != 0)
                    {
                        uiA++;
                    }
                }
            }
            var uZ_ui = sign ? -uiA & 0xFF : uiA;
            return new Posit8((byte)uZ_ui);
        }
    }
}
