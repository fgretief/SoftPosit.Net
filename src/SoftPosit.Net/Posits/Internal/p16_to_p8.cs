// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;
    using posit16_t = Posit16;

    internal static partial class Native
    {
        public static posit8_t p16_to_p8(in posit16_t a)
        {
            const int psA = 16;
            const int psZ = 8;

            if (Posit.IsZeroOrNaR(a))
            {
                return new Posit8((byte)(a.ui >> (psA-psZ)));
            }

            var (sign, kA, tmp) = a;

            sbyte regA;
            ushort exp_frac16A;
            byte uZ_ui;

            if (kA < -3 || kA >= 3)
            {
                regA = 0;
                exp_frac16A = 0;
                uZ_ui = (byte)((kA < 0) ? 0x1 : 0x7F);
            }
            else
            {
                byte regime;

                //2nd bit exp
                exp_frac16A = tmp;
                if (kA < 0)
                {
                    regA = (sbyte)(((-kA) << 1) - (exp_frac16A >> 14));
                    if (regA == 0) regA = 1;
                    regime = (byte)(0x40 >> regA);
                }
                else
                {
                    regA = (sbyte)((kA == 0) ? (1 + (exp_frac16A >> 14)) : (((kA + 1) << 1) + (exp_frac16A >> 14) - 1));
                    regime = (byte)(0x7F - (0x7F >> regA));
                }

                if (regA > 5)
                {
                    uZ_ui = regime;
                }
                else
                {
                    //int shift = regA+8;
                    //exp_frac16A= ((exp_frac16A)&0x3FFF) >> shift; //first 2 bits already empty (for sign and regime terminating bit)
                    uZ_ui = (byte)(regime + (((exp_frac16A) & 0x3FFF) >> (regA + 8)));
                }
            }

            if ((exp_frac16A & (0x80 << regA)) != 0)
            {
                var bitsMore = exp_frac16A & (0xFFFF >> (9 - regA));
                uZ_ui += (byte)((uZ_ui & 1) | bitsMore);
            }

            return new Posit8(sign, uZ_ui);
        }
    }
}