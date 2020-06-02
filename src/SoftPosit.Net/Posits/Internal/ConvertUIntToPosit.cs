// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    internal static partial class Native
    {
        internal static ulong ConvertUIntToPosit(long iA, int nbits, int psZ, int esZ)
            => iA == 0 ? 0 : ConvertUIntToPositImpl(iA, nbits, psZ, esZ);

        internal static ulong ConvertUIntToPosit(ulong uiA, int nbits, int psZ, int esZ)
            => uiA == 0 ? 0 : ConvertUIntToPositImpl(uiA, nbits, psZ, esZ);

        private static ulong ConvertUIntToPositImpl(long iA, int nbits, int psZ, int esZ)
            => ConvertUIntToPositImpl((ulong)iA, nbits, psZ, esZ);

        private static ulong ConvertUIntToPositImpl(ulong uiA, int nbits, int psZ, int esZ)
        {
            Debug.Assert(uiA > 0, "must be positive");

            // output environment
            //Debug.Assert((psZ % 8) == 0, "must be a 8-bit aligned");
            Debug.Assert(psZ >= 2);
            Debug.Assert(esZ >= 0);

            // input environment
            Debug.Assert(nbits > 0);

            // Calculate the log2 of the input
            var log2 = nbits - 1;
            var fracA = uiA;
            var mask = 1ul << log2;
            Debug.Assert(nbits == 64 || (fracA & ~((1ul << nbits) - 1)) == 0, "Upper bits must be clear");
            Debug.Assert(fracA != 0, "Can cause an infinite loop");
            while ((fracA & mask) == 0)
            {
                log2--;
                fracA <<= 1;
            }
            fracA ^= mask; // clear silent one

            // Build the Posit
            var k = (sbyte)(log2 >> esZ);
            var signMaskZ = 1ul << (psZ - 1);
            var regimeZ = (signMaskZ - 1) ^ (((signMaskZ >> 1) - 1) >> k);
            var expZ = (ulong)(log2 % (1 << esZ)) << ((psZ - 1) - (k + 2) - esZ);
            var fracShift = (nbits - psZ) + (k + 2) + esZ;
            var fracZ = fracShift < 0 ? fracA << -fracShift : fracA >> fracShift;

            var uiZ = regimeZ | expZ | fracZ;

            // Diagnostic output
            Console.WriteLine("       log2 = {0}", log2);
            Console.WriteLine("          k = {0}", k);
            Console.WriteLine("     regime = {0}", regimeZ.TopBits(psZ));
            Console.WriteLine("        exp = {0} ({1})", expZ.TopBits(psZ), log2 % (1 << esZ));
            Console.WriteLine("       frac = {0} ({1})", fracZ.TopBits(psZ), fracShift < 0 ? $"<<{-fracShift}" : $">>{fracShift}");
            Console.WriteLine("        uiZ = {0}", uiZ.TopBits(psZ));
            Console.WriteLine();
            Console.WriteLine("      nbits = {0}", nbits);
            Console.WriteLine("      shift = {0}", fracShift);
            Console.WriteLine("         iA = {1} {0}", uiA, uiA.TopBits(nbits));
            Console.WriteLine("      fracA = {0}", fracA.TopBits(nbits));
            Console.WriteLine("       mask = {0}", mask.TopBits(nbits));

            // Rounding
            var bitNPlusOneShift = psZ - (k + 2) - esZ;
            if (!(bitNPlusOneShift < nbits))
            {
                Console.WriteLine("!! No rounding needed. Enough space to fit input");
            }
            else
            {
                var bitNPlusOneMask = mask >> bitNPlusOneShift;
                Console.WriteLine("bitNPlusOne = {1}, ({0})", bitNPlusOneShift, bitNPlusOneMask.TopBits(nbits));
                if ((bitNPlusOneMask & fracA) != 0)
                {
                    var bitLastMask = bitNPlusOneMask << 1;
                    var bitsMoreMask = bitNPlusOneMask - 1;
                    Console.WriteLine("   bitLast  = {0}", bitLastMask.TopBits(nbits));
                    Console.WriteLine("   bitsMore = {0}", bitsMoreMask.TopBits(nbits));
                    // logic for round to nearest, tie to even
                    if (((bitLastMask | bitsMoreMask) & fracA) != 0)
                    {
                        uiZ++;
                        Console.WriteLine("      ++uiZ = {0}", uiZ.TopBits(psZ));
                    }
                }
            }

            return uiZ;
        }
    }
}