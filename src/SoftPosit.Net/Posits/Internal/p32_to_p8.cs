// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;
    using posit32_t = Posit32;

    internal static partial class Native
    {
        public static posit8_t p32_to_p8(in posit32_t a)
        {
            if (Posit.IsZeroOrNaR(a))
            {
                return new Posit8((byte)(a.ui >> 24));
            }

            var uiA = a.ui;

            var sign = signP32UI(uiA);
            if (sign) uiA = (uint)(-(int)uiA);

            byte uZ_ui;
            if (uiA > 0x66000000) uZ_ui = 0x7F;
            else if (uiA < 0x1A000000) uZ_ui = 0x1;
            else
            {
                var signMask = Posit32.SignMask;
                var regSA = signregP32UI(uiA);
                //regime
                sbyte kA;
                var tmp = (uint)(uiA << 2);
                if (regSA)
                {
                    kA = 0;
                    while ((tmp & signMask) != 0)
                    {
                        kA++;
                        tmp = (uint)(tmp << 1);
                    }
                }
                else
                {
                    kA = -1;
                    while ((tmp & signMask) == 0)
                    {
                        kA--;
                        tmp = (uint)(tmp << 1);
                    }
                    tmp &= 0x7FFFFFFFu;
                }

                //2nd and 3rd bit exp
                var exp_frac32A = tmp;

                byte regime;
                sbyte regA;
                if (kA < 0)
                {
                    regA = (sbyte)(((-kA) << 2) - (exp_frac32A >> 29));
                    if (regA == 0) regA = 1;
                    regSA = false;//0;
                    regime = (byte)((regA > 6) ? (0x1) : (0x40 >> regA));

                }
                else
                {
                    regA = (sbyte)((kA == 0) ? (1 + (exp_frac32A >> 29)) : ((kA << 2) + (exp_frac32A >> 29) + 1));
                    regSA = true;//1;
                    regime = (byte)(0x7F - (0x7F >> regA));
                }

                exp_frac32A = (exp_frac32A << 3);
                if (regA > 5)
                {
                    uZ_ui = regime;
                }
                else
                {
                    //exp_frac32A= ((exp_frac32A)&0x3F) >> shift; //first 2 bits already empty (for sign and regime terminating bit)
                    uZ_ui = (byte)(regime | (exp_frac32A >> (regA + 26)));
                }
                if ((exp_frac32A & (0x2000000 << regA)) != 0)
                {
                    var bitsMore = exp_frac32A & (0xFFFFFFFF >> (7 - regA));
                    uZ_ui += (byte)((uZ_ui & 1) | bitsMore);
                }
            }

            if (sign) uZ_ui = (byte)(-(sbyte)uZ_ui);
            return new Posit8(uZ_ui);
        }
    }
}