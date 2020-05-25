// SPDX-License-Identifier: MIT

using System.Data;
using System.Diagnostics.Contracts;
using System.Text;

namespace System.Numerics.Posits.Internal
{
    using posit32_t = Posit32;
    using posit64_t = Posit64;

    internal static partial class Native
    {
        public static posit64_t p32_to_p64(in posit32_t a)
        {
            // input environment
            const int psA = 32;
            const int esA = 2;
            // output environment
            const int psZ = 64;
            const int esZ = 3;

            if (Posit.IsZeroOrNaR(a))
            {
                return new Posit64((ulong)a.ui << (psZ - psA));
            }

            var (sign, kA, tmp) = a;

            var exp_frac64A = (ulong)tmp << (psZ-psA-(esZ-esA));

            const ulong signMaskZ = Posit64.SignMask;

            sbyte regA;
            ulong regime;
            if (kA < 0)
            {
                regA = (sbyte)(-kA);

                // Place exponent bits
                exp_frac64A |= (ulong)(kA & 1) << 62;

                regA = (sbyte)((regA + 1) >> 1);
                //if (regA == 0) regA = 1;
                //regime = 0x40000000_00000000ul >> regA;
                regime = signMaskZ >> (regA + 1);
            }
            else
            {
                // Place exponent bits
                exp_frac64A |= (ulong)(kA & 1) << 62;

                //regA = (sbyte)((kA + 8) >> 3); // es=3
                //Console.WriteLine("+ regA = {0}", regA);
                //regA = (sbyte)((kA == 0) ? 1 : (kA + 2) >> 1);
                regA = (sbyte)((kA + 2) >> 1);
                //regime = 0x7FFFFFFF_FFFFFFFFul - (0x7FFFFFFF_FFFFFFFFul >> regA);
                regime = ~signMaskZ - (~signMaskZ >> regA);
            }

            exp_frac64A >>= regA + 1; // +1 because of regime terminating bit

            var uiZ = regime + exp_frac64A;
            return new Posit64(sign, uiZ);
        }
    }
}