// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;
    using quire8_t = Quire8;

    // ReSharper disable InconsistentNaming

    internal static partial class Native
    {
        static bool isNaRQ8(quire8_t q) => q.m_value == 0x80000000;
        static bool isQ8Zero(quire8_t q) => q.m_value == 0;

        public static posit8_t q8_to_p8(in quire8_t qA)
        {
            //handle Zero
            if (isQ8Zero(qA))
            {
                return Posit8.Zero;
            }
            //handle NaR
            else if (isNaRQ8(qA))
            {
                return Posit8.NaR;
            }

            uint uZ_ui = qA.m_value;

            var sign = (uZ_ui & (1u << 31)) != 0;
            if (sign) uZ_ui = (uint)(-(int)uZ_ui);

            int noLZ = 0;

            uint tmp = uZ_ui;

            while ((tmp & (1<<31)) == 0)
            {
                noLZ++;
                tmp <<= 1;
            }
            var frac32A = tmp;

            //default dot is between bit 19 and 20, extreme left bit is bit 0. Last right bit is bit 31.
            //Scale =  k
            int kA = (19 - noLZ);

            sbyte regA;
            bool regSA;
            byte regime;
            if (kA < 0)
            {
                regA = (sbyte)(-kA & 0xFF);
                regSA = false;//0;
                regime = (byte)(0x40 >> regA);
            }
            else
            {
                regA = (sbyte)(kA + 1);
                regSA = true;//1;
                regime = (byte)(0x7F - (0x7F >> regA));
            }

            byte uA_ui;
            if (regA > 6)
            {
                //max or min pos. exp and frac does not matter.
                uA_ui = (byte)(regSA ? 0x7F : 0x1);
            }
            else
            {
                //remove hidden bit
                frac32A &= 0x7FFFFFFF;
                var shift = regA + 25; // 1 sign bit and 1 r terminating bit , 16+7+2
                var fracA = (int)(frac32A >> shift);

                var bitNPlusOne = ((frac32A >> (shift - 1)) & 1) != 0;

                uA_ui = packToP8UI(regime, fracA);

                if (bitNPlusOne)
                {
                    var bitsMore = ((frac32A << (33 - shift)) & 0xFFFFFFFF) != 0 ? 1 : 0;
                    uA_ui += (byte)((uA_ui & 1) | bitsMore);
                }
            }

            if (sign) uA_ui = (byte)(-(sbyte)uA_ui);
            return new Posit8(uA_ui);
        }
    }
}