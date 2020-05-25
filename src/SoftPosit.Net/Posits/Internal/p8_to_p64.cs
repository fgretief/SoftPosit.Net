// SPDX-License-Identifier: MIT

using System.IO;
using System.Text;

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;
    using posit64_t = Posit64;

    internal static partial class Native
    {
        public static posit64_t p8_to_p64(in posit8_t a)
        {
            // input environment
            const int psA = 8;
            const int esA = 0;
            // output environment
            const int psZ = 64;
            const int esZ = 3;

            if (Posit.IsZeroOrNaR(a))
            {
                return new Posit64((ulong)a.ui << (psZ-psA));
            }

            var (sign, kA, tmp) = a;

            // Place fragment bits
            var exp_frac64A = (ulong)tmp << (psZ-psA-(esZ-esA));

            sbyte regA;
            ulong regime;
            if (kA < 0)
            {
                regA = (sbyte)-kA;

                // Place exponent bits (es=3)
                var exp = (ulong)regA & 1;
                exp |= (ulong)(regA + 1) & 2;
                exp |= (ulong)(regA + 3) & 4;
                exp_frac64A |= (exp) << 60;

                regA = (sbyte)((regA + 8) >> 3); // es=3
                if (regA == 0) regA = 1;
                regime = 0x40000000_00000000ul >> regA;
            }
            else
            {
                // Place exponent bits
                exp_frac64A |= ((ulong)(kA & 7) << 60);

                regA = (sbyte)((kA + 8) >> 3); // es=3
                if (regA == 0) regA = 1;
                regime = 0x7FFFFFFF_FFFFFFFFul - (0x7FFFFFFF_FFFFFFFFul >> regA);
            }

            exp_frac64A >>= regA + 1; //2 because of sign and regime terminating bit

            ulong uiZ = regime + exp_frac64A;
            return new Posit64(sign, uiZ);
        }
    }
}
