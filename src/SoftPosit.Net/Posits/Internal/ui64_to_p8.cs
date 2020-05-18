// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t ui64_to_p8(in ulong a)
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

                ulong mask = 0x40;
                ulong fracA = a;
                while ((fracA & mask) == 0)
                {
                    log2--;
                    fracA <<= 1;
                }

                var k = log2;
                fracA = (fracA ^ mask);
                uiA = (byte)((ulong)(0x7F ^ (0x3F >> k)) | (fracA >> (k + 1)));

                mask = 0x1ul << k; //bitNPlusOne
                if ((mask & fracA) != 0)
                {
                    if ((((mask - 1) & fracA) | ((mask << 1) & fracA)) != 0)
                    {
                        uiA++;
                    }
                }
            }

            return new Posit8(uiA);
        }

    }
}
