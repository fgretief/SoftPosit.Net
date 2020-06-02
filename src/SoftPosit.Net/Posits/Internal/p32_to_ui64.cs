// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit32_t = Posit32;

    internal static partial class Native
    {
        public static ulong p32_to_ui64(in posit32_t value)
        {
            if ((int)value.ui <= 0)
            {
                return 0; // Zero, NaR, negative values
            }

            var uiA = value.ui;

            if (uiA <= 0x3800_0000u) return 0; // 0 <= |pA| <= 1/2 rounds to zero.
            if (uiA <  0x4400_0000u) return 1; // 1/2 < x < 3/2 rounds to 1.
            if (uiA <= 0x4600_0000u) return 2; // 3/2 <= x <= 5/2 rounds to 2. // For speed. Can be commented out
            if (uiA >= 0x7FFF_C000u)           // 2^64 <= x <= inf
            {
                // overflow, so return max integer value
                return ulong.MaxValue; // UINT_MAX
            }

            return convertP32ToUI64(uiA);
        }

        private static ulong convertP32ToUI64(uint uiA)
        {
            var (k, tmp) = splitRegimeP32(uiA);

            // input environment
            const int psA = 32;
            const int esA = 2;

            // output environment
            const int nbits = 64;

            const int expMask = ((1 << esA) - 1);
            var expA = (sbyte)((tmp >> (psA - 1 - esA)) & expMask);

            const uint signMask = 1u << (psA - 1);
            var fracA = signMask | (tmp << esA); // add silent 1
            var fracZ = (ulong)fracA << (nbits - psA);
            var scaleA = expA + k * (1 << esA);
            var shiftZ = (nbits - 1) - scaleA;
            var uiZ = fracZ >> shiftZ;

            // Diagnostic output
            Console.WriteLine("        psA = {0}, esA = {1}", psA, esA);
            Console.WriteLine("        uiA = {0}", uiA.TopBits(psA));
            Console.WriteLine("            = {0}", uiA.ToFormula());
            Console.WriteLine("            = 0x{0:X8}", uiA);
            Console.WriteLine("          k = {0}", k);
            Console.WriteLine("        tmp = {0}", tmp.TopBits(psA));
            Console.WriteLine("       expA = {1} ({0})", expA, expA.TopBits(3));
            Console.WriteLine("      fracA = {0}", fracA.TopBits(psA));
            Console.WriteLine("     scaleA = {0}", scaleA);
            Console.WriteLine("     shiftZ = {0}", shiftZ);
            Console.WriteLine("      fracZ = {0}", fracZ.TopBits());
            Console.WriteLine("      uiZ64 = {0}", uiZ.TopBits());
            Console.WriteLine("            = 0x{0:X16}", uiZ);
            Console.WriteLine("            = {0}", uiZ);

            var shiftA = (psA - 1) - scaleA;
            if (shiftA > ((k+2) + esA)) // only shifts larger than this will have rounding
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

        private static (sbyte, uint) splitRegimeP32(in uint uiA)
        {
            Debug.Assert((int)uiA > 0, "must be positive");

            const int psA = 32;
            const uint signMask = 1u << (psA - 1);

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

            return (k, tmp);
        }
    }
}
