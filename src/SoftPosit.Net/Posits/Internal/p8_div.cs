// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t p8_div(in posit8_t pA, in posit8_t pB)
        {
            byte uiA, uiB, fracA, fracB, regA, regime;
            bool signA, signB, signZ, regSA, regSB, bitNPlusOne = false, rcarry;
            sbyte kA = 0;
            //div_t divresult;

            byte uZ_ui;

            var signMask = Posit8.SignMask;

            uiA = pA.ui;
            uiB = pB.ui;

            //Zero or infinity
            if (uiA == 0x80 || uiB == 0x80 || uiB == 0)
            {
                return Posit8.NaR;
            }
            else if (uiA == 0)
            {
                return Posit8.Zero;
            }

            signA = signP8UI(uiA);
            signB = signP8UI(uiB);
            signZ = signA ^ signB;
            if (signA) uiA = (byte)(-(sbyte)uiA & 0xFF);
            if (signB) uiB = (byte)(-(sbyte)uiB & 0xFF);
            regSA = signregP8UI(uiA);
            regSB = signregP8UI(uiB);

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
            fracA = (byte)(0x80 | tmp);
            ushort frac16A = (ushort)(fracA << 7);

            tmp = (uiB << 2) & 0xFF;
            if (regSB)
            {
                while ((tmp & signMask) != 0)
                {
                    kA--;
                    tmp = (tmp << 1) & 0xFF;
                }
                fracB = (byte)(0x80 | tmp);
            }
            else
            {
                kA++;
                while ((tmp & signMask) == 0)
                {
                    kA++;
                    tmp = (tmp << 1) & 0xFF;
                }
                tmp &= 0x7F;
                fracB = (byte)(0x80 | (0x7F & tmp));
            }

            //divresult = div(frac16A, fracB);
            var divresult_quot = Math.DivRem(frac16A, fracB, out var divresult_rem);
            ushort frac16Z = (ushort)divresult_quot;//divresult.quot;
            ushort rem = (ushort)divresult_rem;//divresult.rem;

            if (frac16Z != 0)
            {
                rcarry = (frac16Z >> 7) != 0; // this is the hidden bit (7th bit) , extreme right bit is bit 0
                if (!rcarry)
                {
                    kA--;
                    frac16Z <<= 1;
                }
            }

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
            if (regA > 6)
            {
                //max or min pos. exp and frac does not matter.
                uZ_ui = (byte)(regSA ? 0x7F : 0x1);
            }
            else
            {
                //remove carry and rcarry bits and shift to correct position
                frac16Z &= 0x7F;
                fracA = (byte)(frac16Z >> (regA + 1));

                bitNPlusOne = (0x1 & (frac16Z >> regA)) != 0;
                uZ_ui = packToP8UI(regime, fracA);

                //uZ.ui = (uint16_t) (regime) + ((uint16_t) (expA)<< (13-regA)) + ((uint16_t)(fracA));
                if (bitNPlusOne)
                {
                    var bitsMore = (((1 << regA) - 1) & frac16Z) != 0 ? 1 : 0;
                    if (rem != 0) bitsMore = 1;
                    //n+1 frac bit is 1. Need to check if another bit is 1 too if not round to even
                    uZ_ui += (byte)((uZ_ui & 1) | bitsMore);
                }
            }
            if (signZ) uZ_ui = (byte)(-(sbyte)uZ_ui & 0xFF);

            return new Posit8(uZ_ui);
        }
    }
}
