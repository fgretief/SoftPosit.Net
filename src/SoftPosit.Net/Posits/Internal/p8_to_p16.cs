// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;
    using posit16_t = Posit16;

    internal static partial class Native
    {
        public static posit16_t p8_to_p16(in posit8_t a)
        {
            var signMask = Posit8.SignMask;

            var uiA = a.ui;

            // NaR or Zero
            if (uiA == 0x80 || uiA == 0)
            {
                return new Posit16((ushort)(uiA << 8));
            }

            var sign = signP8UI(uiA);
            if (sign) uiA = (byte)(-(sbyte)uiA & 0xFF);
            var regSA = signregP8UI(uiA);

            sbyte kA = 0;
            byte tmp = (byte)((uiA << 2) & 0xFF);
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
            ushort exp_frac16A = (ushort)(tmp << 8);

            ushort regime;
            sbyte regA;
            if (kA < 0)
            {
                regA = (sbyte) -kA;
                if ((regA & 0x1) != 0) {exp_frac16A |= 0x8000;}
                regA = (sbyte)((regA + 1) >> 1);
                if (regA == 0) regA = 1;
                regSA = false;//0;
                regime = (ushort)(0x4000 >> regA);
            }
            else
            {
                if ((kA & 0x1) != 0) exp_frac16A |= 0x8000;
                regA = (sbyte)((kA + 2) >> 1);
                if (regA == 0) regA = 1;
                regSA = true;//1;
                regime = (ushort)(0x7FFF - (0x7FFF >> regA));
            }

            exp_frac16A >>= (regA + 2); //2 because of sign and regime terminating bit

            ushort uZ_ui = (ushort)(regime + exp_frac16A);

            if (sign) uZ_ui = (ushort)(-(short)uZ_ui & 0xFFFF);

            return new Posit16(uZ_ui);
        }
    }
}
