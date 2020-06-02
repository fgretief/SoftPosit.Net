// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit64_t = Posit64;

    internal static partial class Native
    {
        public static posit64_t ui32_to_p64(in uint value)
        {
            if (value < 2)
            {
                return new Posit64((ulong)value << 62); // 0 and 1
            }

            // output environment
            const int psZ = 64;
            const int esZ = 3;

            var log2 = psZ-1;
            var fracA = (ulong)value;
            var mask = (1ul << log2);
            Debug.Assert(fracA != 0, "Can cause an infinite loop");
            while ((fracA & mask) == 0)
            {
                log2--;
                fracA <<= 1;
            }

            var k = (sbyte)(log2 >> esZ);
            var exp = (ulong)log2 % (1 << esZ) << ((psZ-1) - (k+2) - esZ);
            var frac = (fracA & ~mask) >> (k+2 + esZ);
            var regime = 0x7FFF_FFFF_FFFF_FFFFul ^ (0x3FFF_FFFF_FFFF_FFFFul >> k);

            var uiA = regime | exp | frac;

            Debug.Assert(uiA == ConvertUIntToPositImpl(value, 32, 64, 3));

            return new Posit64(uiA);
        }

    }
}