// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t p8_mul(in posit8_t pA, in posit8_t pB)
        {
            sbyte kA = 0;

            var signMask = Posit8.SignMask;

            var uiA = pA.ui;
            var uiB = pB.ui;

            //NaR or Zero
            if (uiA == 0x80 || uiB == 0x80)
            {
                return Posit8.NaR;
            }
            else if (uiA == 0 || uiB == 0)
            {
                return Posit8.Zero;
            }

            bool signA = signP8UI(uiA);
            bool signB = signP8UI(uiB);
            bool signZ = signA ^ signB;

            if (signA) uiA = (byte)(-(sbyte)uiA & 0xFF);
            if (signB) uiB = (byte)(-(sbyte)uiB & 0xFF);

            bool regSA = signregP8UI(uiA);
            bool regSB = signregP8UI(uiB);

            var tmp = (uiA << 2) & 0xFF;
            if (regSA)
            {
                while ((tmp & signMask) != 0)
                {
                    kA++;
                    tmp = (tmp << 1) & 0xFF;
                }
            }
            else
            {
                kA = -1;
                while ((tmp & signMask) == 0)
                {
                    kA--;
                    tmp = (tmp << 1) & 0xFF;
                }
                tmp &= 0x7F;
            }
            var fracA = (0x80 | tmp);

            tmp = (uiB << 2) & 0xFF;
            if (regSB)
            {
                while ((tmp & signMask) != 0)
                {
                    kA++;
                    tmp = (tmp << 1) & 0xFF;
                }
            }
            else
            {
                kA--;
                while ((tmp & signMask) == 0)
                {
                    kA--;
                    tmp = (tmp << 1) & 0xFF;
                }
                tmp &= 0x7F;
            }
            ushort frac16Z = (ushort)((ushort)fracA * (0x80 | tmp));

            var rcarry = (frac16Z & (1 << 15)) != 0;
            if (rcarry)
            {
                // FIXME: this code is never executed for any combination of Posit8 value (p8_mul)
                kA++;
                frac16Z >>= 1;
            }

            byte regA, regime;
            if (kA < 0)
            {
                regA = (byte)(-kA & 0xFF);
                regSA = false; //0;
                regime = (byte)(0x40 >> regA);
            }
            else
            {
                regA = (byte)(kA + 1);
                regSA = true; //1;
                regime = (byte)(0x7F - (0x7F >> regA));
            }

            byte uZ_ui;

            if (regA > 6)
            {
                //max or min pos. exp and frac does not matter.
                uZ_ui = (byte)(regSA ? 0x7F : 0x1);
            }
            else
            {
                //remove carry and rcarry bits and shift to correct position
                frac16Z = (ushort)((frac16Z & 0x3FFF) >> regA);
                fracA = (byte)(frac16Z >> 8);
                bool bitNPlusOne = (0x80 & frac16Z) != 0;
                uZ_ui = packToP8UI(regime, fracA);

                //n+1 frac bit is 1. Need to check if another bit is 1 too if not round to even
                if (bitNPlusOne)
                {
                    // FIXME: this code is never executed for any combination of Posit8 value (p8_mul)
                    var bitsMore = (0x7F & frac16Z) != 0 ? 1 : 0;
                    uZ_ui += (byte)((uZ_ui & 1) | bitsMore);
                }
            }

            if (signZ)
            {
                uZ_ui = (byte)(-(sbyte)uZ_ui & 0xFF);
            }

            return new Posit8(uZ_ui);
        }

    }
}
