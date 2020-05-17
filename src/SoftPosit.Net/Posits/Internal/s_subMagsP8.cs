// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    internal static partial class Native
    {
        private static Posit8 softposit_subMagsP8(in Posit8 a, in Posit8 b)
        {
            return softposit_subMagsP8(a.ui, b.ui);
        }

        private static Posit8 softposit_subMagsP8(byte uiA, byte uiB)
        {
            const byte signMask = Posit8.SignMask;
            //const int signShift = Posit8.SignShift;

            //Both uiA and uiB are actually the same signs if uiB inherits sign of sub
            //Make both positive
            var sign = signP8UI(uiA);
            if (sign)
                uiA = (byte)(-(sbyte)uiA & 0xFF);
            else
                uiB = (byte)(-(sbyte)uiB & 0xFF);

            if (uiA == uiB)
            {   // essential, if not need special handling
                return Posit8.Zero;
            }

            if (uiA < uiB)
            {
                uiA ^= uiB;
                uiB ^= uiA;
                uiA ^= uiB;
                sign = !sign; //A becomes B
            }

            int kA;

            var regSA = signregP8UI(uiA);
            var regSB = signregP8UI(uiB);

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
            var frac16A = (0x80 | tmp) << 7;
            var shiftRight = kA;

            tmp = (uiB << 2) & 0xFF;
            if (regSB)
            {
                while ((tmp & signMask) != 0)
                {
                    shiftRight--;
                    tmp = (tmp << 1) & 0xFF;
                }
            }
            else
            {
                shiftRight++;
                while ((tmp & signMask) == 0)
                {
                    shiftRight++;
                    tmp = (tmp << 1) & 0xFF;
                }
                tmp &= 0x7F;
            }
            var frac16B = (0x80 | tmp) << 7;

            byte uZ_ui;
            if (shiftRight >= 14)
            {
                // FIXME: this code is never called for ANY combination of Posit8 values (p8_sub/p8_add)
                uZ_ui = uiA;
                if (sign) uZ_ui = (byte)(-(sbyte)uZ_ui & 0xFFFF);
                return new Posit8(uZ_ui);
            }
            else
            {
                frac16B >>= shiftRight;
            }
            frac16A -= frac16B;
            while ((frac16A >> 14) == 0)
            {
                kA--;
                frac16A <<= 1;
            }
            var ecarry = ((0x4000 & frac16A) >> 14) != 0;
            if (!ecarry)
            {
                // FIXME: this code is never called for ANY combination of Posit8 values (p8_sub/p8_add)
                kA--;
                frac16A <<= 1;
            }

            int regA;
            int regime;

            if (kA < 0)
            {
                regA = (-kA & 0xFF);
                regSA = false; // 0;
                regime = 0x40 >> regA;
            }
            else
            {
                regA = kA + 1;
                regSA = true; // 1;
                regime = 0x7F - (0x7F >> regA);
            }

            if (regA > 6)
            {
                // FIXME: Never triggered for ANY combination of Posit8 values (p8_sub/p8_add)

                //max or min pos. exp and frac does not matter.
                uZ_ui = (byte)(regSA ? 0x7F : 0x01);
            }
            else
            {
                frac16A = (frac16A & 0x3FFF) >> regA;
                var fracA = (byte)(frac16A >> 8);
                bool bitNPlusOne = (0x80 & frac16A) != 0;
                uZ_ui = packToP8UI(regime, fracA);

                if (bitNPlusOne)
                {
                    var bitsMore = (0x7F & frac16A) != 0 ? 1 : 0;
                    uZ_ui += (byte)((uZ_ui & 1) | bitsMore);
                }
            }

            if (sign)
            {
                uZ_ui = (byte)(-(sbyte)uZ_ui & 0xFF);
            }

            return new Posit8(uZ_ui);
        }
    }
}
