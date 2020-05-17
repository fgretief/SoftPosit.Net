// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t ui32_to_p8(in uint a)
        {
            byte uiA;

            if (a > 48)
            {
                uiA = 0x7F;
            }
            else if (a < 2)
            {
                uiA = (byte)(a << 6);
            }
            else
            {
                sbyte log2 = 6;//length of bit

                var fracA = a;
                while ((fracA & 0x40) == 0)
                {
                    log2--;
                    fracA <<= 1;
                }

                var k = log2;

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

            return new Posit8((byte)uiA);
        }

    }
}
