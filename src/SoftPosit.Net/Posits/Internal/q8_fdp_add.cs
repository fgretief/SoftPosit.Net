// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;
    using quire8_t = Quire8;

    internal static partial class Native
    {
        static bool isNaRP8UI(byte a) => (a ^ 0x80) == 0;

        public static quire8_t q8_fdp_add(quire8_t q, in posit8_t a, in posit8_t b)
        {
            var uqZ1_ui = q.m_value;

            var uiA = a.ui;
            var uiB = b.ui;

            //NaR
            if (isNaRQ8(q) || isNaRP8UI(uiA) || isNaRP8UI(uiB))
            {
                return Quire8.NaR;
            }
            else if (uiA == 0 || uiB == 0)
            {
                return q;
            }

            //max pos (sign plus and minus)
            var signA = signP8UI(uiA);
            var signB = signP8UI(uiB);
            var signZ2 = signA ^ signB;

            if (signA) uiA = (byte)(-(sbyte)uiA);
            if (signB) uiB = (byte)(-(sbyte)uiB);

            var regSA = signregP8UI(uiA);
            var regSB = signregP8UI(uiB);

            const int signMaskA = Posit8.SignMask;

            sbyte kA;
            var tmp = (byte)(uiA << 2);
            if (regSA)
            {
                kA = 0;
                while ((tmp & signMaskA) != 0)
                {
                    kA++;
                    tmp = (byte)(tmp << 1);
                }
            }
            else
            {
                kA = -1;
                while ((tmp & signMaskA) == 0)
                {
                    kA--;
                    tmp = (byte)(tmp << 1);
                }
                tmp &= 0x7F;
            }
            var fracA = (0x80 | tmp);

            tmp = (byte)(uiB << 2);
            if (regSB)
            {
                while ((tmp & signMaskA) != 0)
                {
                    kA++;
                    tmp = (byte)(tmp << 1);
                }
            }
            else
            {
                kA--;
                while ((tmp & signMaskA) == 0)
                {
                    kA--;
                    tmp = (byte)(tmp << 1);
                }
                tmp &= 0x7F;
            }
            var frac32Z = (uint)(fracA * (0x80 | tmp)) << 16;

            var rcarry = (frac32Z & (1 << 31)) != 0;//1st bit (position 2) of frac32Z, hidden bit is 4th bit (position 3)
            if (rcarry)
            {
                kA++;
                frac32Z >>= 1;
            }

            //default dot is between bit 19 and 20, extreme left bit is bit 0. Last right bit is bit 31.
            //Scale = 2^es * k + e  => 2k + e // firstPost = 19-kA, shift = firstPos -1 (because frac32Z start from 2nd bit)
            //int firstPos = 19 - kA;
            var shiftRight = 18 - kA;

            var uqZ2_ui = (uint)(frac32Z >> shiftRight);
            
            if (signZ2) uqZ2_ui = (uint)(-(int)uqZ2_ui);

            //Addition
            var uqZ = new Quire8(uqZ2_ui + uqZ1_ui);

            //Exception handling
            if (isNaRQ8(uqZ))
            {
                return new Quire8(0);
            }

            return uqZ;
        }
    }
}