// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit16_t = Posit16;
    using posit32_t = Posit32;

    internal static partial class Native
    {
        public static posit32_t p16_to_p32(in posit16_t a)
        {
            // input environment
            const int psA = 16;
            const int esA = 1;
            // output environment
            const int psZ = 32;
            const int esZ = 2;

            if (Posit.IsZeroOrNaR(a))
            {
                return new Posit32((uint)a.ui << (psZ-psA));
            }

            var (sign, kA, tmp) = a;

            var exp_frac32A = (uint)tmp << (psZ-psA-(esZ-esA));

            sbyte regA;
            uint regime;
            if (kA < 0)
            {
                regA = (sbyte)-kA;
                exp_frac32A |= (uint)(regA & 1) << (psZ-2);
                regA = (sbyte)((regA + 1) >> 1);
                if (regA == 0) regA = 1;
                regime = 0x40000000u >> regA;
            }
            else
            {
                exp_frac32A |= (uint)(kA & 1) << (psZ-2);
                regA = (sbyte)(kA == 0 ? 1 : ((kA + 2) >> 1));

                regime = 0x7FFFFFFFu - (0x7FFFFFFFu >> regA);
            }

            exp_frac32A >>= regA + 1; // +1 because of regime terminating bit

            var uiZ = regime + exp_frac32A;
            return new Posit32(sign, uiZ);
        }
    }
}
