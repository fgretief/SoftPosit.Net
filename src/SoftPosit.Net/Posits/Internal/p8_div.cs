// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t p8_div(in posit8_t a, in posit8_t b)
        {
            if (Posit.IsNaR(a) || Posit.IsZeroOrNaR(b))
            {
                return Posit8.NaR;
            }
            else if (Posit.IsZero(a))
            {
                return Posit8.Zero;
            }
            else if (a.ui == b.ui)
            {
                return Posit8.One;
            }

            var (signA, uiA) = a;
            var (signB, uiB) = b;
            var signZ = signA ^ signB;
            var regSA = signregP8UI(uiA);
            var regSB = signregP8UI(uiB);
            var signMask = Posit8.SignMask;

            sbyte kA;
            var tmp = (uiA << 2) & 0xFF;
            if (regSA)
            {
                kA = 0;
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
            byte fracA = (byte)(0x80 | tmp);
            ushort frac16A = (ushort)(fracA << 7);

            tmp = (uiB << 2) & 0xFF;
            if (regSB)
            {
                while ((tmp & signMask) != 0)
                {
                    kA--;
                    tmp = (tmp << 1) & 0xFF;
                }
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
            }
            byte fracB = (byte)(0x80 | tmp);

            //div_t divresult;
            //divresult = div(frac16A, fracB);
            var divresult_quot = Math.DivRem(frac16A, fracB, out var divresult_rem);
            ushort frac16Z = (ushort)divresult_quot;//divresult.quot;
            ushort rem = (ushort)divresult_rem;//divresult.rem;

            if (frac16Z != 0)
            {
                bool rcarry = (frac16Z >> 7) != 0;
                if (!rcarry)
                {
                    kA--;
                    frac16Z <<= 1;
                }
            }

            byte regA;
            byte regime;
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

            byte uiZ;
            if (regA > 6)
            {
                //max or min pos. exp and frac does not matter.
                uiZ = (byte)(regSA ? 0x7F : 0x1);
            }
            else
            {
                //remove carry and rcarry bits and shift to correct position
                frac16Z &= 0x7F;
                fracA = (byte)(frac16Z >> (regA + 1));

                var bitNPlusOne = (0x1 & (frac16Z >> regA)) != 0;
                uiZ = packToP8UI(regime, fracA);

                //uZ.ui = (uint16_t) (regime) + ((uint16_t) (expA)<< (13-regA)) + ((uint16_t)(fracA));
                if (bitNPlusOne)
                {
                    var bitsMore = (((1 << regA) - 1) & frac16Z) != 0 ? 1 : 0;
                    if (rem != 0) bitsMore = 1;
                    //n+1 frac bit is 1. Need to check if another bit is 1 too if not round to even
                    uiZ += (byte)((uiZ & 1) | bitsMore);
                }
            }

            return new Posit8(signZ, uiZ);
        }
    }
}
