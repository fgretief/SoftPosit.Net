// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t p8_mul(in posit8_t a, in posit8_t b)
        {
            if (Posit.IsNaR(a) || Posit.IsNaR(b))
            {
                return Posit8.NaR;
            }
            else if (Posit.IsZero(a) || Posit.IsZero(b))
            {
                return Posit8.Zero;
            }

            var (signA, uiA) = a;
            var (signB, uiB) = b;
            bool signZ = signA ^ signB;
            bool regSA = signregP8UI(uiA);
            bool regSB = signregP8UI(uiB);
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

            byte uiZ;

            if (regA > 6)
            {
                //max or min pos. exp and frac does not matter.
                uiZ = (byte)(regSA ? 0x7F : 0x1);
            }
            else
            {
                //remove carry and rcarry bits and shift to correct position
                frac16Z = (ushort)((frac16Z & 0x3FFF) >> regA);
                fracA = (byte)(frac16Z >> 8);
                bool bitNPlusOne = (0x80 & frac16Z) != 0;
                uiZ = packToP8UI(regime, fracA);

                //n+1 frac bit is 1. Need to check if another bit is 1 too if not round to even
                if (bitNPlusOne)
                {
                    // FIXME: this code is never executed for any combination of Posit8 value (p8_mul)
                    var bitsMore = (0x7F & frac16Z) != 0 ? 1 : 0;
                    uiZ += (byte)((uiZ & 1) | bitsMore);
                }
            }

            return new Posit8(signZ, uiZ);
        }

    }
}
