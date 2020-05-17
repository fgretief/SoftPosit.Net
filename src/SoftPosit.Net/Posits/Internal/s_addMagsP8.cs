// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    internal static partial class Native
    {
        private static Posit8 softposit_addMagsP8(in Posit8 a, in Posit8 b)
        {
            return softposit_addMagsP8(a.ui, b.ui);
        }

        private static Posit8 softposit_addMagsP8(byte uiA, byte uiB)
        {
            const byte signMask = Posit8.SignMask;
            //const int signShift = Posit8.SignShift;

            var sign = signP8UI(uiA); //sign is always positive.. actually don't have to do this.
            if (sign)
            {
                uiA = (byte)(-(sbyte)uiA & 0xFF);
                uiB = (byte)(-(sbyte)uiB & 0xFF);
            }

            if ((sbyte)uiA < (sbyte)uiB)
            {
                uiA ^= uiB;
                uiB ^= uiA;
                uiA ^= uiB;
            }

            var regSA = signregP8UI(uiA);
            var regSB = signregP8UI(uiB);

            int kA = 0;
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
            var frac16A = (0x80 | tmp) << 7;
            var shiftRight = kA;

            tmp = (uiB << 2) & 0xFF;
            if (regSB)
            {
                //while (tmp >> 7)
                while ((tmp & signMask) != 0)
                {
                    shiftRight--;
                    tmp = (tmp << 1) & 0xFF;
                }
            }
            else
            {
                shiftRight++;
                //while (!(tmp >> 7))
                while ((tmp & signMask) == 0)
                {
                    shiftRight++;
                    tmp = (tmp << 1) & 0xFF;
                }
                tmp &= 0x7F;
            }
            var frac16B = (0x80 | tmp) << 7;

            //Manage CLANG (LLVM) compiler when shifting right more than number of bits
            frac16B = (shiftRight > 7) ? 0 : (frac16B >> shiftRight); //frac32B >>= shiftRight;

            frac16A += frac16B;

            var rcarry = 0x8000 & frac16A; //first left bit
            if (rcarry != 0)
            {
                kA++;
                frac16A >>= 1;
            }

            int regA;
            int regime;
            if (kA < 0)
            {
                regA = (-kA & 0xFF);
                regSA = false; //0;
                regime = 0x40 >> regA;
            }
            else
            {
                regA = kA + 1;
                regSA = true; //1;
                regime = 0x7F - (0x7F >> regA);
            }

            int uZui;

            if (regA > 6)
            {
                //max or min pos. exp and frac does not matter.
                uZui = regSA ? 0x7F : 0x01;
            }
            else
            {
                frac16A = (frac16A & 0x3FFF) >> regA;
                var fracA = (byte)(frac16A >> 8);
                var bitNPlusOne = (0x80 & frac16A) != 0;
                uZui = packToP8UI(regime, fracA);

                //n+1 frac bit is 1. Need to check if another bit is 1 too if not round to even
                if (bitNPlusOne)
                {
                    int bitsMore = (0x7F & frac16A) != 0 ? 1 : 0;
                    uZui += (uZui & 1) | bitsMore;
                }
            }
            if (sign) uZui = -uZui & 0xFF;

            return new Posit8((byte)uZui);
        }
    }
}
