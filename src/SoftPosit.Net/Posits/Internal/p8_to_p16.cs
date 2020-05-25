// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;
    using posit16_t = Posit16;

    internal static partial class Native
    {
        public static posit16_t p8_to_p16(in posit8_t a)
        {
            // input environment
            const int psA = 8;
            const int esA = 0;
            // output environment
            const int psZ = 16;
            const int esZ = 1;

            if (Posit.IsZeroOrNaR(a))
            {
                return new Posit16((ushort)(a.ui << (psZ-psA)));
            }

            var (sign, kA, tmp) = a;

            var exp_frac16A = (ushort)(tmp << (psZ-psA-(esZ-esA)));

            sbyte regA;
            ushort regime;
            if (kA < 0)
            {
                regA = (sbyte) -kA;
                if ((regA & 1) != 0) exp_frac16A |= 0x4000;
                regA = (sbyte)((regA + 1) >> 1);
                if (regA == 0) regA = 1;
                regime = (ushort)(0x4000 >> regA);
            }
            else
            {
                regA = kA;
                if ((regA & 1) != 0) exp_frac16A |= 0x4000;
                regA = (sbyte)((regA + 2) >> 1);
                if (regA == 0) regA = 1;
                regime = (ushort)(0x7FFF - (0x7FFF >> regA));
            }

            exp_frac16A >>= regA + 1; //1 because of sign and regime terminating bit

            var uiZ = (ushort)(regime + exp_frac16A);
            return new Posit16(sign, uiZ);
        }
    }
}
