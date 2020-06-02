// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit16_t = Posit16;

    internal static partial class Native
    {
        public static ulong p16_to_ui64(in posit16_t value)
        {
            if ((short)value.ui <= 0)
            {
                return 0; // Zero, NaR, negative values
            }

            var uiA = value.ui;

            if (uiA <= 0x3000) return 0; // 0 <= |pA| <= 1/2 rounds to zero.
            if (uiA < 0x4800) return 1; // 1/2 < x < 3/2 rounds to 1.
            if (uiA <= 0x5400) return 2; // 3/2 <= x <= 5/2 rounds to 2. // For speed. Can be commented out

            return convertP16ToUI64(uiA);
        }

        private static ulong convertP16ToUI64(ushort uiA)
        {
            var (k, tmp) = splitRegimeP16(uiA);

            // input environment
            const int psA = 16;
            const int esA = 1;

            const int expMask = ((1 << esA) - 1);
            var expA = (sbyte)((tmp >> (psA - 1 - esA)) & expMask);

            const ushort signMask = 1 << (psA - 1);
            var fracA = signMask | (tmp << esA); // add silent 1
            var fracZ = (ulong)fracA << (64 - psA);
            var scaleA = expA + k * (1 << esA);
            var shiftA = (psA - 1) - scaleA;
            //var uiZ = fracA >> shiftA;
            var shiftZ = 63 - scaleA;
            var uiZ = fracZ >> shiftZ;

            // Diagnostic output
            Console.WriteLine("        psA = {0}, esA = {1}", psA, esA);
            Console.WriteLine("        uiA = {0}", uiA.TopBits(psA));
            Console.WriteLine("            = {0}", uiA.ToFormula());
            Console.WriteLine("            = 0x{0:X4}", uiA);
            Console.WriteLine("          k = {0}", k);
            Console.WriteLine("        tmp = {0}", tmp.TopBits(psA));
            Console.WriteLine("       expA = {1} ({0})", expA, expA.TopBits(3));
            Console.WriteLine("      fracA = {0}", fracA.TopBits(psA));
            Console.WriteLine("     shiftA = {0}, scaleA = {1}", shiftA, scaleA);
            Console.WriteLine("      fracZ = {0}", fracZ.TopBits(32, 64));
            Console.WriteLine("     shiftZ = {0}", shiftZ);
            Console.WriteLine("      uiZ64 = {0}", uiZ.TopBits());

            if (shiftA > (k + 2 + esA)) // only shifts larger than this will have rounding
            {
                Debug.Assert((scaleA + 1) < 64);
                var bitNPlusOneMask = signMask >> (scaleA + 1);
                if ((bitNPlusOneMask & fracA) != 0)
                {
                    var bitLastMask = bitNPlusOneMask << 1;
                    var bitsMoreMask = bitNPlusOneMask - 1;
                    // logic for round to nearest, tie to even
                    if (((bitLastMask | bitsMoreMask) & fracA) != 0)
                    {
                        uiZ += 1;
                    }
                }
            }

            return uiZ;
        }

        private static (sbyte, ushort) splitRegimeP16(in ushort uiA)
        {
            Debug.Assert((short)uiA > 0, "must be positive");

            const int psA = 16;
            const ushort signMask = 1 << (psA - 1);

            var signOfRegime = (uiA & (signMask >> 1)) != 0;
            var tmp = uiA << 2;
            sbyte k;

            if (signOfRegime)
            {
                k = 0;
                while ((tmp & signMask) != 0)
                {
                    k++;
                    tmp <<= 1;
                }
            }
            else
            {
                k = -1;
                while ((tmp & signMask) == 0)
                {
                    k--;
                    tmp <<= 1;
                }
                tmp &= signMask - 1;
            }

            return (k, (ushort)tmp);
        }
    }
}
