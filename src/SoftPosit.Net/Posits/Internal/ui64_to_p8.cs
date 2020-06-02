// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t ui64_to_p8(in ulong value)
        {
            var uiA = value;

            byte uiZ;
            if (value > 48)
            {
                uiZ = 0x7F; // +maxpos
            }
            else if (value < 2)
            {
                uiZ = (byte)(uiA << 6); // 0, 1
            }
            else
            {
                sbyte log2 = 6;//length of bit

                ulong mask = 0x40;
                ulong fracA = uiA;
                while ((fracA & mask) == 0)
                {
                    log2--;
                    fracA <<= 1;
                }

                var k = log2;
                fracA = (fracA ^ mask);
                uiZ = (byte)((0x7F ^ (0x3F >> k)) | (byte)(fracA >> (k + 1)));

                mask = 0x1ul << k; //bitNPlusOne
                if ((mask & fracA) != 0)
                {
                    if ((((mask - 1) & fracA) | ((mask << 1) & fracA)) != 0)
                    {
                        uiZ++;
                    }
                }

                Debug.Assert(uiZ == ConvertUIntToPositImpl(uiA, 6, 8, 0));
            }

            return new Posit8(uiZ);
        }
    }
}
