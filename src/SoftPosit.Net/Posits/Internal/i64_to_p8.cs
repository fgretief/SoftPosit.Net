// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t i64_to_p8(in long value)
        {
            if (value < -48)
            {   //-48 to -MAX_INT rounds to P32 value -268435456
                return new Posit8(0x81); // -maxpos
            }

            var sign = value < 0;
            var iA = sign ? -value : value;

            byte uiZ;
            if (iA > 48)
            {
                uiZ = 0x7F;
            }
            else if (iA < 2)
            {
                uiZ = (byte)(iA << (8 - 2)); // 0, 1, -1
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

                uiZ = (byte)((0x7F ^ (0x3F >> k)) | (byte)(fracA >> (k + 1)));

                mask = 0x1 << k; //bitNPlusOne
                if ((mask & fracA) != 0)
                {
                    if ((((mask - 1) & fracA) | ((mask << 1) & fracA)) != 0)
                    {
                        uiZ++;
                    }
                }

                Debug.Assert(uiZ == ConvertUIntToPositImpl(iA, 6, 8, 0));
            }

            return new Posit8(sign, uiZ);
        }
    }
}
