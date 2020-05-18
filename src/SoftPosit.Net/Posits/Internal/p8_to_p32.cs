// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;
    using posit32_t = Posit32;

    internal static partial class Native
    {
        public static posit32_t p8_to_p32(in posit8_t a)
        {
            var uiA = a.ui;

            if (uiA == 0x80 || uiA == 0)
            {
                return new Posit32((uint)(uiA << 24));
            }

            const byte signMask = Posit8.SignMask;

            var sign = signP8UI(uiA);
            if (sign) uiA = (byte)(-(sbyte)uiA & 0xFF);
            var regSA = signregP8UI(uiA);

            sbyte kA = 0;
            var tmp = (byte)((uiA << 2) & 0xFF);
            if (regSA)
            {
                while ((tmp & signMask) != 0)
                {
                    kA++;
                    tmp = (byte)((tmp << 1) & 0xFF);
                }
            }
            else
            {
                kA = -1;
                while ((tmp & signMask) == 0)
                {
                    kA--;
                    tmp = (byte)((tmp << 1) & 0xFF);
                }
                tmp &= 0x7F;
            }
            var exp_frac32A = (uint)(tmp << 22);

            uint regime;
            sbyte regA;
            if (kA < 0)
            {
                regA = (sbyte) -kA;
                // Place exponent bits
                exp_frac32A |= (uint)(((regA & 0x1) | ((regA + 1) & 0x2)) << 29);

                regA = (sbyte)((regA + 3) >> 2);
                if (regA == 0) regA = 1;
                regSA = false;//0;
                regime = 0x40000000u >> regA;
            }
            else
            {
                exp_frac32A |= (uint)((kA & 0x3) << 29);

                regA = (sbyte)((kA + 4) >> 2);
                if (regA == 0) regA = 1;
                regSA = false;//1;
                regime = 0x7FFFFFFFu - (0x7FFFFFFFu >> regA);
            }

            exp_frac32A = ((uint)exp_frac32A) >> (regA + 1); //2 because of sign and regime terminating bit

            var uZ_ui = regime + exp_frac32A;

            if (sign) uZ_ui = (uint)(-(int)uZ_ui & 0xFFFFFFFF);

            return new Posit32(uZ_ui);
        }

    }
}
